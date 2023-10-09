using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


public class Room : MonoBehaviour
{
    [Range(1,4)]
    [SerializeField] int _roomSize = 0;

    [SerializeField] List<GameObject> _objectList;
    [SerializeField] List<GameObject> _persoList;
    [SerializeField] List<GameObject> _EventList;

    public int RoomSize { get => _roomSize; }

#if UNITY_EDITOR
    [MenuItem("Assets/Create Room")]

    void PrefabCreator()
    {
        CreatePrefabInProject("RoomType1.prefab");
    }
    public static string CurrentProjectFolderPath
    {
        get
        {
            var projectWindowUtilType = typeof(ProjectWindowUtil);
            MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
            object obj = getActiveFolderPath.Invoke(null, new object[0]);
            return obj.ToString();
        }
    }
    private static void CreatePrefabInProject(string prefabName)
    {
        var prefab = UnityEngine.Resources.Load(prefabName);
        string targetPath = $"{CurrentProjectFolderPath}/{prefabName}.prefab";
        AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(prefab), targetPath);
    }
    private void OnValidate()
    {
        transform.localScale = new Vector3(RoomSize * 5, transform.localScale.y, 1);
    }
#endif
    public Room(DataRoom data)
    {
        
    }

}
