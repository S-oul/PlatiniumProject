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
        print(collision.name);

        Task task = collision.transform.GetComponent<Task>();
        if(task != null && _controller._isInteracting)
        {
            task._player = gameObject;
            task.Init();
            return;
        }


        Lift lift = collision.transform.parent.GetComponent<Lift>();
        if(lift != null && _controller._isInteracting)
        {
            lift.InteractLift(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Room room = collision.transform.GetComponent<Room>();
        if (room != null && collision.gameObject.layer == LayerMask.NameToLayer("Room"))
        {
            room.ListPlayer.Add(gameObject);
            //room.ListPlayer.RemoveAt(room.ListPlayer.Count - 1);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Room room = collision.transform.GetComponent<Room>();
        if (room != null && collision.gameObject.layer == LayerMask.NameToLayer("Room"))
        {
            room.ListPlayer.Remove(gameObject);
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
