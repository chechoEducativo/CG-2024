using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformData
{
    public Matrix4x4 objectToWorld;
    public Matrix4x4 worldToObject;

    public TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        objectToWorld = Matrix4x4.TRS(position, rotation, scale);
        worldToObject = objectToWorld.inverse;
    }
    
    public TransformData(Transform source, params TransformData[] children)
    {
        objectToWorld = source.localToWorldMatrix;
        worldToObject = source.worldToLocalMatrix;

        this.children = new List<TransformData>();
        
        this.children.AddRange(children);
    }

    public Vector3 Position => new Vector3(objectToWorld.m03, objectToWorld.m13, objectToWorld.m23);
    public Quaternion Rotation => objectToWorld.rotation;
    public Vector3 EulerAngles => objectToWorld.rotation.eulerAngles;
    public Vector3 WorldScale => objectToWorld.lossyScale;

    public List<TransformData> children;
}
