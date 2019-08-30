# HLSL Camera

HLSL Camera is a tool to visualize the effects of the changes in your HLSL code as you edit it. The shader effect is applied to either a camera feed or an image of your choice.

The application uses and requires DirectX's Effect-Compiler tool (fxc.exe) to compile HLSL shaders. This tool is part of the DirectX SDK and as of Windows 8, the DirectX SDK is part of the Windows SDK.

The compiled WPF application can be downloaded from the [release tab](https://github.com/Vanlalhriata/HlslCamera/releases/).

### Requirements:
- .NET 4.8
- DirectX SDK (for the Effect-Compiler tool)

### Usage:
- Make sure the path to `fxc.exe` is correct.
- Paste your HLSL code into the text area and click on Execute. This will compile the code and apply the resulting shader on the adjacent image or video.
- The compiled pixel shader is stored in the application folder as `Shader.ps`

### Notes:
- The application closely follows the [HLSLTester application]([http://blogs.microsoft.co.il/tamir/2008/06/17/hlsl-pixel-shader-effects-tutorial/](http://blogs.microsoft.co.il/tamir/2008/06/17/hlsl-pixel-shader-effects-tutorial/)) developed by Tamir Khason.