using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTask : InputTask
{
    PlayerController _controllerP1;
    PlayerController _controllerP2;
    public override void StartTask()
    {


        _controllerP1 = PlayersDoingTask[0].GetComponent<PlayerController>();
        //_controllerP2 = PlayersDoingTask[1].GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(IsStarted && !IsDone)
        {
            if(_controllerP1.JoystickContext != Vector2.zero) print(_controllerP1.JoystickContext);
        }
    }
}
