using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    public Action<Task> OnTaskCompleted { get; set; }
    private List<GameObject> _playersDoingTask = new List<GameObject>();
    [Header("Task variables")]
    [Range(1, 4)][SerializeField] int _numberOfPlayers = 1;
    [Range(1, 5)] public int _difficulty = 1;
    GameObject _player;
    Room _room;
    bool _isDone = false;
    private bool _isStarted = false;


    public List<GameObject> PlayersDoingTask { get => _playersDoingTask; set => _playersDoingTask = value; }
    public GameObject PlayerGameObject { get => _player; set => _player = value; }
    public bool IsDone { get => _isDone; set => _isDone = value; }
    public int NumberOfPlayers { get => _numberOfPlayers; set => _numberOfPlayers = value; }

    public bool IsStarted { get => _isStarted; set => _isStarted = value; }

    public Room RoomTask { get => _room; set => _room = value; }
    public Room ThisRoom { get => _room; set => _room = value; }

    private void Start()
    {
        _room = transform.parent.parent.GetComponent<Room>();
        if(_room == null) { _room = transform.parent.GetComponent<Room>();}
        if (_room == null) { _room = transform.GetComponent<Room>(); }

        _room.TaskRoom = this;
    }
    public abstract void Init();

    public abstract void End(bool isSuccessful);
    public void OnPlayerJoinedTask(GameObject player)
    {
        _playersDoingTask.Add(player);
        _player = player;
        if(_playersDoingTask.Count == _numberOfPlayers)
        {
            Init();
            
        }
        
    }

    public void OnRoomSuccess()
    {

    }
    public void OnRoomFail()
    {

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




