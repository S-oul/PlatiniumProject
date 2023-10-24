using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;

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
        if (collidertype == null) { return; }
        if (_controller.IsInteracting)
        {
            switch (collidertype)
            {
                case InteractableNPC:
                    ((InteractableNPC)collidertype).Interact(gameObject);
                    _controller.IsInteracting = false;
                    break;

                case Lift:
                    ((Lift)collidertype).InteractLift(gameObject);
                    _controller.IsInteracting = false;
                    break;

                case Object:
                    ((Object)collidertype).Interact(gameObject);
                    _controller.IsInteracting = false;
                    break;

                case DecryptageTask:
                    ((DecryptageTask)collidertype).PlyrInput = GetComponent<PlayerInput>();
                    ((DecryptageTask)collidertype).Init();
                    GetComponent<PlayerController>().DisableMovement();
                    GetComponent<PlayerInput>().actions["Decryptage"].Enable();
                    _controller.IsInteracting = false;
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
            case "ZoneEvent":
                collidertype = collision.transform.GetComponent<ZoneEvent>();
                ((ZoneEvent)collidertype).PlayerEnter(gameObject);
                break;
            case "Object":
                collidertype = collision.transform.GetComponent<Object>();
                break;
            case "Laser":
                print("haaaaaaaaaaaaaaaaaaaaaa");
                GetComponent<PlayerController>().DownPlayer();
                break;
            case "DecryptInteract":
                collidertype = collision.transform.parent.GetComponent<DecryptageTask>();
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
   
}
