# SelfieSegmentationBarracuda
![demo_segmentation](/screenshot/demo_segmentation.gif)
![demo_virtual_background](/screenshot/demo_virtual_background.gif)

**SelfieSegmentationBarracuda** is a human segmentation neural network that works with a monocular color camera.

SelfieSegmentationBarracuda is a Unity Package that runs the [Mediapipe Selfie Segmentation](https://google.github.io/mediapipe/solutions/selfie_segmentation) on the [Unity Barracuda](https://docs.unity3d.com/Packages/com.unity.barracuda@latest).

## Install
SelfieSegmentationBarracuda can be installed from npm or GitHub URL.

### Install from npm (Recommend)
SelfieSegmentationBarracuda can be installed by adding following sections to the manifest file (`Packages/manifest.json`).

To the `scopedRegistries` section:
```
{
  "name": "creativeikep",
  "url": "https://registry.npmjs.com",
  "scopes": [ "jp.ikep" ]
}
```
To the `dependencies` section:
```
"jp.ikep.mediapipe.selfiesegmentation": "1.0.1"
```
Finally, the manifest file looks like below:
```
{
    "scopedRegistries": [
        {
            "name": "creativeikep",
            "url": "https://registry.npmjs.com",
            "scopes": [ "jp.ikep" ]
        }
    ],
    "dependencies": {
        "jp.ikep.mediapipe.selfiesegmentation": "1.0.1",
        ...
    }
}
```

### Install from GitHub URL
SelfieSegmentationBarracuda can be installed by adding below URL on the Unity Package Manager's window
```
https://github.com/creativeIKEP/SelfieSegmentationBarracuda.git?path=Packages/SelfieSegmentationBarracuda#v1.0.1
```
or, adding below sentence to your manifest file(`Packages/manifest.json`) `dependencies` block. Example is below.
```
{
  "dependencies": {
    "jp.ikep.mediapipe.selfiesegmentation": "https://github.com/creativeIKEP/SelfieSegmentationBarracuda.git?path=Packages/SelfieSegmentationBarracuda#v1.0.1",
    ...
  }
}

```

## Usage Demo
Below code is the demo that segment the prominent humans in the scene.
Check ["/Assets/Sample/SegmentationSample/Script/SegmentationVisuallizer.cs"](/Assets/Sample/SegmentationSample/Script/SegmentationVisuallizer.cs) and ["/Assets/Sample/SegmentationSample/Scenes/SegmentationSample.unity"](/Assets/Sample/SegmentationSample/Scenes/SegmentationSample.unity) for SelfieSegmentationBarracuda usage demo details.

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

## Demo Image
- The video for demo scene was downloaded from [here](https://www.pexels.com/ja-jp/photo/7261928/).
- The image for virtual background demo scene was downloaded from [here](https://pixabay.com/images/id-5637906/).

## ONNX Model
The ONNX model files have been downloaded from [PINTO_model_zoo](https://github.com/PINTO0309/PINTO_model_zoo) > [109_Selfie_Segmentation](https://github.com/PINTO0309/PINTO_model_zoo/tree/main/109_Selfie_Segmentation).

## Author
[IKEP](https://ikep.jp)

## LICENSE
Copyright (c) 2021 IKEP

[Apache-2.0](/LICENSE.md)
