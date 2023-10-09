using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using NaughtyAttributes.Editor;



//[EditorWindowTitle(title ="Inspector Rule")]
public class RuleInspector : EditorWindow
{
    public string myString = "Hello World";

    [SerializeField] public SerializedObject SelectedRules; 
        //Selection.activeContext.GameObject().GetComponent<Rules>().ConvertTo<SerializedProperty>();
    
        


    [MenuItem("Window/Room rule")]
    public static void ShowWindow()
    {
        GetWindow(typeof(RuleInspector));
    }
    void OnGUI()
    {
        GUILayout.Label("Room Rule Settings \n", EditorStyles.boldLabel);

        EditorGUILayout.ObjectField(SelectedRules.ConvertTo<SerializedProperty>());
            
            //(Resources.LoadAll("Assets/Resources/Prefabs/Rooms/").ConvertTo<SerializedObject>());

        //EditorGUILayout.ObjectField(SelectedRules);
        EditorGUILayout.BeginToggleGroup("Optional Settings",true);
        {
            //EditorGUILayout.ObjectField(SelectedRules);


        }



        EditorGUILayout.EndToggleGroup();
    }
}
