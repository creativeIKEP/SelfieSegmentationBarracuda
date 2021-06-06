using UnityEngine;
using Unity.Barracuda;

namespace Mediapipe.SelfieSegmentation{
    public class SelfieSegmentation: System.IDisposable
    {
        Model model;
        IWorker woker;

        int width = 256;
        int height = 256; 
        int in_ch = 3;
        int out_ch = 1;
        ComputeShader preProcessCS;
        ComputeBuffer networkInputBuffer;
        public RenderTexture outputTexture;

        public SelfieSegmentation(SelfieSegmentationResource resource){
            preProcessCS = resource.preProcessCS;

            networkInputBuffer = new ComputeBuffer(width * height * in_ch, sizeof(float));

            outputTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
            // outputTexture.enableRandomWrite = true;
            // outputTexture.Create();
            
            model = ModelLoader.Load(resource.model);
            woker = model.CreateWorker();
        }

        public void ProcessImage(Texture inputTexture){
            // Resize `inputTexture` texture to network model image size.
            preProcessCS.SetTexture(0, "_inputTexture", inputTexture);
            preProcessCS.SetBuffer(0, "_output", networkInputBuffer);
            preProcessCS.Dispatch(0, width / 8, height / 8, 1);

            //Execute neural network model.
            // var inputTensor = new Tensor(1, height, width, in_ch, networkInputBuffer);
            var inputTensor = new Tensor(inputTexture, in_ch);
            woker.Execute(inputTensor);
            inputTensor.Dispose();

            var segTemp = CopyOutputToTempRT("activation_10", width, height);
            Graphics.Blit(segTemp, outputTexture);
            RenderTexture.ReleaseTemporary(segTemp);
        }

        public void Dispose(){
            networkInputBuffer?.Dispose();
            woker?.Dispose();
            outputTexture?.Release();
        }

        RenderTexture CopyOutputToTempRT(string name, int w, int h)
        {
            var rtFormat = RenderTextureFormat.ARGB32;
            var shape = new TensorShape(1, h, w, out_ch);
            var rt = RenderTexture.GetTemporary(w, h, 0, rtFormat);
            var tensor = woker.PeekOutput(name).Reshape(shape);
            tensor.ToRenderTexture(rt);
            tensor.Dispose();
            return rt;
        }
    }
}