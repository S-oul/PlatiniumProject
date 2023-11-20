using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    List<Room> _roomList = new List<Room>();
    [SerializeField] List<Room> _roomTaskList = new List<Room>();

    [SerializeField] float _timeForTheDay;
    [SerializeField] DaySlider _daySlider;
    [SerializeField] DayTimer _dayTimer;
    [SerializeField] Animator _animator;

    private int _roomLose = 0;
    private int _roomWin = 0;
    [SerializeField] int _maxRoomFail = 3;

    [SerializeField] int _dayIndex;

    private List<Lift> _liftList = new List<Lift>();
    int _playerCount;

    GameObject _finalRoom;

    GameObject _finalDoor;

    [SerializeField] GameObject[] _players = new GameObject[4];

    [SerializeField] int _numberOfTasksMade;

    public int PlayerCount { get => _playerCount; set => _playerCount = value; }

    public GameObject[] Players { get => _players; }
    public List<Room> RoomList { get => _roomList; set => _roomList = value; }
    public List<Lift> LiftList { get => _liftList; set => _liftList = value; }
    public List<Room> RoomTaskList { get => _roomTaskList; set => _roomTaskList = value; }
    public int NumberOfTasksMade { get => _numberOfTasksMade; set => _numberOfTasksMade = value; }
    public GameObject FinalDoor { get => _finalDoor; set => _finalDoor = value; }
    public int DayIndex { get => _dayIndex; set => _dayIndex = value; }
    public GameObject FinalRoom { get => _finalRoom; set => _finalRoom = value; }
    public Animator Animator { get => _animator; set => _animator = value; }
    public float TimeForTheDay { get => _timeForTheDay; set => _timeForTheDay = value; }

    private void Start()
    {
        StartDay();
    }

    void StartDay()
    {
        _daySlider.SetValue(1);
    }

    public int RoomWin()
    {
        _roomWin++;
        _daySlider.AddValue(_daySlider.OnRoomWin);
        return _roomWin;
    }
    public int RoomLose()
    {
        _roomLose++;
        _daySlider.RemoveValue(_daySlider.OnRoomLoose);
        if (_roomLose > _maxRoomFail)
        {
            print("T'AS PERDU TROUDUCUL");
        }
        return _roomLose;
    }

    public void ResetAllList()
    {
        _liftList.Clear();

    }

    public void LinkLifts()
    {
        Shuffle(_liftList);
        for(int i = 0; i < _liftList.Count; i++)
        {
            if (i + 1 >= _liftList.Count)
            {
                _liftList[i].TeleportPos = _liftList[0].MyPos;
            }
            else
            {
                _liftList[i].TeleportPos = _liftList[i+1].MyPos;
            }
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }

    private List<Lift> Shuffle(List<Lift> list)
    {
        int r = Random.Range(0, list.Count - 2);
        int n = list.Count;
        while (n> 1)
        {
            Lift l = list[r];
            list.RemoveAt(r);
            list.Add(l);
            n--;
        }
        return list;
    }

    public void CheckIfDayFinished()
    {
        if(_numberOfTasksMade == RoomTaskList.Count)
        {
            OpenTheFinalDoor();
        }
    }

    public void OpenTheFinalDoor()
    {
        
        StartCoroutine(_finalDoor.GetComponent<FinalDoor>().OpenDoor());
    }

    public void GoToFinalRoom()
    {
        foreach(GameObject player in _players)
        {
            if (player != null)
            {
                player.transform.position = _finalRoom.transform.position;
            }
        }
        
    }
}
