using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomPostProcessManager : ScriptableRendererFeature
{
    class ScreenSpaceFog : ScriptableRenderPass
    {
        private Material renderingMaterial;
        
        private RTHandle tempRenderTexture; //Textura temporal para aplicar cambios con shader

        public ScreenSpaceFog(Material renderingMaterial)
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
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;
            RenderingUtils.ReAllocateIfNeeded(ref tempRenderTexture, descriptor);
        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            VolumeManager volumeManager = VolumeManager.instance;
            VolumeStack stack = volumeManager.stack;
            DepthFogForSmokes postProcessData = stack.GetComponent<DepthFogForSmokes>();
            if (postProcessData == null || !postProcessData.IsActive()) return;
            if (renderingMaterial == null) return;
            
            renderingMaterial.SetFloat("_Weight", postProcessData.weight.value);
            
            renderingMaterial.SetVector("_FogParemeters", new Vector4(
                postProcessData.nearDistance.value,
                postProcessData.farDistance.value
                ));
            
            renderingMaterial.SetColor("_FogColor", postProcessData.fogColor.value);
            
            CommandBuffer cmd = CommandBufferPool.Get("Screen Space Fog");
            RTHandle cameraTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;
            cmd.Blit(cameraTarget, tempRenderTexture);
            cmd.Blit(tempRenderTexture, cameraTarget, renderingMaterial, renderingMaterial.FindPass("Universal Forward"));
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public void Dispose()
        {
            if (tempRenderTexture != null) tempRenderTexture.Release();
        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }

    ScreenSpaceFog m_ScriptablePass;
    [SerializeField] private Material renderingMaterial;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new ScreenSpaceFog(renderingMaterial);

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


