using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ValorantSmokeFog : VolumeComponent, IPostProcessComponent
{
    public ClampedFloatParameter weight = new ClampedFloatParameter(1, 0, 1);
    public ColorParameter color = new ColorParameter(Color.white);
    public FloatParameter fogStart = new FloatParameter(3);
    public FloatParameter fogEnd = new FloatParameter(5);

    public bool IsActive() => true;

    public bool IsTileCompatible() => true;
}
