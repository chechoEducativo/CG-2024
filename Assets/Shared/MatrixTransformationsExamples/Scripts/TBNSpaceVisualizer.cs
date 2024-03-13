using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TBNSpaceVisualizer : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] private float deformAmount;
    [SerializeField] private Material deformMaterial;

    private MeshFilter meshFilter;

    private List<Vector3> normals = new List<Vector3>();
    private List<Vector3> tangents = new List<Vector3>();
    private List<Vector3> bitangents = new List<Vector3>();

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

    private void OnValidate()
    {
        DrawData();
        if (deformMaterial != null)
        {
            deformMaterial.SetFloat("_DeformationAmount", deformAmount);
        }
    }

    private void DrawData()
    {
        normals.Clear();
        tangents.Clear();
        bitangents.Clear();
        Mesh sharedMesh = MeshFilter.sharedMesh;
        for (int i = 0; i < sharedMesh.vertexCount; i++)
        {
            Vector3 nonDeformedPosition = sharedMesh.vertices[i];
            Vector2 uv = sharedMesh.uv[i] * new Vector2(2,2) - new Vector2(1,1);
            Vector3 deformedPosition = new Vector3(uv.x, uv.y, 0);
            Vector3 normal = Vector3.Lerp(sharedMesh.normals[i], Vector3.forward, deformAmount);
            Vector3 startPos = Vector3.Lerp(nonDeformedPosition, deformedPosition, deformAmount);
            
            
            normals.Add(startPos);
            normals.Add(startPos + normal * 0.1f);

            Vector3 tangent = Vector3.Lerp(sharedMesh.tangents[i], -Vector3.right, deformAmount);
            tangents.Add(startPos);
            tangents.Add(startPos + tangent * 0.1f);
            
            bitangents.Add(startPos);
            bitangents.Add(startPos + Vector3.Cross(tangent,normal) * 0.1f);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.blue;
        Gizmos.DrawLineList(normals.ToArray());
        
        Gizmos.color = Color.red;
        Gizmos.DrawLineList(tangents.ToArray());
        
        Gizmos.color = Color.green;
        Gizmos.DrawLineList(bitangents.ToArray());

    }
#endif
}
