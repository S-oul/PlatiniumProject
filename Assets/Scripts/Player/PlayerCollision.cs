using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    PlayerController _controller;
    private void Awake()
    {
        _controller = gameObject.GetComponent<PlayerController>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Task>() != null)
        {
            if(_controller._isInteracting)
            {
                collision.gameObject.GetComponent<Task>()._player = gameObject;
                collision.gameObject.GetComponent<Task>().Init();
            }
            
        }
    }
}
