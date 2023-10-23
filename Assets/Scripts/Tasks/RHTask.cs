using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RHTask : Task
{

    List<GameObject> _playersAsked = new List<GameObject>();
    GameManager _gameManager;
    GameObject _playerNeeded;

    [SerializeField] RH _npcRH;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _npcRH = transform.parent.parent.GetComponentInChildren<RH>();
        Debug.Log(_npcRH);
    }
    public override void End(bool isSuccessful)
    {
        
    }

    public override void Init()
    {
        _playerNeeded = null;
        
        foreach (var playerInGame in _gameManager.Players)
        {
            if (playerInGame != _player && playerInGame != null)
            {
                _playersAsked.Add(playerInGame);
            }

        }
        _playerNeeded = _playersAsked[Random.Range(0, _playersAsked.Count)];
        _npcRH.DisplayPlayer( _playerNeeded);
    }

}
