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
        ComputeShader postProcessCS;
        ComputeBuffer networkInputBuffer;
        public RenderTexture outputTexture;
        public RenderTexture outputTexture2;

        public SelfieSegmentation(SelfieSegmentationResource resource){
            preProcessCS = resource.preProcessCS;
            postProcessCS = resource.postProcessCS;

            networkInputBuffer = new ComputeBuffer(width * height * in_ch, sizeof(float));

            outputTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
            outputTexture2 = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
            
            model = ModelLoader.Load(resource.model);
            woker = model.CreateWorker();
        }

        public void ProcessImage(Texture inputTexture){
            // Resize `inputTexture` texture to network model image size.
            preProcessCS.SetTexture(0, "_inputTexture", inputTexture);
            preProcessCS.SetBuffer(0, "_output", networkInputBuffer);
            preProcessCS.Dispatch(0, width / 8, height / 8, 1);

            //Execute neural network model.
            var inputTensor = new Tensor(1, height, width, in_ch, networkInputBuffer);
            woker.Execute(inputTensor);
            inputTensor.Dispose();

            var segTemp = CopyOutputToTempRT("activation_10", width, height);
            var postTemp = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
            postTemp.enableRandomWrite = true;
            postTemp.Create();

            postProcessCS.SetTexture(0, "_inputTexture", segTemp);
            postProcessCS.SetTexture(0, "_output", postTemp);
            postProcessCS.Dispatch(0, width / 8, height / 8, 1);

            if(outputTexture.width != inputTexture.width || outputTexture.height != inputTexture.height){
                outputTexture?.Release();
                outputTexture = new RenderTexture(inputTexture.width, inputTexture.height, 0, RenderTextureFormat.ARGB32);

                outputTexture2?.Release();
                outputTexture2 = new RenderTexture(inputTexture.width, inputTexture.height, 0, RenderTextureFormat.ARGB32);
            }

            Graphics.Blit(segTemp, outputTexture);
            Graphics.Blit(postTemp, outputTexture2);
            
            RenderTexture.ReleaseTemporary(segTemp);
            postTemp.Release();
        }

        public void Dispose(){
            networkInputBuffer?.Dispose();
            woker?.Dispose();
            outputTexture?.Release();
            outputTexture2?.Release();
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