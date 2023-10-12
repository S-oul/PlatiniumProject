using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    [Header("Task variables")]
    [Range(1, 4)][SerializeField] int _numberOfPlayers = 1;
    [Range(1, 5)]public  int _difficulty = 1;
    bool isDone;
    [HideInInspector]public GameObject _player;
    public void Init()
    {
        StartTask();
    }
    public abstract void StartTask();
}

public interface ITimedTask
{
    float _givenTime { get; }
            
}
