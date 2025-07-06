using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class ColliderUpdater : MonoBehaviour
{
    void Awake()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        MeshCollider meshCollider = GetComponent<MeshCollider>();

        if (meshFilter != null && meshCollider != null && meshFilter.sharedMesh != null)
        {
            meshCollider.sharedMesh = meshFilter.sharedMesh;
            Debug.Log("Mesh Collider successfully updated at runtime!");
        }
        else
        {
            Debug.LogError("ColliderUpdater Error: Could not find MeshFilter/MeshCollider, or the mesh was empty.");
        }
    }
}