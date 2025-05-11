using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MazeGenerator))]
public class MazeGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MazeGenerator generator = (MazeGenerator)target;

        if (GUILayout.Button("Generate Maze"))
        {
            generator.GenerateMaze();
        }

        if (GUILayout.Button("Clear Last Generated Maze"))
        {
            generator.ClearLastGeneratedMaze();
        }
    }
}
