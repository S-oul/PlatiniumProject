using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DeskDuolingo : Object
{
    GameObject _task;

    [SerializeField] Transform _contourPlayer;

    bool _isUsed;
    public GameObject TaskDuolingo { get => _task; set => _task = value; }
    public Transform ContourPlayer { get => _contourPlayer; set => _contourPlayer = value; }

    private void Start()
    {
        _contourPlayer.gameObject.SetActive(false);
        _isUsed = false;
    }

    public override void Interact(GameObject player)
    {
        if (!_task.GetComponent<Task>().IsDone)
            if (!IsUsed)
            {
                IsUsed = true;
                _task.GetComponent<DuolingoTask>().NPCDuolingo.CheckIfDesksAreUsed();
                _contourPlayer.gameObject.SetActive(false);
                player.transform.position = gameObject.transform.Find("PlayerPosition").position;
                player.GetComponent<PlayerController>().BlockPlayer(true);
                player.transform.Find("Animation").GetComponent<SpriteRenderer>().sortingLayerName = "Deco";
                player.GetComponent<PlayerController>().DisableMovementEnableInputs();
                _task.GetComponent<Task>().OnPlayerJoinedTask(player);
                player.GetComponent<PlayerUI>().DisplayInputToPress(false, "");
                
            }
            else
            {
                if (!_task.GetComponent<DuolingoTask>().IsStarted)
                {
                    IsUsed = false;
                    _task.GetComponent<DuolingoTask>().NPCDuolingo.CheckIfDesksAreUsed();
                    player.transform.position = _task.transform.parent.parent.Find("PlayerRespawnPoint").position;
                    player.GetComponent<PlayerController>().BlockPlayer(false);
                    player.transform.Find("Animation").GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                    player.GetComponent<PlayerController>().EnableMovementInteractDisableInputs();
                    _task.GetComponent<Task>().OnPlayerLeaveTask(player);
                    player.GetComponent<PlayerUI>().DisplayInputToPress(false, "");
                }
                

            }





    }

    
        
    
}
