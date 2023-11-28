using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneEvent : MonoBehaviour
{
    [SerializeField] GameObject _task;

    [SerializeField] DataManager.TaskEnum _typeTask;
    public GameObject Task { get => _task; }

    private void Awake()
    {
        _task = Instantiate(DataManager.Instance.AllTasks[(int)_typeTask]);
        _task.transform.parent = this.transform;
        _task.transform.position = this.transform.position;
    }

    public void PlayerEnter(GameObject player)
    {
        Task taskComp = _task.GetComponent<Task>();
        print("HAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAa");
        if (!taskComp.IsDone)
        { 
            taskComp.OnPlayerJoinedTask(player);
        }
        
    }   
}
