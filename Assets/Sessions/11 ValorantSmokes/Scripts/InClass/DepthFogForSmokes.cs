using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
public class DepthFogForSmokes : VolumeComponent, IPostProcessComponent
{
    public ClampedFloatParameter weight = new ClampedFloatParameter(0, 0, 1);
    public FloatParameter nearDistance = new FloatParameter(0);
    public FloatParameter farDistance = new FloatParameter(10);
    public ColorParameter fogColor = new ColorParameter(new Color(0.29f, 0.69f, 0f));

    public bool IsActive() => true;
    public bool IsTileCompatible() => true;
}
