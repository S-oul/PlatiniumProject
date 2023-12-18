using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameObject _lastPlayerToFail; //for animating the camera in Game Over cutscene
    Coroutine _failerCo;
    bool _hisfaultBool = false;

    List<Room> _roomList = new List<Room>();
    [SerializeField] List<Room> _roomTaskList = new List<Room>();

    [SerializeField] float _timeForTheDay;
    [SerializeField] DaySlider _daySlider;
    [SerializeField] DayTimer _dayTimer;
    [SerializeField] DayManager _dayManager;


    private int _roomLose = 0;
    private int _roomWin = 0;
    [SerializeField] int _maxRoomFail = 3;

    [SerializeField] int _dayIndex;

    [SerializeField] private List<Lift> _liftList = new List<Lift>();
    int _playerCount;

    GameObject _finalRoom;

    GameObject _finalDoor;

    [SerializeField] List<GameObject> _players = new List<GameObject>();

    [SerializeField] int _numberOfTasksMade;

    Transform _roomRemainingText;
    Transform _roomRemainingImage;

    [SerializeField] GameObject _pauseMenu;
    [SerializeField] bool _pauseBool = false;

    public int PlayerCount { get => _playerCount; set => _playerCount = value; }
    public List<GameObject> Players { get => _players; set => _players = value; }
    public List<Room> RoomList { get => _roomList; set => _roomList = value; }
    public List<Lift> LiftList { get => _liftList; set => _liftList = value; }
    public List<Room> RoomTaskList { get => _roomTaskList; set => _roomTaskList = value; }
    public int NumberOfTasksMade { get => _numberOfTasksMade; set => _numberOfTasksMade = value; }
    public GameObject FinalDoor { get => _finalDoor; set => _finalDoor = value; }
    public int DayIndex { get => _dayIndex; set => _dayIndex = value; }
    public GameObject FinalRoom { get => _finalRoom; set => _finalRoom = value; }
    public float TimeForTheDay { get => _timeForTheDay; set => _timeForTheDay = value; }
    public DayManager DayManager { get => _dayManager; set => _dayManager = value; }
    public DaySlider DaySlider { get => _daySlider; set => _daySlider = value; }
    public Transform RoomRemainingText { get => _roomRemainingText; set => _roomRemainingText = value; }
    public Transform RoomRemainingImage { get => _roomRemainingImage; set => _roomRemainingImage = value; }
    // public TextMeshProUGUI RoomsRemainingText { get => _roomsRemainingText; set => _roomsRemainingText = value; } // Room without S 
    public bool PauseBool { get => _pauseBool; set => _pauseBool = value; }
    public GameObject PauseMenu { get => _pauseMenu; set => _pauseMenu = value; }
    public GameObject LastPlayerToFail { get => _lastPlayerToFail; set => _lastPlayerToFail = value; }
    public bool HisfaultBool { get => _hisfaultBool; set => _hisfaultBool = value; }

    public void ChangeLastPlayerToFail(GameObject g)
    {
        if(_failerCo != null) StopCoroutine(_failerCo);
        _hisfaultBool = true;
        _lastPlayerToFail = g;
        _failerCo = StartCoroutine(HisFault());
    }
    IEnumerator HisFault()
    {
        yield return new WaitForSecondsRealtime(4f);
        _hisfaultBool = false;
    }
    public float DaySliderOverDay = 0;
    private void Start()
    {
        /*StartDay();*/
    }

    public void StartDay()
    {
        _dayTimer = DayManager.DayTimer;
        _daySlider = DayManager.DaySlider;
        RoomRemainingImage.gameObject.SetActive(false);
        RoomRemainingText.gameObject.SetActive(true);
        RoomRemainingText.Find("Value").GetComponent<TextMeshProUGUI>().text = (RoomTaskList.Count - NumberOfTasksMade).ToString();
        
        

    }

    public int RoomWin()
    {
        _roomWin++;
        _daySlider.AddValue(_daySlider.OnRoomWin);
        return _roomWin;
    }
    public int RoomLose()
    {
        if (_daySlider.IsOnCrunch)
        {
            SceneManager.LoadScene(3);
        }
        _roomLose++;
        _daySlider.RemoveValue(_daySlider.OnRoomLoose);

        return _roomLose;
    }

    public void ResetAllList()
    {
        _liftList.Clear();

    }

    public void LinkLifts()
    {
        ShuffleLift(_liftList);
        for (int i = 0; i < _liftList.Count; i++)
        {
            if (i + 1 >= _liftList.Count)
            {
                _liftList[i].TeleportPos = _liftList[0].MyPos;
            }
            else
            {
                _liftList[i].TeleportPos = _liftList[i + 1].MyPos;
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }


    }

    public List<Lift> ShuffleLift(List<Lift> list)
    {
        int r = 0;
        int n = list.Count;
        while (n > 1)
        {
            r = Random.Range(0, list.Count - 2);
            Lift l = list[r];
            list.RemoveAt(r);
            list.Add(l);
            n--;
        }
        return list;
    }



    public void CheckIfDayFinished()
    {
        if (_numberOfTasksMade == RoomTaskList.Count)
        {
            /*_dayTimer.DoTimer = false; 
            _daySlider.IsOnCrunch = true;*/
            RoomRemainingImage.gameObject.SetActive(true);
            RoomRemainingText.gameObject.SetActive(false);
            OpenTheFinalDoor();
        }
    }

    public void OpenTheFinalDoor()
    {
        _dayTimer.DoTimer = false;
        _daySlider.IsOnCrunch = true;
        StartCoroutine(_finalDoor.GetComponent<FinalDoor>().OpenDoor());
    }
    public void SetPause()
    {
        print("MANGER COFFEE");
        _pauseBool = !_pauseBool;
        _pauseMenu.SetActive(_pauseBool);
        if (_pauseBool)
        {
            Time.timeScale = 0;

        }
        else
        {
            Time.timeScale = 1;
        }

    }
    public void GoToFinalRoom()
    {
        foreach (GameObject player in _players)
        {
            if (player != null)
            {
                player.transform.position = _finalRoom.transform.position + Vector3.up *4;
            }
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
        DestroyImmediate(gameObject);
        //DestroyObject(gameObject);
        //return this.gameObject;
    }
   
}
