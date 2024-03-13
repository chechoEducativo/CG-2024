using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBaker : MonoBehaviour
{
    [Serializable]
    enum Axis
    {
        X,
        Y,
        Z
    }
    
    [SerializeField] private Transform positionTarget;
    [SerializeField] private Axis normalAxis = Axis.Z;
    [SerializeField] private bool negateNormalAxis;

    private List<TransformData> points;

    private Vector3 GetCurrentNormal()
    {
        Vector3 normal = positionTarget.right;
        switch (normalAxis)
        {
            case Axis.Y:
                normal = positionTarget.up;
                break;
            case Axis.Z:
                normal = positionTarget.forward;
                break;
        }

        normal *= negateNormalAxis ? -1 : 1;
        return normal;
    }
    
    private Vector3 GetCurrentTangent()
    {
        Vector3 tangent = positionTarget.forward;
        switch (normalAxis)
        {
            case Axis.Y:
                tangent = positionTarget.right;
                break;
            case Axis.Z:
                tangent = positionTarget.up;
                break;
        }

        tangent *= negateNormalAxis ? -1 : 1;
        return tangent;
    }

    public void CreateControlPoint()
    {
        if (positionTarget == null) return;

        Vector3 tangent = GetCurrentTangent();
        TransformData controlPoint = new TransformData(positionTarget, 
            new TransformData(positionTarget.position + tangent, positionTarget.rotation, Vector3.one),
            new TransformData(positionTarget.position - tangent, positionTarget.rotation, Vector3.one));
        points.Add(controlPoint);
    }
}
