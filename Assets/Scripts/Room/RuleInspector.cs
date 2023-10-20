using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using NaughtyAttributes.Editor;
using Unity.VisualScripting.FullSerializer;
using System.Collections.Generic;



//[EditorWindowTitle(title ="Inspector Rule")]
public class RuleInspector : EditorWindow
{
    #region Sizes
    public Rect HeaderSize;
    public Rect MiddleSize;

    public Rect RoomSize;

    public Rect ButtonAddSize;
    public Rect ButtonRemoveSize;
    #endregion

    private List<int> _roomList;

    [MenuItem("Tools/Room rule")]
    public static void ShowWindow()
    {
        RuleInspector window = (RuleInspector)GetWindow(typeof(RuleInspector));
        window.minSize = new Vector2(800, 400);
        window.Show();
    }
    void OnGUI()
    {
        GuiLayout();
        DrawHeader();
        DrawMiddle();
        //GUI.DragWindow GUI.DragWindowGUI.DragWindowGUI.DragWindow
    }
    private void GuiLayout()
    {
        HeaderSize = new Rect(0, 0, Screen.width, 50);
        MiddleSize = new Rect(0,50, Screen.width, 300);
        //new Rect(0,350,);

        RoomSize = new Rect(0, MiddleSize.y + ButtonAddSize.y + 10, 50, 50);

        ButtonAddSize = new Rect(0 + 5, MiddleSize.y + 5, 75, 20);
        ButtonRemoveSize = new Rect(80 + 5, MiddleSize.y + 5, 100, 20);

    }
    private void DrawHeader()
    {
        GUI.color = Color.white;
        Texture2D texture = new Texture2D(1,1);
        texture.Apply();
        GUI.DrawTexture(HeaderSize, texture);
    }
    
    private void DrawMiddle()
    {
        GUI.color = Color.grey;
        Texture2D background = new Texture2D(1, 1);
        background.Apply();
        GUI.DrawTexture(MiddleSize, background);

        bool addRoom = GUI.Button(ButtonAddSize, "Add Room");
        if (addRoom) AddRoom();

        bool removeRoom = GUI.Button(ButtonRemoveSize, "Remove Room");
        if (removeRoom) RemoveRoom();

        int maxX = 0; 

        for(int i = 0; i < _roomList.Count; i++)
        {
            foreach (int j in _roomList) { maxX += j; }

            GUI.color = Color.Lerp(Color.red, Color.blue, (float)i/_roomList.Count);
            Texture2D texture = new Texture2D(1, 1);
            texture.Apply();
            RoomSize.x = 50 * i + 5 * i;
            GUI.DrawTexture(RoomSize, texture);
        }

    }

    void AddRoom()
    {
        _roomList.Add(1);
    }
    void RemoveRoom()
    {
        _roomList.Remove(1);
    }

    private void OnEnable()
    {
        _roomList = new List<int>();
    }
}
