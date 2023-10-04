using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Range(1,3)]
    [SerializeField] int _roomSize = 0;
    public int RoomSize { get => _roomSize; }

}
