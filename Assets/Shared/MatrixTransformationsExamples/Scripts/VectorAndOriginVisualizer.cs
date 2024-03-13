using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class VectorAndOriginVisualizer : MonoBehaviour
{
    [Serializable]
    public enum SpaceReference
    {
        Object,
        World,
        View
    }
    
    [SerializeField] private Camera referenceCamera;
    [SerializeField] private SpaceReference spaceReference;
    [SerializeField][Range(0,1)] private float vectorOpacity;
    [SerializeField] private bool visualize;

    private List<Vector3> lineVertices = new List<Vector3>();
    
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh sharedMesh;

    private MeshFilter MeshFilter
    {
        get
        {
            if (meshFilter == null)
            {
                meshFilter = GetComponent<MeshFilter>();
            }

            return meshFilter;
        }
    }
    
    private MeshRenderer MeshRenderer
    {
        get
        {
            if (meshRenderer == null)
            {
                meshRenderer = GetComponent<MeshRenderer>();
            }

            return meshRenderer;
        }
    }
    
    private Mesh SharedMesh
    {
        get
        {
            if (sharedMesh == null)
            {
                sharedMesh = MeshFilter.sharedMesh;
            }

            return sharedMesh;
        }
    }

    void SolveLines()
    {
        lineVertices.Clear();
        Vector3 origin = transform.position;
        switch (spaceReference)
        {
            case SpaceReference.World:
                origin = Vector3.zero;
                break;
            case SpaceReference.View:
                origin = referenceCamera.transform.position;
                break;
            default:
                break;
        }
        foreach (Vector3 vertex in SharedMesh.vertices)
        {
            lineVertices.Add(transform.TransformPoint(vertex));
            lineVertices.Add(origin);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!visualize) return;
        SolveLines();
        Gizmos.color = new Color(0,1,0,vectorOpacity);
        Gizmos.DrawLineStrip(lineVertices.ToArray(), false);
    }
#endif
}
