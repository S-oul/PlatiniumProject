using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    public Action<Task> OnTaskCompleted { get; set; }
    private List<GameObject> _playersDoingTask = new List<GameObject>();
    [Header("Task variables")]
    [Range(1, 4)][SerializeField] int _numberOfPlayers = 1;
    [Range(1, 5)][SerializeField] int _difficulty = 1;
    [SerializeField] bool _addPlayerAtRunTime = false;
    GameObject _player;
    Room _room;
    bool _isDone = false;
    private bool _isStarted = false;

    protected GameManager _gameManager;

    public List<GameObject> PlayersDoingTask { get => _playersDoingTask; set => _playersDoingTask = value; }
    public GameObject PlayerGameObject { get => _player; set => _player = value; }
    public bool IsDone { get => _isDone; set => _isDone = value; }
    public int NumberOfPlayers { get => _numberOfPlayers; set => _numberOfPlayers = value; }

    public bool IsStarted { get => _isStarted; set => _isStarted = value; }

    public Room RoomTask { get => _room; set => _room = value; }
    public Room ThisRoom { get => _room; set => _room = value; }
    public int Difficulty { get => _difficulty; set => _difficulty = value; }
    public bool AddPlayerAtRunTime { get => _addPlayerAtRunTime; set => _addPlayerAtRunTime = value; }

    private void Start()
    {

        

    }

    private void Awake()
    {
/*        _room = transform.parent.parent.GetComponent<Room>();
        if (_room == null) { _room = transform.parent.GetComponent<Room>(); }
        if (_room == null) { _room = transform.GetComponent<Room>(); }
        _gameManager = GameManager.Instance;*/
        //print(_gameManager);
    }

    public virtual void Init()
    {

        _room = transform.parent.parent.GetComponent<Room>();
        if (_room == null) { _room = transform.parent.GetComponent<Room>(); }
        if (_room == null) { _room = transform.GetComponent<Room>(); }
        _gameManager = GameManager.Instance;
        _room.TaskRoom = this;
        
    }

    public virtual void End(bool isSuccessful)
    {
        PlayersDoingTask.Clear();
        IsStarted = false;
        IsDone = isSuccessful;
        if (isSuccessful)
        {
            OnRoomSuccess();
        }
        else
        {
            OnRoomFail();
        }
    }
    public void OnPlayerJoinedTask(GameObject player)
    {
        
        if (!IsDone)
        {
            if (AddPlayerAtRunTime)
            {
                if (_isStarted)
                {
                    if (_playersDoingTask.Count < 4)
                    {
                        _playersDoingTask.Add(player);
                        return;
                    }
                }
                _playersDoingTask.Add(player);
                _isStarted = true;
                Init();

            }
            if (_numberOfPlayers == 1)
            {
                if (!_isStarted)
                {
                    _player = player;
                    _playersDoingTask.Add(player);
                    _isStarted = true;
                    Init();
                }

            }
            else
            {
                _playersDoingTask.Add(player);
                if (_playersDoingTask.Count == _numberOfPlayers)
                {
                    if (!_isStarted)
                    {
                        _isStarted = true;
                        Init();
                    }
                }
            }
        }
        
    }

    public void OnRoomSuccess()
    {
        Debug.Log(gameObject.name + " = Success");
        GameManager.Instance.NumberOfTasksMade++;
        
        GameManager.Instance.RoomWin();
        GameManager.Instance.CheckIfDayFinished();
        _room.WinStateScreen.ChangeColor(Color.green);
    }
    public void OnRoomFail()
    {
        Debug.Log(gameObject.name);
        GameManager.Instance.NumberOfTasksMade++;
        GameManager.Instance.RoomLose();
        GameManager.Instance.CheckIfDayFinished();
        _room.WinStateScreen.ChangeColor(Color.red);
    }
    public void OnPlayerExitTaskRoom(GameObject player)
    {

        if (_playersDoingTask.Contains(player) && !IsStarted)
        {
            _playersDoingTask.Remove(player);
        }
        
    }
}

public interface ITimedTask
{
    float _givenTime { get; }

}




