using UnityEngine;
using UnityEngine.UI;
using Mediapipe.SelfieSegmentation;

public class SegmentationVisuallizer : MonoBehaviour
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
        // Predict segmentation by neural network model.
        segmentation.ProcessImage(webCamInput.inputImageTexture);
        // Visualize segmentation texture as UI image.
        segmentationImage.texture = segmentation.texture;
    } 

    void OnApplicationQuit(){
        segmentation.Dispose();
    }
}
