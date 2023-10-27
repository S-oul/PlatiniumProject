using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RHTask : Task
{

    public List<GameObject> _playersAsked = new List<GameObject>();
    GameManager _gameManager;
     GameObject _playerNeeded;

    [SerializeField] RH _npcRH;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _npcRH = transform.parent.parent.GetComponentInChildren<RH>();
    }
    public override void End(bool isSuccessful)
    {
        StartCoroutine(FeedBack(isSuccessful));
        _npcRH.IsPlayerNeeded = false;
        base.End(isSuccessful);
    }

    public override void Init()
    {
        
        if (_gameManager.PlayerCount > 1)
        {
            _playerNeeded = null;

            foreach (var playerInGame in _gameManager.Players)
            {
                if (playerInGame != PlayerGameObject && playerInGame != null)
                {
                    _playersAsked.Add(playerInGame);
                }

            }
            _playerNeeded = _playersAsked[Random.Range(0, _playersAsked.Count)];
            _npcRH.DisplayPlayer(_playerNeeded);
            _npcRH.PlayerNeeded = _playerNeeded;
            _npcRH.TaskRH = this;
            _npcRH.IsPlayerNeeded = true;

        }
        else
        {
            _npcRH.Talk("Where are your friends?");
        }
    }
        
    IEnumerator FeedBack(bool value)
    {
        _npcRH.DisplayPlayer(null);
        if (value == true)
        {
            
            _npcRH.Talk("Thank you");
        }
        else
        {
            _npcRH.Talk(">:o");
        }
        yield return new WaitForSeconds(3f);
        _npcRH.NPCUI.DisplayTalkingBubble(false);
    }
}
