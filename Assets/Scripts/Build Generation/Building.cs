using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;
using Unity.VisualScripting.FullSerializer;

public class Building : MonoBehaviour
{
    #region Visible Variable 
    [SerializeField] private int _maxRooms = 3;
    [SerializeField] private int _maxFloors = 5;

    [Tooltip("The Space Between each floor")]
    [SerializeField] private float _heightBetweenFloor = 2f;

    [SerializeField] private List<GameObject> _poolType1 = new List<GameObject>();
    [SerializeField] private List<GameObject> _poolType2 = new List<GameObject>();
    [SerializeField] private List<GameObject> _poolType3 = new List<GameObject>();
    [SerializeField] private List<GameObject> _poolType4 = new List<GameObject>();

    [SerializeField] private List<List<GameObject>> _allPool = new List<List<GameObject>>();

    //[MenuItem("Assets/Create Room")]

    [SerializeField] private List<Floors> _floors = new List<Floors>();
    #endregion
    private void OnValidate()
    {
        _allPool.Add(_poolType1);
        _allPool.Add(_poolType2);
        _allPool.Add(_poolType3);
        _allPool.Add(_poolType4);
    }

  

    [Button("Sort Room")]
    public void SortingRoom()
    {
        Debug.Log("ayo");
    }


    void GenerateFloor(float height)
    {
        List<GameObject> rooms = new List<GameObject>();
        int currentRoom = 0;
        bool isfirst = true;

        while (currentRoom < _maxRooms)
        {
            int r = Random.Range(2, 5);
            while (currentRoom + r > _maxRooms)
            {
                r -= 1;
                if (r == 0)
                {
                    print("NOPE R = 0" );
                    return;
                }
            }

            GameObject go = Instantiate(_allPool[r-1][Random.Range(0, _allPool[r-1].Count)], transform);
            Room room = go.GetComponent<Room>();
            currentRoom += room.RoomSize;
            if (isfirst) 
            { 
                isfirst = false;
                go.transform.position = new Vector3(Mathf.Abs(room.transform.localScale.x/2), height, 0);
            }
            else
            {
                go.transform.position = new Vector3(rooms[^1].transform.localPosition.x + rooms[^1].transform.localScale.x/2 + go.transform.localScale.x/2, height, 0);
            }
            rooms.Add(go);
        }
    }

    #region Generate & Destroy
    [Button("Generate Building")]
    private void Generate()
    {
        for (float i = 0; i < _maxFloors; i++)
        {
            GenerateFloor(i * 5f + i * _heightBetweenFloor);

        }
    }




    [Button("DestroyALL")]
    public void DestroyALL()
    {
        for (int i = transform.childCount - 1; i != -1; i--)
        {
            print(i);
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }


    [Button("Destroy & Generate")]
    public void DestroyAndGenerate()
    {
        DestroyALL();
        Generate();
    }
    #endregion

    [System.Serializable]
    public struct Floors
    {
        private List<Room> rooms;
        private int _actualRoom;
        public List<Room> Rooms { get => rooms; set => rooms = value; }

    }


}