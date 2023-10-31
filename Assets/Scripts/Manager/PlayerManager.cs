using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    PlayerInputManager _inputManager;
   
    private void Awake()
    {
        _inputManager = gameObject.GetComponent<PlayerInputManager>();
    }
    void OnPlayerJoined(PlayerInput newPlayer)
    {
        GameManager.Instance.Players[newPlayer.playerIndex] = newPlayer.gameObject;
        GameManager.Instance.PlayerCount++;
        //PickRandomSprite(GameManager.Instance.Players[newPlayer.playerIndex]);                          // Randome sprite->animatior chosen here
        GameManager.Instance.Players[newPlayer.playerIndex].transform.position = transform.position;    // Player spawns at the location of the PlayerManager Object
        newPlayer.actions["InputTask"].Disable();                                                       // is "InputTask" the 'task' of adding a player?
        Camera.main.gameObject.GetComponent<Cam>().Targets.Add(newPlayer.gameObject);
    }
    void PickRandomSprite(GameObject player)                                                                         //Change to Pick randome Animation
    {
        print(player);
        Sprite temp = DataManager.Instance.SpritePlayers[Random.Range(0, DataManager.Instance.SpritePlayers.Count)]; // find sprite in list   - change to list of animations
        DataManager.Instance.SpritePlayers.Remove(temp);                                                             // remove sprite->Animation from list (to avoid identical players)   
        player.GetComponent<SpriteRenderer>().sprite = temp;                                                         // assign sprite->Animation to player's SpriteRenderer-> Animator. 
    }
}
