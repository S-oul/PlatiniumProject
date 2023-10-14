using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    public Action<Task> OnTaskCompleted { get; set; }

    [Header("Task variables")]
    [Range(1, 4)][SerializeField] int _numberOfPlayers = 1;
    [Range(1, 5)] public int _difficulty = 1;
    bool isDone;
    [NonSerialized] public GameObject _player;

    public GameObject PlayerGameObject { get => _player; }

    public abstract void Init();

}

public interface ITimedTask
{
    float _givenTime { get; }

}




