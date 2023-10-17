using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Lift : Room
{
    private Transform _myPos;
    private Transform _teleportPos;

    public Transform MyPos { get => _myPos; }
    public Transform TeleportPos { get => _teleportPos; set => _teleportPos = value; }
    private void Awake()
    {
        _myPos = transform;
    }

    public void InteractLift(GameObject player)
    {
        player.transform.position = TeleportPos.position;  
    }

    //Transform TeleportPlayer()
}
