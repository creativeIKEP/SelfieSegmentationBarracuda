using UnityEngine;
using UnityEngine.UI;
using Mediapipe.SelfieSegmentation;

public class Visuallizer : MonoBehaviour
{
    [SerializeField] WebCamInput webCamInput;
    [SerializeField] RawImage inputImageUI;
    [SerializeField] RawImage segmentationImage;
    [SerializeField] Shader shader;
    [SerializeField] SelfieSegmentationResource resource;

    SelfieSegmentation segmentation;
    Material material;

    void Start(){
        segmentation = new SelfieSegmentation(resource);

        material = new Material(shader);
        segmentationImage.material = material;
    }

    void LateUpdate(){
        inputImageUI.texture = webCamInput.inputImageTexture;

        segmentation.ProcessImage(webCamInput.inputImageTexture);

        segmentationImage.texture = segmentation.outputTexture;
        material.SetTexture("_inputImage", webCamInput.inputImageTexture);
    } 

    void OnApplicationQuit(){
        segmentation.Dispose();
    }
}
