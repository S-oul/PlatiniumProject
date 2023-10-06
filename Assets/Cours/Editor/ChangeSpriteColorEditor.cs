using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChangeSpriteColor))]
public class ChangeSpriteColorEditor : Editor
{
    private ChangeSpriteColor myObject = null;

    private void OnEnable()
    {
        this.myObject = (ChangeSpriteColor)this.target;  // "targeting" the ChangeSpriteColor class
    }

    public override void OnInspectorGUI() // this is a unity method that we will be rewriting. 
    {
        this.myObject.myText = EditorGUILayout.TextField("My Text", this.myObject.myText);

        //There are a lot of great tool to edit the Editor like rearanging the buttons.
        EditorGUILayout.BeginHorizontal();
        {
            this.name = EditorGUILayout.TextField("Name", this.name);
        }
        EditorGUILayout.EndHorizontal();
         
    }
}
