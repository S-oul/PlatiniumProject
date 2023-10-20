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

        NPC npc = collision.transform.GetComponent<NPC>();
        if (npc != null && _controller.IsInteracting)
        {
            npc.Interact();
            _controller.IsInteracting = false;
        }

        Lift lift = collision.transform.parent.GetComponent<Lift>();
        if (lift != null && _controller.IsInteracting)
        {
            lift.InteractLift(this.gameObject);
            _controller.IsInteracting = false;
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Enter :" + collision.gameObject.name);

        Room room = collision.transform.GetComponent<Room>();
        if (room != null && collision.gameObject.layer == LayerMask.NameToLayer("Room"))
        {
            room.ListPlayer.Add(gameObject);
            room.OnRoomEnter();
            //room.ListPlayer.RemoveAt(room.ListPlayer.Count - 1);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Room room = collision.transform.GetComponent<Room>();
        if (room != null && collision.gameObject.layer == LayerMask.NameToLayer("Room"))
        {
            room.ListPlayer.Remove(gameObject);
            room.OnRoomExit();
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
