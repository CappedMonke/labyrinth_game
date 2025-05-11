using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WallCombinerTarget))]
public class MeshCombinerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Combine Meshes and Collider"))
        {
            CombineMeshes((WallCombinerTarget)target);
        }
    }

    private void CombineMeshes(WallCombinerTarget target)
    {
        Transform root = target.transform;
        MeshFilter[] meshFilters = root.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        Mesh combinedMesh = new Mesh { name = "CombinedMesh" };
        combinedMesh.CombineMeshes(combine, true, true);

        // Save asset
        string path = "Assets/CombinedMesh.asset";
        AssetDatabase.CreateAsset(combinedMesh, path);
        Debug.Log($"Combined mesh saved to: {path}");

        // Apply to parent object
        MeshFilter mf = root.GetComponent<MeshFilter>();
        if (mf == null) mf = root.gameObject.AddComponent<MeshFilter>();
        mf.sharedMesh = combinedMesh;

        MeshRenderer mr = root.GetComponent<MeshRenderer>();
        if (mr == null) mr = root.gameObject.AddComponent<MeshRenderer>();
        mr.sharedMaterial = meshFilters[1]?.GetComponent<MeshRenderer>()?.sharedMaterial;

        MeshCollider mc = root.GetComponent<MeshCollider>();
        if (mc == null) mc = root.gameObject.AddComponent<MeshCollider>();
        mc.sharedMesh = combinedMesh;
        mc.convex = false;
    }
}