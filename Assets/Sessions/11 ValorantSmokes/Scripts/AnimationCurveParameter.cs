using UnityEngine;
using UnityEngine.Rendering;


public class AnimationCurveParameter : VolumeParameter<AnimationCurve>
{
    public AnimationCurveParameter(AnimationCurve val)
    {
        value = val;
    }
}
