using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    PlayerController _controller;

    MonoBehaviour collidertype;

    #region In Game Var
    bool _IsInLift = false;
    bool _isInNPC = false;
    bool _ = false;
    #endregion

    #region Accesseur
    public bool IsInLift { get => _IsInLift; set => _IsInLift = value; }
    public bool IsInNPC { get => _isInNPC; set => _isInNPC = value; }

    #endregion

    private void Awake()
    {
        _controller = gameObject.GetComponent<PlayerController>();
    }
    private void Update()
    {

        //Debug.Log("AAAAAAAAAAAAAAAAAA" + collision.name);
        if (_controller.IsInteracting)
        {
            if (collidertype == null) { return; }
            switch (collidertype)
            {
                case NPC:
                    ((NPC)collidertype).Interact(gameObject);
                    _controller.IsInteracting = false;
                break;
                case Lift:
                    ((Lift)collidertype).InteractLift(gameObject);
                    Debug.Log("Lift" + collidertype.name);

                    break;
            }
        }
/*
        Lift lift = collision.transform.parent.GetComponent<Lift>();
        if (lift != null && _controller.IsInteracting)
        {
            lift.InteractLift(this.gameObject);
            _controller.IsInteracting = false;
        }*/       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //print("Enter :" + collision.gameObject.name);

        Room room = collision.transform.GetComponent<Room>();
        if (room != null && collision.gameObject.layer == LayerMask.NameToLayer("Room"))
        {
            room.ListPlayer.Add(gameObject);
            room.OnRoomEnter();
            return;
        }

        switch (collision.tag)
        {
            case "NPC":
                //_isInNPC = true;
                collidertype = collision.transform.GetComponent<NPC>();
                break;
            case "Lift":
                //_IsInLift = true;
                collidertype = collision.transform.parent.GetComponent<Lift>();
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collidertype = null;
        Room room = collision.transform.GetComponent<Room>();
        if (room != null && collision.gameObject.layer == LayerMask.NameToLayer("Room"))
        {
            room.ListPlayer.Remove(gameObject);
            room.OnRoomExit();
            return;
        }
/*
        switch (collision.tag)
        {
            case "NPC":
                //_isInNPC = true;
                collidertype = null;
                break;
            case "Lift":
                _IsInLift = true;
                break;
        }*/
    }

    public class ColliderData<T> where T : MonoBehaviour
    {
        private T _colidertype;
        public T Colidertype { get => _colidertype; set => _colidertype = value; }

        public ColliderData(T type)
        {
            Colidertype = type;
        }
    }    
}
