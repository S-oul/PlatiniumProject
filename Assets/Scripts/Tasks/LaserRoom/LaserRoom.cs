using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRoom : Task
{
    //[SerializeField] List<GameObject> _listPlayer = new List<GameObject>();
    Room _room;



    void Start()
    {
        _room = transform.parent.parent.GetComponent<Room>();
    }
    public override void End(bool isSuccessful)
    {
        
        throw new System.NotImplementedException();
    }

    public override void Init()
    {
        if(_room.ListPlayer.Count > NumberOfPlayers)
        {
            StartTask();
        }
    }

    void StartTask()
    {

    }
}
