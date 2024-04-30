using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PostProcessHandler : ScriptableRendererFeature
{
    private DistanceFogPass pass;
    
    public override void Create()
    {
        pass = new DistanceFogPass();
        pass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
       renderer.EnqueuePass(pass);
    }
}
