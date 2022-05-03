Thank you for purchasing the Stylized Town. We hope you enjoy our 3D environments.

This package uses linear color space (Edit->Project Settings->Player->Color Space) and the deferred render path (Edit->Project Settings->Graphics->Rendering Path). Please change your project settings accordingly.

To achieve the same visuals as shown in the screenshots install the Post Processing stack via the Windows->Packet Manager. 
Follow the Unity docs to set up the Postprocessing stack: https://unity3d.com/how-to/set-up-post-processing-stack
You find the appropriate profiles under Scenes/SampleScene(Day|Night)_Profiles

This package comes with a few custom shaders. In URP and HDRP you can edit these shaders with Shadergraph. 

Unfortunately, there is no unified way in Unity to provide Customers with a simple Renderpipline experience. Therefore some manual steps are required.

Using the HDRP version.
1. Create a new HDRP Unity project using Unity 2019.4 or higher. 
2. Delete ALL the example content in the project and hierarchy window!
3. Import the HDRP package found in HDRP-URP-Package folder.
4. If the scene still looks pink assign the HDRP Asset (StylizedTown/Settings/HDRenderPipelineAsset) in ProjectSettings->Graphics->Scriptable Render Pipeline Settings
Do not double click the HDRP package in the current Standard project. Manuell Renderpipeline conversion is not necessary.


Using the URP version.
1. Create a new URP Unity project using Unity 2019.4 or higher. 
2. Delete ALL the example content in the project and hierarchy window!
3. Import the URP package found in HDRP-URP-Package folder.
4. If the scene still looks pink assign the URP Asset (StylizedTown/URPRenderPipelineAsset) in ProjectSettings->Graphics->Scriptable Render Pipeline Settings
Do not double click the URP package in the current Standard project. Manuell Renderpipeline conversion is not necessary.

Thank you for reading this file. In case you still have questions don't hesitate to contact us.

Best Triplebrick