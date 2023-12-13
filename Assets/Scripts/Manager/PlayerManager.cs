using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager Instance;
    PlayerInputManager _inputManager;
    int _sortingOrderTracker = 0;

    public PlayerInputManager InputManager { get => _inputManager; set => _inputManager = value; }
    public enum ControllerType
    {
        None,
        Xbox,
        Playstation,
        Switch
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        InputManager = gameObject.GetComponent<PlayerInputManager>();
        DontDestroyOnLoad(gameObject);

    }
    void OnPlayerJoined(PlayerInput newPlayer)
    {
        if (newPlayer.devices[0].name == "Keyboard")
        {
            Destroy(newPlayer.gameObject);
        }
        else
        {
            GameManager.Instance.Players[newPlayer.playerIndex] = newPlayer.gameObject;
            GameManager.Instance.PlayerCount++;
            GetControllerType(newPlayer);
            
            //PickRandomAnimation(GameManager.Instance.Players[newPlayer.playerIndex]);                         // Randome animatior chosen here
            AssignAnimationToPlayer(newPlayer);
            GetColor(newPlayer.gameObject);
            GameManager.Instance.Players[newPlayer.playerIndex].transform.position = transform.position;      // Player spawns at the location of the PlayerManager Object
            AssignSortingOrder(GameManager.Instance.Players[newPlayer.playerIndex]);
            newPlayer.actions["InputTask"].Disable();                                                         // is "InputTask" the 'task' of adding a player?
            Camera.main.gameObject.GetComponent<Cam>().Targets.Add(newPlayer.gameObject);
            if(AudioManager.Instance != null)
            {
                AudioManager.Instance.AllSoundSource.Add(newPlayer.gameObject.transform.Find("AudioSource").GetComponent<AudioSource>());
            }
            
            
            if (GameManager.Instance.PlayerCount == 4)
            {
                InputManager.DisableJoining();
            }
        }
        
    }

    public void ResetPlayers()
    {
        GameManager.Instance.PlayerCount = 0;
    }

    void GetControllerType(PlayerInput player)
    {
        InputDevice gamepad = player.devices[0];
        if (gamepad is UnityEngine.InputSystem.DualShock.DualShockGamepad)
        {
            player.gameObject.GetComponent<PlayerController>().Type = ControllerType.Playstation;
        }
        else if(gamepad is UnityEngine.InputSystem.XInput.XInputController)
        {
            player.gameObject.GetComponent<PlayerController>().Type = ControllerType.Xbox;
        }
        else if (gamepad is UnityEngine.InputSystem.Switch.SwitchProControllerHID)
        {
            foreach (var item in Gamepad.all)
            {
                if ((item is UnityEngine.InputSystem.XInput.XInputController) && (Mathf.Abs((float)(item.lastUpdateTime - gamepad.lastUpdateTime)) < 0.1f))
                {
                    //Debug.Log($"Switch Pro controller detected and a copy of XInput was active at almost the same time. Disabling XInput device. `{gamepad}`; `{item}`");
                    InputSystem.DisableDevice(item);
                }
            }
            player.gameObject.GetComponent<PlayerController>().Type = ControllerType.Switch;
        }

        else
        {
            player.gameObject.GetComponent<PlayerController>().Type = ControllerType.Playstation;
        }
    }
    /*
    void PickRandomSprite(GameObject player)                                                                         //Change to Pick randome Animation
    {
        print(player);
        Sprite temp = DataManager.Instance.AnimationPlayers[Random.Range(0, DataManager.Instance.AnimationPlayers.Count)]; // find sprite in list   - change to list of animations
        DataManager.Instance.AnimationPlayers.Remove(temp);                                                             // remove sprite->Animation from list (to avoid identical players)   
        player.GetComponent<SpriteRenderer>().sprite = temp;                                                         // assign sprite->Animation to player's SpriteRenderer-> Animator. 
    }
    */

    public void GetColor(GameObject player)
    {
        switch (player.gameObject.GetComponentInChildren<Animator>().runtimeAnimatorController.name)
        {
            case "Blue_Animation":
                player.GetComponent<PlayerController>().ColorPlayer = Color.blue;
                player.GetComponent<PlayerController>().Name = "Frederic";
                break;
            case "Red_Animation":
                player.GetComponent<PlayerController>().ColorPlayer = Color.red;
                player.GetComponent<PlayerController>().Name = "Tom";
                break;
            case "Green_Animation":
                player.GetComponent<PlayerController>().ColorPlayer = Color.green;
                player.GetComponent<PlayerController>().Name = "Franck";
                break;
            case "Yellow_Animation":
                player.GetComponent<PlayerController>().ColorPlayer = Color.yellow;
                player.GetComponent<PlayerController>().Name = "Celine";
                break;

        }
    }
    void PickRandomAnimation(GameObject player) //Pick randome Animation
    {
        print(player);
        RuntimeAnimatorController temp = DataManager.Instance.AnimationPlayers[Random.Range(0, DataManager.Instance.AnimationPlayers.Count)]; 
        DataManager.Instance.AnimationPlayers.Remove(temp);                                                                
        player.GetComponentInChildren<Animator>().runtimeAnimatorController = temp;                                                         
    }

    void AssignAnimationToPlayer(PlayerInput player) //Pick randome Animation
    {
        player.gameObject.GetComponentInChildren<Animator>().runtimeAnimatorController = DataManager.Instance.DicSpritePlayer[player.devices[0].description];
    }

    void AssignSortingOrder(GameObject player)
    {
        player.GetComponentInChildren<SpriteRenderer>().sortingOrder = _sortingOrderTracker;
        _sortingOrderTracker++;
    }

    public GameObject FindPlayerFromColor(Color color)
    {
        GameObject tempPlayer = null;
        foreach(GameObject player in GameManager.Instance.Players)
        {
            if(player != null && player.GetComponent<PlayerController>().ColorPlayer == color)
            {
                tempPlayer = player;
            }
        }
        return tempPlayer;
    }
}
