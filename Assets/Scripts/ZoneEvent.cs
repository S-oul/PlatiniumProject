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
        _task = Instantiate(DataManager.Instance.AllTasks[(int)_typeTask], this.transform);
    }

    public void PlayerEnter(GameObject player)
    {
        Task taskComp = _task.GetComponent<Task>();
        if (!taskComp.IsDone)
        {
            taskComp.OnPlayerJoinedTask(player);
        }
        
    }   
}
