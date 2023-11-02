using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTask : InputTask
{
    PlayerController _controllerP1;
    PlayerController _controllerP2;

    float angleP1 = 0;
    float oldAngleP1 = 0;

    public override void StartTask()
    {


        _controllerP1 = PlayersDoingTask[0].GetComponent<PlayerController>();
        //_controllerP2 = PlayersDoingTask[1].GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(IsStarted && !IsDone)
        {
            if (_controllerP1.DecrytContext != Vector2.zero)
            {
                angleP1 = Mathf.Atan2(_controllerP1.DecrytContext.y, _controllerP1.DecrytContext.x) * Mathf.Rad2Deg ;
                float delta = oldAngleP1 - angleP1;
                switch (delta)
                {
                    case > 0:
                        print("Clockwise : " + delta);
                    break;
                    case < 0: 
                        print("Counter-Clockwise : " + delta); 
                    break;
                }
                oldAngleP1 = angleP1;
            }
        }
    }
}
