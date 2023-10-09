using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tasks : MonoBehaviour
{
    [Header("Task variables")]
    [Range(1, 4)][SerializeField] int _numberOfPlayers = 1;
    [Range(1, 5)][SerializeField] int _difficulty = 1;
    bool isDone;
    [HideInInspector] Player _player;
}

public interface ITimedTask
{
    float _givenTime { get; }

}
