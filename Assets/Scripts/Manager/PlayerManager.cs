using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager Instance;
    PlayerInputManager _inputManager;
    int _sortingOrderTracker = 0;

    public PlayerInputManager InputManager { get => _inputManager; set => _inputManager = value; }

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
        GameManager.Instance.Players[newPlayer.playerIndex] = newPlayer.gameObject;
        GameManager.Instance.PlayerCount++;
        print(newPlayer);
        //PickRandomAnimation(GameManager.Instance.Players[newPlayer.playerIndex]);                         // Randome animatior chosen here
        AssignAnimationToPlayer(newPlayer);
        GameManager.Instance.Players[newPlayer.playerIndex].transform.position = transform.position;      // Player spawns at the location of the PlayerManager Object
        AssignSortingOrder(GameManager.Instance.Players[newPlayer.playerIndex]);
        newPlayer.actions["InputTask"].Disable();                                                         // is "InputTask" the 'task' of adding a player?
        Camera.main.gameObject.GetComponent<Cam>().Targets.Add(newPlayer.gameObject);
        AudioManager.instance.AllSoundSource.Add(newPlayer.gameObject.transform.Find("AudioSource").GetComponent<AudioSource>());
        Debug.Log(newPlayer.devices[0]);
        if (GameManager.Instance.PlayerCount == 4)
        {
            InputManager.DisableJoining();
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
