using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(PostProcessPixelRenderer), PostProcessEvent.BeforeStack, "GGS/Post Process Pixel")]
public sealed class PostProcessPixel : PostProcessEffectSettings
{
    [Tooltip("The size of a pixel")]
    public IntParameter scale = new IntParameter { value = 64 };

    [Tooltip("The color steps")]
    public IntParameter steps = new IntParameter { value = 8 };
}

public sealed class PostProcessPixelRenderer : PostProcessEffectRenderer<PostProcessPixel>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/GGS/Pixel Post Process"));
        sheet.properties.SetFloat("_Steps", settings.steps);
        sheet.properties.SetFloat("_Scale", settings.scale);

        Matrix4x4 clipToView = GL.GetGPUProjectionMatrix(context.camera.projectionMatrix, true).inverse;
        sheet.properties.SetMatrix("_ClipToView", clipToView);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}