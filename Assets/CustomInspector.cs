using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(LevelGenerator))]
public class CustomInspector : Editor
{
    public override void  OnInspectorGUI()
    {
        EditorGUILayout.LabelField("For GD Use:");
        DrawDefaultInspector();

        LevelGenerator LG = (LevelGenerator)target;
        if (GUILayout.Button("Create New Wall")) { LG.RunByInspector(); }
    }
}
