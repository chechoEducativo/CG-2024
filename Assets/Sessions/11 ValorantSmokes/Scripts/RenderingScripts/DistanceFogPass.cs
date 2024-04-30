using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DistanceFogPass : ScriptableRenderPass
{
    private const string SHADER_PATH = "DistanceDepth";
    
    private RTHandle postProcessTemp;

    private Material renderingMaterial;
    
    private Material RenderingMaterial
    {
        get
        {
            if (renderingMaterial == null)
            {
                renderingMaterial = new Material(Resources.Load<Shader>(SHADER_PATH));
            }

            return renderingMaterial;
        }
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        RenderTextureDescriptor colDescr = renderingData.cameraData.cameraTargetDescriptor;
        RenderingUtils.ReAllocateIfNeeded(ref postProcessTemp, colDescr);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get("Distance Depth");

        RTHandle camColorTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;
        cmd.Blit(camColorTarget, postProcessTemp);
        cmd.Blit(postProcessTemp, camColorTarget, RenderingMaterial, RenderingMaterial.FindPass("Universal Forward"));
        
        context.ExecuteCommandBuffer(cmd);
        cmd.Release();
    }
}
