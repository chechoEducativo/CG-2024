using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DistanceFog : VolumeComponent
{
    public AnimationCurveParameter strengthByDistance =
        new AnimationCurveParameter(new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1)));

    public ColorParameter fogColor = new ColorParameter(Color.white);
}
