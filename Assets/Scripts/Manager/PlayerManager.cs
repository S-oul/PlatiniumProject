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
        PickRandomSprite(GameManager.Instance.Players[playerInput.playerIndex]);
        GameManager.Instance.Players[playerInput.playerIndex].transform.position = transform.position;
        playerInput.actions["InputTask"].Disable();
        Camera.main.gameObject.GetComponent<Cam>().Targets.Add(playerInput.gameObject);
        Debug.Log(playerInput.devices[0]);
    }
    void PickRandomSprite(GameObject player)                                                                         //Change to Pick randome Animation
    {
        print(player);
        Sprite temp = DataManager.Instance.SpritePlayers[Random.Range(0, DataManager.Instance.SpritePlayers.Count)]; // find sprite in list   - change to list of animations
        DataManager.Instance.SpritePlayers.Remove(temp);                                                             // remove sprite->Animation from list (to avoid identical players)   
        player.GetComponent<SpriteRenderer>().sprite = temp;                                                         // assign sprite->Animation to player's SpriteRenderer-> Animator. 
    }
}
