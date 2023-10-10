using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;

public class Building : MonoBehaviour
{
    #region Visible Variable 
    [SerializeField] private int _maxRooms = 3;
    [SerializeField] private int _maxFloors = 5;

    [Tooltip("The Space Between each floor")]
    [SerializeField] private float _heightBetweenFloor = 2f;

    [SerializeField] private GameObject _spawnRoom; 

    [SerializeField] private List<GameObject> _poolType1 = new List<GameObject>();
    [SerializeField] private List<GameObject> _poolType2 = new List<GameObject>();
    [SerializeField] private List<GameObject> _poolType3 = new List<GameObject>();
    [SerializeField] private List<GameObject> _poolType4 = new List<GameObject>();

    [SerializeField] private List<List<GameObject>> _allPool = new List<List<GameObject>>();

    //[MenuItem("Assets/Create Room")]



    [SerializeField] private List<Floors> _floors = new List<Floors>();

    #endregion

    private bool _hasBigRoom = false;


    private void OnValidate()
    {
        _allPool.Clear();
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
        string floorID = GenerateFloorID();
        List<GameObject> rooms = new List<GameObject>();
    }
    void GenerateFloor(float height, GameObject firstRoom)
    {
        string floorID = GenerateFloorID();
        List<GameObject> rooms = new List<GameObject>();
    }

    string GenerateFloorID()
    {
        string floorsID = "";
        for (int i = 0; i < _maxRooms; i++)
        {
            floorsID += 'A';
        }
        return floorsID;
    }
    bool CheckAddID(string fID, string AddID)
    {
        int i = AddID.Count();
        string emp = "";
        while(i != 0)
        {
            emp += 'A';
            i--;
        }
        print(emp + " // "+fID);
        if (fID.Contains(emp)) return true;
        else return false;
    }
    


    #region Generate & Destroy
    [Button("Generate Building")]
    private void Generate()
    {
        _hasBigRoom = false;
        for (float i = 0; i < _maxFloors; i++)
        {
            GenerateFloor(i * 5f + i * _heightBetweenFloor, _spawnRoom);

        }
    }

    #region Instatiate Room OLD
    /*
    
        bool hasLift = false;

        int oldR = -999;

        while (currentRoom < _maxRooms)
        {
            int r;
            if (!_hasBigRoom) r = Random.Range(1, 5);
            else r = Random.Range(1, 4);

            if (hasLift) { r = Random.Range(2, 4); }
            if (currentRoom + r == _maxRooms && hasLift == false ) { r = 1; }

            while (currentRoom + r > _maxRooms) { r -= 1; }

            if (r == 1) hasLift = true;
            if (r == 4) _hasBigRoom = true;



            oldR = r;
        }
 
 
 GameObject go = Instantiate(_allPool[r-1][Random.Range(0, _allPool[r-1].Count)], transform);
    Room room = go.GetComponent<Room>();
    room.InitRoom();
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
    
     
     
     
     
     
     */
    #endregion



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