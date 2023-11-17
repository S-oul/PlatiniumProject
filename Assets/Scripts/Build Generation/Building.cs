using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

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


    [SerializeField] private List<FloorData> _spawnFloors = new List<FloorData>();
    [SerializeField] private List<FloorData> _floorsW2Max = new List<FloorData>();

    [SerializeField] private Transform _finalRoomPosition;

    private List<List<GameObject>> _allPool = new List<List<GameObject>>();
    
    private GameManager _gameManager;

    #endregion

    #region Generation value;

    private bool _hasSpawnRoom = false;
    private bool _hasBigRoom = false;
    private bool _hasCodeRoom = false;


    #endregion

    private void OnValidate()
    {
        _gameManager = GameManager.Instance;
        _allPool.Clear();
        _allPool.Add(_poolType1);
        _allPool.Add(_poolType2);
        _allPool.Add(_poolType3);
        _allPool.Add(_poolType4);
    }

    #region GENERATE FLOOR 

    private void Start()
    {
        OnValidate();
        _gameManager.ResetAllList();
        DestroyALL();
        Generate();
    }
    void GenerateFloor(int floor,float height)
    {
        //Chance to spawn a Big3 Room
        //float treshold = ((float)floor +1) / (float)_maxFloors;
        //float chancetoSpawn = Random.Range(0, 101) / 100f;
        // print(treshold + " / " + chancetoSpawn + " / hasBigRoom : " + _hasBigRoom);
        
        string data;
        data = _floorsW2Max[Random.Range(0, _floorsW2Max.Count)]._roomstype;

        int randomReverse = Random.Range(0,2);
        if (randomReverse == 1)
        {
            data = Reverse(data);
        }

        int i = 0;
        foreach (char c in data)
        {
            if (c == 'S')
            {
                Room spawnRoom = instantiateRoom(_spawnRoom, height, i);
                i += spawnRoom.RoomSize;
                _gameManager.FinalDoor = spawnRoom.transform.Find("FinalDoor").gameObject;
            }
            else
            {
                int intC = CharToInt(c);
                

                int r = Random.Range(0, _allPool[intC].Count);
                if(_allPool[intC][r].GetComponent<Room>().Id == "C" && !_hasCodeRoom && floor > 1)
                {
                    print("oui)");
                    _hasCodeRoom = true;
                }else
                {
                    while (_allPool[intC][r].GetComponent<Room>().Id == "C")
                    {
                        r = Random.Range(0, _allPool[intC].Count);
                    }
                }
                i += instantiateRoom(_allPool[intC][r], height, i).RoomSize;
            }
            
        }

    }
    void GenerateFloor(FloorData f, int floor, float height)
    {
        string data = f._roomstype;
        int i = 0;
        foreach (char c in data)
        {
            if (c == 'S')
            {
                Room spawnRoom = instantiateRoom(_spawnRoom, height, i);
                i += spawnRoom.RoomSize;
                _gameManager.FinalDoor = spawnRoom.transform.Find("FinalDoor").gameObject;
            }
            else
            {
                int intC = CharToInt(c);


                int r = Random.Range(0, _allPool[intC].Count);
                if (_allPool[intC][r].GetComponent<Room>().Id == "C" && !_hasCodeRoom && floor > 1)
                {
                    _hasCodeRoom = true;
                }
                else
                {
                    while (_allPool[intC][r].GetComponent<Room>().Id == "C")
                    {
                        r = Random.Range(0, _allPool[intC].Count);
                    }
                }
                i += instantiateRoom(_allPool[intC][r], height, i).RoomSize;
            }
        }
    }

    Room instantiateRoom(GameObject room, float height, int roomStart)
    {
        GameObject go = Instantiate(room);  
        go.transform.parent = transform;
        Room ro = go.GetComponent<Room>();
        ro.InitRoom();

        go.transform.position = new Vector3(roomStart * 5 + go.transform.localScale.x/2, height, 0);
        return ro;
    }

        #region Instantiate Room V2 
        /*//Spawn room on 3 floor n Middle
            if(floor == 2 && !_hasSpawnRoom)
            {
                _hasSpawnRoom = true;
                //string SpawnRoomID = instantiateRoom(_spawnRoom, height, _maxRooms / 2 - 1);;
                /*            _roomMatrix[floor, _maxRooms / 2 - 1] = SpawnRoomID;
                            _roomMatrix[floor, _maxRooms / 2] = SpawnRoomID;
                instantiateRoom(_spawnRoom, height, _maxRooms / 2 - 1);


    }
    if (floor == 3)
    {
        int j = 0;
        foreach (int i in _floors[0]._roomstype)
        {
            instantiateRoom(_allPool[i][Random.RandomRange(0, _allPool[i].Count)], height, j);
            j += _floors[0]._roomstype[i];

        }
    }

    //BIG Room
    if (!_hasBigRoom)
    {
        int r = Random.Range(0, 101);
        int treshhold = 20 * (floor + 1);
        if (floor == _maxFloors - 1)
        {
            r = 100;
        }

        if (r < treshhold)
        {
            print(r + " " + treshhold);
            r = Random.Range(0, _maxRooms - 3);
            while (_roomMatrix[floor, r] != null || _roomMatrix[floor, r + 1] != null || _roomMatrix[floor, r + 2] != null || _roomMatrix[floor, r + 3] != null)
            {
                r = Random.Range(0, _maxRooms - 4);
            }
            _hasBigRoom = true;
            /*string SpawnRoomID = instantiateRoom(_allPool[3][0], height, r);
            _roomMatrix[floor, r] = SpawnRoomID;
            _roomMatrix[floor, r + 1] = SpawnRoomID;
            _roomMatrix[floor, r + 2] = SpawnRoomID;
            _roomMatrix[floor, r + 3] = SpawnRoomID;
        }
    }
    for (int i = 0; i < _maxRooms - 3; i++)
    {
        if (_roomMatrix[floor, i] == null)
        {

        }
    }

    for (int i = 0; i < _maxRooms; i++)
    {
        //print(floor + "/" + i + " : " + _roomMatrix[floor, i]);
        if (_roomMatrix[floor, i] == null)
        {
            instantiateRoom(_allPool[0][1], height, i);
        }
    }


    */
        #endregion

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
            }     */
        #endregion


    #endregion

    #region Generate & Destroy Button
    private void Generate()
    {
        _hasSpawnRoom = false;
        _hasBigRoom = false;
        System.Console.Clear();
        GameObject finalRoom = Instantiate(_poolType4[_gameManager.DayIndex], _finalRoomPosition.position, Quaternion.identity);
        _gameManager.FinalRoom = finalRoom;
        for (float i = 0; i < _maxFloors; i++)
        {
            if (i == 2)
            {
                GenerateFloor(_spawnFloors[Random.Range(0, _spawnFloors.Count)], (int)i, i * 5f + i * _heightBetweenFloor);
            }
            else GenerateFloor((int)i, i * 5f + i * _heightBetweenFloor);

        }
        _gameManager.LinkLifts();
    }




[Button("DestroyALL")]
    public void DestroyALL()
    {
        for (int i = transform.childCount - 1; i != -1; i--)
        {
            /*print(i);*/
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }


    [Button("Generate")]
    public void DestroyAndGenerate()
    {
        OnValidate();
        _gameManager.ResetAllList();
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

    #region Utilities Func
    string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        System.Array.Reverse(charArray);
        return new string(charArray);
    }

    int CharToInt(char c)
    {
        switch (c)
        {
            case '0': return 0;
            case '1': return 1;
            case '2': return 2;
            case '3': return 3;
            case '4': return 4;
            case '5': return 5;
            case '6': return 6;
            case '7': return 7;
            case '8': return 8;
            case '9': return 9;
            default:
                print("ERROR ERROR : " + c);
                return -1;
        }
    }

    #endregion
}
[System.Serializable]
public class FloorData
{
    
    public string _roomstype;

}