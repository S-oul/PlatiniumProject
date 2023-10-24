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
    [NonSerialized] public GameObject _player;
    Room _room;
    bool _isDone = false;
    private bool _isStarted = false;

    public GameObject PlayerGameObject { get => _player; }
    public bool IsDone { get => _isDone; set => _isDone = value; }
    public int NumberOfPlayers { get => _numberOfPlayers; set => _numberOfPlayers = value; }

    public bool IsStarted { get => _isStarted; set => _isStarted = value; }

    public Room RoomTask { get => _room; set => _room = value; }
    public Room ThisRoom { get => _room; set => _room = value; }

    private void Start()
    {
        _room = transform.parent.parent.GetComponent<Room>();
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




