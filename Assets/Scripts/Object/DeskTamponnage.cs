using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeskTamponnage : Object
{
    bool _isUsed;

    List<GameObject> _playersOnDesk = new List<GameObject>();
    List<Transform> _posListOnDesk = new List<Transform>();
    GameObject _task;
    [SerializeField] DataManager.TaskEnum _typeTask;
    [SerializeField] Transform _contourPlayer;
    int _indexPos;

    public GameObject Task { get => _task; }
    private void Awake()
    {
        _indexPos = 0;
        _task = Instantiate(DataManager.Instance.AllTasks[(int)_typeTask], this.transform);
        foreach (Transform pos in gameObject.transform.Find("PlayerPos"))
        {
            _posListOnDesk.Add(pos);
        }
        _contourPlayer.gameObject.SetActive(false);
    }
    public override void Interact(GameObject player)
    {
        if (!_isUsed)
        {
            
            if(_playersOnDesk.Count < 2)
            {
                if(!_playersOnDesk.Contains(player))
                {
                    _playersOnDesk.Add(player);
                    player.transform.position = _posListOnDesk[_indexPos].position;
                    player.transform.Find("Animation").GetComponent<SpriteRenderer>().sortingLayerName = "Deco";
                    player.GetComponent<PlayerController>().BlockPlayer(true);
                    player.GetComponent<PlayerController>().DisableMovementEnableInputs();
                    _task.GetComponent<Task>().OnPlayerJoinedTask(player);
                    player.GetComponent<PlayerUI>().DisplayInputToPress(false, "");
                    _indexPos++;
                    if (_playersOnDesk.Count == 2)
                    {
                        _contourPlayer.gameObject.SetActive(false);
                        _isUsed = true;
                    }
                    else
                    {
                        _contourPlayer.gameObject.SetActive(true);
                    }
                }
                else
                {
                    _playersOnDesk.Remove(player);
                    _indexPos--;
                    player.transform.position = _task.GetComponent<TamponageTask>().gameObject.transform.parent.parent.Find("PlayerRespawnPoint").position;
                    player.transform.Find("Animation").GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                    player.GetComponent<PlayerController>().BlockPlayer(false);
                    player.GetComponent<PlayerController>().EnableMovementDisableInputs();
                    _task.GetComponent<Task>().OnPlayerLeaveTask(player);
                    player.GetComponent<PlayerUI>().DisplayInputToPress(true, "Y");
                    _contourPlayer.gameObject.SetActive(false);
                    //_task.GetComponent<DuolingoTask>().NPCDuolingo.CheckIfDesksAreUsed();


                }
               
            }
            

        }
    }
}
