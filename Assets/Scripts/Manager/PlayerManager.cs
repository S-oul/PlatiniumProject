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
        Playstation
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
            GameManager.Instance.Players[newPlayer.playerIndex].transform.position = transform.position;      // Player spawns at the location of the PlayerManager Object
            AssignSortingOrder(GameManager.Instance.Players[newPlayer.playerIndex]);
            newPlayer.actions["InputTask"].Disable();                                                         // is "InputTask" the 'task' of adding a player?
            Camera.main.gameObject.GetComponent<Cam>().Targets.Add(newPlayer.gameObject);
            if(AudioManager.instance != null)
            {
                AudioManager.instance.AllSoundSource.Add(newPlayer.gameObject.transform.Find("AudioSource").GetComponent<AudioSource>());
            }
            
            
            if (GameManager.Instance.PlayerCount == 4)
            {
                InputManager.DisableJoining();
            }
        }
        
    }

    void GetControllerType(PlayerInput player)
    {
        if (player.devices[0].displayName.Contains("Xbox"))
        {
            player.gameObject.GetComponent<PlayerController>().Type = ControllerType.Xbox;
        }
        else if (player.devices[0].displayName.Contains("PLAYSTATION"))
        {
            player.gameObject.GetComponent<PlayerController>().Type = ControllerType.Playstation;
        }
        else
        {
            player.gameObject.GetComponent<PlayerController>().Type = ControllerType.None;
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

    
}
