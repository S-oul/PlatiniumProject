using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCollision : MonoBehaviour
{

    PlayerController _controller;
    PlayerInput _inputs;
    PlayerUI _playerUI;

    MonoBehaviour collidertype;


    #region In Game Var
    bool _IsInLift = false;
    bool _isInNPC = false;
    bool _canAutoLift = true;

    #endregion

    #region Accesseur
    public bool IsInLift { get => _IsInLift; set => _IsInLift = value; }
    public bool IsInNPC { get => _isInNPC; set => _isInNPC = value; }

    #endregion

    private void Awake()
    {
        _inputs = GetComponent<PlayerInput>();
        _controller = gameObject.GetComponent<PlayerController>();
        _playerUI = gameObject.GetComponent<PlayerUI>();
    }
    private void Update()
    {
        MakeInteraction();
    }
    void MakeInteraction()
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
                    ((DecryptageTask)collidertype).OnPlayerJoinedTask(gameObject);

                    _controller.IsInteracting = false;
                    break;
                case GoatTask:
                    ((GoatTask)collidertype).OnPlayerJoinedTask(this.gameObject);
                    break;
                case StoreTask:

                    _controller.EnableDecryptageDisableMovements();
                    ((StoreTask)collidertype).OnPlayerJoinedTask(this.gameObject);
                    break;
                case FinalDoor:
                    ((FinalDoor)collidertype).EnterInTheDoor();
                    break;
            }
        }
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
                //print("haaaaaaaaaaaaaaaaaaaaaa");
                StartCoroutine(_controller.PlayerDown(collision.GetComponent<Laser>().TimePlayerIsDown));
                break;
            case "DecryptInteract":
                collidertype = collision.transform.parent.GetComponent<DecryptageTask>();
                break;
            case "CodeZone":
                LeCode lecode = collision.transform.parent.GetComponent<LeCode>();
                if (!lecode.HaveOnePlayer())
                {
                    lecode.Player = gameObject;
                    lecode.Init();
                    _playerUI.DisplayLeCodeUI(true);
                    _inputs.actions["Interact"].Disable();
                    _inputs.actions["Jump"].Disable();
                    _inputs.actions["Code"].Enable();
                    lecode.Controller = _controller;
                }
                break;
            case "Goat":
                collidertype = collision.transform.GetComponent<GoatTask>();
                break;
            case "StoreZone":
                collidertype = collision.transform.parent.GetComponent<StoreTask>();
                break;
            case "FinalDoor":
                collidertype = collision.transform.GetComponent<FinalDoor>();
                break;
            case "AutoLift":
                if (_canAutoLift)
                {
                    StartCoroutine(AutoLiftWait());
                    collidertype = collision.transform.GetComponent<Lift>();
                    ((Lift)collidertype).InteractLift(gameObject);
                }
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

        switch (collision.tag)
        {
            case "CodeZone":
                LeCode lecode = collision.transform.parent.GetComponent<LeCode>();
                if (lecode.Player == gameObject)
                {
                    lecode.Player = null;
                    _playerUI.DisplayLeCodeUI(false);
                    _inputs.actions["Interact"].Enable();
                    _inputs.actions["Jump"].Enable();
                    _inputs.actions["Code"].Disable();
                }
                break;
        }
    }
   
    IEnumerator AutoLiftWait()
    {
        _canAutoLift = false;
        yield return new WaitForSeconds(3);
        _canAutoLift = true;
    }
}
