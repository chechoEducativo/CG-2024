using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class ProjectionVizualizer : MonoBehaviour
{
    [SerializeField] private Camera cameraReference;

    [SerializeField] private Material renderingMaterial;
    [SerializeField][Range(0,1)] private float deformAmount;

    private void SetMaterialMatricesForTransformation()
    {
        Matrix4x4 proj = GL.GetGPUProjectionMatrix(cameraReference.projectionMatrix, false);
        
        renderingMaterial.SetMatrix("_ReferenceCamView", cameraReference.worldToCameraMatrix);
        renderingMaterial.SetMatrix("_ReferenceCamProjection", proj);

        float orthoSize = cameraReference.orthographicSize;
        
        Matrix4x4 overwrittenProj = Matrix4x4.Ortho(-orthoSize * cameraReference.aspect, orthoSize * cameraReference.aspect, -orthoSize, orthoSize, cameraReference.nearClipPlane, cameraReference.farClipPlane);
        overwrittenProj = GL.GetGPUProjectionMatrix(overwrittenProj, false);
        renderingMaterial.SetMatrix("_ReferenceCamInverseProjection", overwrittenProj.inverse);
        renderingMaterial.SetMatrix("_ReferenceCamInverseView", cameraReference.cameraToWorldMatrix);
        float n = cameraReference.nearClipPlane, f = cameraReference.farClipPlane;
        renderingMaterial.SetVector("_ReferenceCamZBufferParams", new Vector4((f-n)/n, 1, (f-n)/n*f, 1/f));
        renderingMaterial.SetFloat("_ApplyPerspectiveDeprojection", deformAmount);
    }

    private void Update()
    {
        if (renderingMaterial != null)
        {
            SetMaterialMatricesForTransformation();
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (cameraReference != null)
        {
            Gizmos.color = new Color(0, 1, 1, 1 - deformAmount);
            
            Gizmos.matrix = Matrix4x4.TRS(cameraReference.transform.position, cameraReference.transform.rotation,
                new Vector3(cameraReference.aspect, 1, 1));
            Gizmos.DrawFrustum(Vector3.zero, cameraReference.fieldOfView, cameraReference.farClipPlane,
                cameraReference.nearClipPlane, 1);
            

            Gizmos.matrix = Matrix4x4.TRS(cameraReference.transform.position, cameraReference.transform.rotation,
                new Vector3(cameraReference.aspect, 1, 1));

            Vector3 cubeCenter =
                Vector3.forward * (((cameraReference.farClipPlane - cameraReference.nearClipPlane) * 0.5f) + cameraReference.nearClipPlane);

            float zSize = cameraReference.farClipPlane - cameraReference.nearClipPlane;
            Gizmos.color = new Color(0, 1, 1, deformAmount);
            Gizmos.DrawWireCube(cubeCenter, new Vector3(cameraReference.orthographicSize * 2, cameraReference.orthographicSize * 2, zSize));
        }
    }
    
#endif
    
}
