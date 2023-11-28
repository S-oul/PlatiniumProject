using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskTamponnage : Object
{
    bool _isUsed;

    List<GameObject> _playersOnDesk = new List<GameObject>();
    List<Transform> _posListOnDesk = new List<Transform>();
    public bool IsUsed { get => _isUsed; set => _isUsed = value; }
    GameObject _task;
    [SerializeField] DataManager.TaskEnum _typeTask;

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
    }
    public override void Interact(GameObject player)
    {
        if (!_isUsed)
        {
            
            if(_playersOnDesk.Count < 2)
            {
                _playersOnDesk.Add(player);
                Debug.Log("Players: " + _playersOnDesk.Count);
                player.transform.position = _posListOnDesk[_indexPos].position;
                //player.transform.Find("Animation").GetComponent<SpriteRenderer>().sortingOrder = 1;
                player.GetComponent<PlayerController>().BlockPlayer(true);
                player.GetComponent<PlayerController>().DisableMovementEnableInputs();
                _task.GetComponent<Task>().OnPlayerJoinedTask(player);
                _indexPos++;
                if (_playersOnDesk.Count == 2)
                {
                    Debug.Log("Used");
                    _isUsed = true;
                }
            } 
        }
    }
}
