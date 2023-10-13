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


        Task task = collision.gameObject.GetComponent<Task>();
        if(task != null && _controller._isInteracting)
        {
            task._player = gameObject;
            task.StartTask();
            return;
        }

        Lift lift = collision.gameObject.GetComponent<Lift>();
        if(lift != null && _controller._isInteracting)
        {
            lift.InteractLift(this.gameObject);
        }
    }





/*
    public T[] GetComponentsInDirectChildren<T>() where T : Component
    {
        List<T> children = new List<T>();
        foreach(Transform childTransform in transform)
        {
            T childComponent = childTransform.GetComponent<T>();
            if(childComponent != null)
            {
                children.Add(childComponent);
            }
        }

        return children.ToArray();
    }*/
}
