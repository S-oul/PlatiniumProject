    using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
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
        playerInput.actions["InputTask"].Disable();
    }

}
