using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskRH : Object
{
    GameObject _player;
    bool _isUsed;
    List<GameObject> _playersAsked = new List<GameObject>();
    GameManager _gameManager;

    public GameObject Player { get =>  _player; }

    public bool IsUsed { get => _isUsed; set => _isUsed = value; }

    private void Start()
    {
        _gameManager = GameManager.Instance;    
    }
    public override void Interact(GameObject player)
    {
        _player = player;
        _isUsed = true;
        foreach (var playerInGame in _gameManager.Players)
        {
            if (playerInGame != _player && playerInGame != null)
            {
                _playersAsked.Add(playerInGame);
            }

        }

    }


}
