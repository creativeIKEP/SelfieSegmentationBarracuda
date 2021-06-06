using UnityEngine;
using UnityEngine.UI;
using Mediapipe.SelfieSegmentation;

public class Visuallizer : MonoBehaviour
{
    [SerializeField] WebCamInput webCamInput;
    [SerializeField] RawImage inputImageUI;
    [SerializeField] RawImage segmentationImage;
    [SerializeField] SelfieSegmentationResource resource;

    SelfieSegmentation segmentation;

    void Start(){
        segmentation = new SelfieSegmentation(resource);
    }

    void LateUpdate(){
        inputImageUI.texture = webCamInput.inputImageTexture;
        segmentation.ProcessImage(webCamInput.inputImageTexture);
    } 

    void OnRenderObject(){
        segmentationImage.texture = segmentation.outputTexture;
    }

    void OnApplicationQuit(){
        segmentation.Dispose();
    }
}
