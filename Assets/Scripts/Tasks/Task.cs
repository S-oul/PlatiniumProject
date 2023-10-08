using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tasks : MonoBehaviour
{
    int _numberOfPlayers;
    [Range(1, 5)] int _difficulty;
    
}

public interface ITimedTask
{
    float _givenTime { get; }

}
