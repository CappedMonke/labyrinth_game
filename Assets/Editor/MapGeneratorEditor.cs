using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGenerator = (MapGenerator)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Regenerate Labyrinth"))
        {
            if (Application.isPlaying)
            {
                mapGenerator.RegenerateLabyrinth();
            }
            else
            {
                Debug.LogWarning("Regenerate Labyrinth can only be used in Play Mode.");
            }
        }
    }
}