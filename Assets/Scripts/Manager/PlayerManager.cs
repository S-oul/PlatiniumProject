using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    PlayerInputManager _inputManager;
    int _sortingOrderTracker = 0;
   
    private void Awake()
    {
        _inputManager = gameObject.GetComponent<PlayerInputManager>();
        DontDestroyOnLoad(gameObject);
    }
    void OnPlayerJoined(PlayerInput newPlayer)
    {
        GameManager.Instance.Players[newPlayer.playerIndex] = newPlayer.gameObject;
        GameManager.Instance.PlayerCount++;
        PickRandomAnimation(GameManager.Instance.Players[newPlayer.playerIndex]);                         // Randome animatior chosen here
        GameManager.Instance.Players[newPlayer.playerIndex].transform.position = transform.position;      // Player spawns at the location of the PlayerManager Object
        AssignSortingOrder(GameManager.Instance.Players[newPlayer.playerIndex]);
        newPlayer.actions["InputTask"].Disable();                                                         // is "InputTask" the 'task' of adding a player?
        Camera.main.gameObject.GetComponent<Cam>().Targets.Add(newPlayer.gameObject);
        Debug.Log(newPlayer.devices[0].displayName);
        
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

    void AssignSortingOrder(GameObject player)
    {
        player.GetComponentInChildren<SpriteRenderer>().sortingOrder = _sortingOrderTracker;
        _sortingOrderTracker++;
    }
}
