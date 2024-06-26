using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomPostProcessHandler : ScriptableRendererFeature
{
    Fog2 m_ScriptablePass;

    [SerializeField] private Material valorantSmokeMaterial;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new Fog2(valorantSmokeMaterial);

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


