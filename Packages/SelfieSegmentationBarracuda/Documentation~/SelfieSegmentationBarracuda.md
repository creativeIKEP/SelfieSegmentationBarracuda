# SelfieSegmentationBarracuda Usage Documentation
### Install
SelfieSegmentationBarracuda can be installed by adding below URL on the Unity Package Manager's window
```
https://github.com/creativeIKEP/SelfieSegmentationBarracuda.git?path=Packages/SelfieSegmentationBarracuda#v1.0.0
```
or, adding below sentence to your manifest file(`Packages/manifest.json`) `dependencies` block. Example is below.
```
{
  "dependencies": {
    "jp.ikep.mediapipe.selfiesegmentation": "https://github.com/creativeIKEP/SelfieSegmentationBarracuda.git?path=Packages/SelfieSegmentationBarracuda#v1.0.0",
    ...
  }
}

```

### Usage Demo
Below code is the demo that segment the prominent humans in the scene.
Check ["/Assets/Sample/SegmentationSample/Script/SegmentationVisuallizer.cs"](https://github.com/creativeIKEP/SelfieSegmentationBarracuda/blob/main/Assets/Sample/SegmentationSample/Script/SegmentationVisuallizer.cs) and ["/Assets/Sample/SegmentationSample/Scenes/SegmentationSample.unity"](https://github.com/creativeIKEP/SelfieSegmentationBarracuda/blob/main/Assets/Sample/SegmentationSample/Scenes/SegmentationSample.unity) for SelfieSegmentationBarracuda usage demo details.

```cs
using UnityEngine;
// Import SelfieSegmentationBarracuda package
using Mediapipe.SelfieSegmentation;

public class <YourClassName>: MonoBehaviour
{
  // Set "Packages/SelfieSegmentationBarracuda/ResourceSet/SelfieSegmentationResource.asset" on the Unity Editor.
  [SerializeField] SelfieSegmentationResource resource;

  SelfieSegmentation segmentation;

  void Start(){
      segmentation = new SelfieSegmentation(resource);
  }

  void Update(){
      Texture input = ...; // Your input image texture

      // Predict segmentation by neural network model.
      segmentation.ProcessImage(input);
      // Segmentation results can be obtained with `SelfieSegmentation.texture`.
      Texture result = segmentation.texture;
  }

  void OnApplicationQuit(){
      // Must call Dispose method when no longer in use.
      segmentation.Dispose();
  }
}
```
