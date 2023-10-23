using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    public Action<Task> OnTaskCompleted { get; set; }
    private List<GameObject> playersDoingTask = new List<GameObject>();
    [Header("Task variables")]
    [Range(1, 4)][SerializeField] int _numberOfPlayers = 1;
    [Range(1, 5)] public int _difficulty = 1;
    [NonSerialized] public GameObject _player;
    bool isAlreadyUsed = false;

    bool _isDone = false;
    public GameObject PlayerGameObject { get => _player; }
    public bool IsDone { get => _isDone; set => _isDone = value; }
    public int NumberOfPlayers { get => _numberOfPlayers; set => _numberOfPlayers = value; }

    public abstract void Init();

    public abstract void End(bool isSuccessful);
    public void OnPlayerJoinedTask(GameObject player)
    {
        _player = player;
        Init();
    }

    

}

public interface ITimedTask
{
    float _givenTime { get; }

}




