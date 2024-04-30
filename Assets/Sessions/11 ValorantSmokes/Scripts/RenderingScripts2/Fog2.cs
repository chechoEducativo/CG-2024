using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Fog2 : ScriptableRenderPass
{
    private RTHandle postProcessTempTexture;
    private Material renderingMaterial;
    public Fog2(Material renderingMaterial)
    {
        this.renderingMaterial = renderingMaterial;
    }
    
    // This method is called before executing the render pass.
    // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
    // When empty this render pass will render to the active camera render target.
    // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
    // The render pipeline will ensure target setup and clearing happens in a performant manner.
    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        if (renderingMaterial == null) return;
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
        descriptor.depthBufferBits = 0;
        RenderingUtils.ReAllocateIfNeeded(ref postProcessTempTexture, descriptor);
    }

    // Here you can implement the rendering logic.
    // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
    // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
    // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (renderingMaterial == null) return;
        
        VolumeManager manager = VolumeManager.instance;
        VolumeStack stack = manager.stack;
        ValorantSmokeFog fogData = stack.GetComponent<ValorantSmokeFog>();
        if (fogData == null || !fogData.IsActive()) return;
        
        renderingMaterial.SetFloat("_Weight", fogData.weight.value);
        renderingMaterial.SetColor("_Color", fogData.color.value);
        renderingMaterial.SetVector("_FogParameters", new Vector4(fogData.fogStart.value, fogData.fogEnd.value,0,0));
        CommandBuffer cmd = CommandBufferPool.Get("Valorant Smokes");
        RTHandle screenTexture = renderingData.cameraData.renderer.cameraColorTargetHandle;
        cmd.Blit(screenTexture, postProcessTempTexture);
        cmd.Blit(postProcessTempTexture, screenTexture, renderingMaterial, renderingMaterial.FindPass("Universal Forward"));
        context.ExecuteCommandBuffer(cmd);
        cmd.Release();
    }

    // Cleanup any allocated resources that were created during the execution of this render pass.
    public override void OnCameraCleanup(CommandBuffer cmd)
    {
    }
}