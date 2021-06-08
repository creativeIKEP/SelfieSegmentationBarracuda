using UnityEngine;
using Unity.Barracuda;

namespace Mediapipe.SelfieSegmentation{
    public class SelfieSegmentation: System.IDisposable
    {
        // Segmentation result texutre. Resolution is the same as input texture.
        public RenderTexture texture;

        #region constant number
        // Input and output image size defined by neural network model.
        const int IMAGE_SIZE = 256; 
        // Input image channel defined by neural network model.
        const int IN_CH = 3;
        // Output image channel defined by neural network model.
        const int OUT_CH = 1;
        #endregion

        #region private variables
        Model model;
        IWorker woker;
        ComputeShader preProcessCS;
        ComputeBuffer networkInputBuffer;
        #endregion

        #region public methods
        public SelfieSegmentation(SelfieSegmentationResource resource){
            preProcessCS = resource.preProcessCS;

            networkInputBuffer = new ComputeBuffer(IMAGE_SIZE * IMAGE_SIZE * IN_CH, sizeof(float));
            // Initialize with the resolution of IMAGE_SIZE * IMAGE_SIZE, 
            // but resize it later according to the resolution of the input texture.
            texture = new RenderTexture(IMAGE_SIZE, IMAGE_SIZE, 0, RenderTextureFormat.ARGB32);
            
            // Prepare neural network model.
            model = ModelLoader.Load(resource.model);
            woker = model.CreateWorker();
        }

        public void ProcessImage(Texture inputTexture){
            // Resize `inputTexture` texture to network model image size.
            preProcessCS.SetTexture(0, "_inputTexture", inputTexture);
            preProcessCS.SetBuffer(0, "_output", networkInputBuffer);
            preProcessCS.Dispatch(0, IMAGE_SIZE / 8, IMAGE_SIZE / 8, 1);

            // Execute neural network model.
            var inputTensor = new Tensor(1, IMAGE_SIZE, IMAGE_SIZE, IN_CH, networkInputBuffer);
            woker.Execute(inputTensor);
            inputTensor.Dispose();

            // Get segmentation output as RenderTexture.
            var segTemp = CopyOutputToTempRT("activation_10", IMAGE_SIZE, IMAGE_SIZE, OUT_CH);
            
            if(texture.width != inputTexture.width || texture.height != inputTexture.height){
                // Resize to the same resolution as input texture.
                texture?.Release();
                texture = new RenderTexture(inputTexture.width, inputTexture.height, 0, RenderTextureFormat.ARGB32);
            }

            // Render to segmentation texture to output texture.
            Graphics.Blit(segTemp, texture);
            
            RenderTexture.ReleaseTemporary(segTemp);
        }

        public void Dispose(){
            networkInputBuffer?.Dispose();
            woker?.Dispose();
            texture?.Release();
        }
        #endregion

        RenderTexture CopyOutputToTempRT(string name, int w, int h, int ch)
        {
            var rtFormat = RenderTextureFormat.ARGB32;
            var shape = new TensorShape(1, h, w, ch);
            var rt = RenderTexture.GetTemporary(w, h, 0, rtFormat);
            var tensor = woker.PeekOutput(name).Reshape(shape);
            tensor.ToRenderTexture(rt);
            tensor.Dispose();
            return rt;
        }
    }
}
