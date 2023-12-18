using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Tilemaps.Tile;

public class PlayerCollision : MonoBehaviour
{

    PlayerController _controller;
    PlayerInput _inputs;
    PlayerUI _playerUI;
    AudioSource _audioSource;
    MonoBehaviour collidertype;


    #region In Game Var
    bool _IsInLift = false;
    bool _isInNPC = false;
    bool _canAutoLift = true;

    #endregion

    #region Accesseur
    public bool IsInLift { get => _IsInLift; set => _IsInLift = value; }
    public bool IsInNPC { get => _isInNPC; set => _isInNPC = value; }
    public MonoBehaviour Collidertype { get => collidertype; set => collidertype = value; }

    #endregion

    private void Awake()
    {
        _inputs = GetComponent<PlayerInput>();
        _controller = gameObject.GetComponent<PlayerController>();
        _playerUI = gameObject.GetComponent<PlayerUI>();
        _audioSource = gameObject.transform.Find("AudioSource").GetComponent<AudioSource>();
    }
    private void Update()
    {
        MakeInteraction();
    }
    void MakeInteraction()
    {
       
        
        if (Collidertype == null) {  return; }
        if (_controller.IsInteracting)
        {
            
            switch (Collidertype)
            {
                case InteractableNPC:
                    ((InteractableNPC)Collidertype).Interact(gameObject);
                    break;
                case Lift:
                    ((Lift)Collidertype).InteractLift(gameObject);
                    break;
                case LeCode:
                    ((LeCode)Collidertype).Controller = _controller;
                    ((LeCode)Collidertype).OnPlayerJoinedTask(gameObject);
                    break;
                case Object:
                    ((Object)Collidertype).Interact(gameObject);
                    break;
                case DecryptageTask:
                    ((DecryptageTask)Collidertype).OnPlayerJoinedTask(gameObject);
                    break;
                case GoatTask:
                    ((GoatTask)Collidertype).OnPlayerJoinedTask(this.gameObject);
                    _playerUI.DisplayInputToPress(false, "");
                    break;
                case StoreTask:

                    _controller.EnableDecryptageDisableMovements();
                    ((StoreTask)Collidertype).OnPlayerJoinedTask(this.gameObject);
                    break;
                case FinalDoor:
                    ((FinalDoor)Collidertype).EnterInTheDoor();
                    break;
                case GraffitiGameManager:
                    ((GraffitiGameManager)Collidertype).OnPlayerJoinedTask(this.gameObject);
                    _playerUI.DisplayInputToPress(false, "");
                    break;
            }
            
            _controller.IsInteracting = false;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Room room = collision.transform.GetComponent<Room>();
        if (room != null && (collision.gameObject.layer == LayerMask.NameToLayer("Room") || collision.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast")))
        {
            room.ListPlayer.Add(gameObject);
            room.OnRoomEnter();
            return;
        }
        

        switch (collision.tag)
        {
            case "NPC":
                //_isInNPC = true;
                Collidertype = collision.transform.GetComponent<NPC>();
                break;
            case "ChattyNPC":
                Collidertype = collision.transform.GetComponent<NPC>();
                ((InteractableNPC)Collidertype).VocalTalk();
                break;
            case "ZoneEvent":
                Collidertype = collision.transform.GetComponent<ZoneEvent>();
                ((ZoneEvent)Collidertype).PlayerEnter(gameObject);
                break;
            case "Object":
                Collidertype = collision.transform.GetComponent<Object>();
                
                if (!collision.transform.GetComponent<Object>().IsUsed)
                {
                    
                    _playerUI.DisplayInputToPress(true, "Y");
                }
                
                
                break;
/*            case "Laser":
                AudioManager.instance.PlaySFXOS("LaserImpact", _audioSource);
                StartCoroutine(_controller.PlayerDown(collision.GetComponent<Laser>().TimePlayerIsDown));
                break;*/
            case "DecryptInteract":
                Collidertype = collision.transform.parent.GetComponent<DecryptageTask>();
                _playerUI.DisplayInputToPress(true, "Y");
                
                break;
            case "CodeZone":
                Collidertype = collision.transform.parent.GetComponent<LeCode>();
                _playerUI.DisplayInputToPress(true, "Y");

                /*                if (!lecode.HaveOnePlayer())
                                {
                                    lecode.Player = gameObject;
                                    lecode.Init();
                                    _playerUI.DisplayLeCodeUI(true);
                                    _inputs.actions["Interact"].Disable();
                                    _inputs.actions["Jump"].Disable();
                                    _inputs.actions["Code"].Enable();
                                    lecode.Controller = _controller;
                                }*/
                break;
            case "Goat":
                Collidertype = collision.transform.GetComponent<GoatTask>();
                _playerUI.DisplayInputToPress(true, "Y");

                break;
            case "StoreZone":
                Collidertype = collision.transform.parent.GetComponent<StoreTask>();
                _playerUI.DisplayInputToPress(true, "Y");
                break;
            case "FinalDoor":
                Collidertype = collision.transform.GetComponent<FinalDoor>();
                if (collision.transform.gameObject.GetComponent<FinalDoor>().IsOpened)
                {
                    _playerUI.DisplayInputToPress(true, "Y");
                }
                break;
            case "AutoLift":
                if (_canAutoLift)
                {
                    StartCoroutine(AutoLiftWait());
                    Collidertype = collision.transform.GetComponent<Lift>();
                    ((Lift)Collidertype).AutoLiftInteract(gameObject);
                }
                break;
            case "GraffitiTask":
                Collidertype = collision.transform.parent.GetComponent<GraffitiGameManager>();
                if (!collision.transform.parent.GetComponent<GraffitiGameManager>().IsDone)
                {
                    _playerUI.DisplayInputToPress(true, "Y");
                }
                
                break;
            
                
        }
        
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Lift":
                //_IsInLift = true;
                _playerUI.DisplayInputToPress(true, "Y");
                Collidertype = collision.transform.parent.GetComponent<Lift>();
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Collidertype = null;
        
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
                _playerUI.DisplayInputToPress(false, "");

            /*                LeCode lecode = collision.transform.parent.GetComponent<LeCode>();
                            if (lecode.Player == gameObject)
                            {
                                lecode.Player = null;
                                _playerUI.DisplayLeCodeUI(false);
                                _inputs.actions["Interact"].Enable();
                                _inputs.actions["Jump"].Enable();
                                _inputs.actions["Code"].Disable();
                            }*/
                break;
            case "NPC":
                //_isInNPC = true;
                Collidertype = collision.transform.GetComponent<NPC>();
                break;
            case "Object":
                _playerUI.DisplayInputToPress(false, "");
                
                //Collidertype = collision.transform.GetComponent<Object>();
                break;
            case "DecryptInteract":
                Collidertype = collision.transform.parent.GetComponent<DecryptageTask>();
                _playerUI.DisplayInputToPress(false, "");
                break;
            case "Goat":
                Collidertype = collision.transform.GetComponent<GoatTask>();
                _playerUI.DisplayInputToPress(false, "");
                break;
            case "StoreZone":
                Collidertype = collision.transform.parent.GetComponent<StoreTask>();
                _playerUI.DisplayInputToPress(false, "");
                break;
            case "FinalDoor":
                Collidertype = collision.transform.GetComponent<FinalDoor>();
                if (collision.transform.gameObject.GetComponent<FinalDoor>().IsOpened)
                {
                    _playerUI.DisplayInputToPress(false, "");
                }
                break;
            case "Lift":
                _playerUI.DisplayInputToPress(false, "");
                break;
            case "GraffitiTask":
                
                _playerUI.DisplayInputToPress(false, "");
                break;
        }
        
    }



    IEnumerator AutoLiftWait()
    {
        _canAutoLift = false;
        yield return new WaitForSeconds(1.5f);
        _canAutoLift = true;
    }
}
