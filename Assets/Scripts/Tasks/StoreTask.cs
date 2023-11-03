using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreTask : InputTask
{
    PlayerController _controllerP1;
    PlayerController _controllerP2;

    PlayerUI _P1UI;
    PlayerUI _P2UI;



    float _angleP1 = 0;
    float _oldAngleP1 = 0;
    float _angleP2 = 0;
    float _oldAngleP2 = 0;

    int _P1RotateCount = 0;
    int _P2RotateCount = 0;

    [SerializeField] float _JoystickForce = 1;
    [SerializeField] float _storePosPercent = 0;
    [SerializeField] float _storeSpeed = 1;
    [SerializeField] GameObject _store;
    [SerializeField] Transform _startPos;
    [SerializeField] Transform _endPos;

    float _deltaLever = 0;

    [SerializeField] Transform _joystickBG;
    [SerializeField] Transform _joystickP1;
    [SerializeField] Transform _joystickP2;
    
    [SerializeField] Transform _deadZoneP1;
    [SerializeField] Transform _deadZoneP2;


    public override void StartTask()
    {
        _P1UI = PlayersDoingTask[0].GetComponent<PlayerUI>();
        _P2UI = PlayersDoingTask[1].GetComponent<PlayerUI>();


        _controllerP1 = PlayersDoingTask[0].GetComponent<PlayerController>();
        _controllerP2 = PlayersDoingTask[1].GetComponent<PlayerController>();
    }
    public override void End(bool isSuccessful)
    {
        _controllerP1.DisableDecryptageEnableMovements();
        _controllerP2.DisableDecryptageEnableMovements();

        base.End(isSuccessful);

    }
    private void Update()
    {
        if(IsStarted && !IsDone)
        {
            float deltaP1;
            float deltaP2;
            #region P1
            if (_controllerP1.DecrytContext != Vector2.zero)
            {
                _joystickP1.localPosition = _controllerP1.DecrytContext * _JoystickForce;
                //print(_P1UI.Joystick.position);
                _angleP1 = Mathf.Atan2(_controllerP1.DecrytContext.y, _controllerP1.DecrytContext.x) * Mathf.Rad2Deg ;
                deltaP1 = _oldAngleP1 - _angleP1;
                float _360angleP1 = _angleP1 - 90 - 180;
                _deadZoneP1.eulerAngles = new Vector3(0, 0, _360angleP1);

                if (deltaP1 > 0) _P1RotateCount++;
                else if (deltaP1 < 0) _P1RotateCount = 0;

                
                _oldAngleP1 = _angleP1;
            }
            else
            {
                _joystickP1.localPosition = Vector3.zero;
                _P1RotateCount = 0;
            }

            #endregion
            #region P2
            if (_controllerP2.DecrytContext != Vector2.zero)
            {
                _joystickP2.localPosition = _controllerP2.DecrytContext * _JoystickForce;

                _angleP2 = Mathf.Atan2(_controllerP2.DecrytContext.y, _controllerP2.DecrytContext.x) * Mathf.Rad2Deg;
                deltaP2 = _oldAngleP2 - _angleP2;
                float _360angleP2 = _angleP2 - 90 - 180;
                _deadZoneP2.eulerAngles = new Vector3(0, 0, _360angleP2);


                if (deltaP2 > 0) _P2RotateCount++;
                else if (deltaP2 < 0) _P2RotateCount = 0;


                _oldAngleP2 = _angleP2;
            }
            else
            {
                _joystickP2.localPosition = Vector3.zero;
                _P2RotateCount = 0;
            }

            _deltaLever += Time.deltaTime;
            


            #endregion

            /*            if()
                        {
                            _storePosPercent += _storeSpeed/((float)Difficulty/3f) * Time.deltaTime;
                            print(_storePosPercent);
                        }
                        else
                        {
                            _storePosPercent -= _storeSpeed / ((float)Difficulty / 3f) * Time.deltaTime;
                        }
                        if(_storePosPercent > 1) { End(true); }*/
            _storePosPercent = Mathf.Clamp01(_storePosPercent);
            _store.transform.position = Vector3.Lerp(_startPos.position, _endPos.position, _storePosPercent); 
        }
    }
}
