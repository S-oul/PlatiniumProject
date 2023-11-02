using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTask : InputTask
{
    PlayerController _controllerP1;
    PlayerController _controllerP2;

    float _angleP1 = 0;
    float _oldAngleP1 = 0;
    float _angleP2 = 0;
    float _oldAngleP2 = 0;

    int _P1RotateCount = 0;
    int _P2RotateCount = 0;

    [SerializeField] float _storePosPercent = 0;
    [SerializeField] float _storeSpeed = 1;
    [SerializeField] GameObject _store;
    [SerializeField] Transform _startPos;
    [SerializeField] Transform _endPos;



    public override void StartTask()
    {


        _controllerP1 = PlayersDoingTask[0].GetComponent<PlayerController>();
        //_controllerP2 = PlayersDoingTask[1].GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(IsStarted && !IsDone)
        {
            #region P1
            if (_controllerP1.DecrytContext != Vector2.zero)
            {
                _angleP1 = Mathf.Atan2(_controllerP1.DecrytContext.y, _controllerP1.DecrytContext.x) * Mathf.Rad2Deg ;
                float delta = _oldAngleP1 - _angleP1;
                float _360angleP1 = _angleP1 + 180;

                if (delta > 0) _P1RotateCount++;
                else if (delta < 0) _P1RotateCount = 0;

                
                _oldAngleP1 = _angleP1;
            }
            else
            {
                _P1RotateCount = 0;
            }

            #endregion
           /* #region P2
            if (_controllerP2.DecrytContext != Vector2.zero)
            {
                _angleP2 = Mathf.Atan2(_controllerP2.DecrytContext.y, _controllerP2.DecrytContext.x) * Mathf.Rad2Deg;
                float delta = _oldAngleP2 - _angleP2;
                float _360angleP2 = _angleP2 + 180;

                if (delta > 0) _P2RotateCount++;
                else if (delta < 0) _P2RotateCount = 0;


                _oldAngleP2 = _angleP2;
            }
            else
            {
                _P2RotateCount = 0;
            }

            if (_P2RotateCount > 9)
            {
                print(_P2RotateCount + " ISROTATIN");
            }
            #endregion*/

            if(P1Spinning() /*&& P2Spinning()*/)
            {
                _storePosPercent += _storeSpeed/((float)Difficulty/3f) * Time.deltaTime;
                print(_storePosPercent);
            }
            else
            {
                _storePosPercent -= _storeSpeed / ((float)Difficulty / 3f) * Time.deltaTime;
            }
            _storePosPercent = Mathf.Clamp01(_storePosPercent);
            _store.transform.position = Vector3.Lerp(_startPos.position, _endPos.position, _storePosPercent); 
        }
    }

    bool P1Spinning()
    {
        if(_P1RotateCount > 9)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool P2Spinning()
    {
        if (_P2RotateCount > 9)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
