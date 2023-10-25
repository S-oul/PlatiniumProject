using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    PlayerInputManager _inputManager;
   
    private void Awake()
    {
        _inputManager = gameObject.GetComponent<PlayerInputManager>();
    }
    void OnPlayerJoined(PlayerInput playerInput)
    {
        GameManager.Instance.Players[playerInput.playerIndex] = playerInput.gameObject;
        GameManager.Instance.PlayerCount++;
        PickRandomSprite(GameManager.Instance.Players[playerInput.playerIndex]);
        GameManager.Instance.Players[playerInput.playerIndex].transform.position = transform.position;
        playerInput.actions["InputTask"].Disable();
        Camera.main.gameObject.GetComponent<Cam>().Targets.Add(playerInput.gameObject);
    }


    void PickRandomSprite(GameObject player)
    {
        print(player);
        Sprite temp = DataManager.Instance.SpritePlayers[Random.Range(0, DataManager.Instance.SpritePlayers.Count)];
        DataManager.Instance.SpritePlayers.Remove(temp);
        player.GetComponent<SpriteRenderer>().sprite = temp;
    }
}
