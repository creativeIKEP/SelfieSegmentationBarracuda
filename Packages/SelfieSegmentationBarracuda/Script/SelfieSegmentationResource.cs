using UnityEngine;
using Unity.Barracuda;

namespace Mediapipe.SelfieSegmentation{
    [CreateAssetMenu(fileName = "SelfieSegmentationResource", menuName = "ScriptableObjects/Selfie Segmentation Resource")]
    public class SelfieSegmentationResource : ScriptableObject
    {
        public NNModel model;
        public ComputeShader preProcessCS;
        public ComputeShader postProcessCS;
    }
}