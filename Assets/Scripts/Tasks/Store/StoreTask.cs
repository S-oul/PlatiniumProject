using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class StoreTask : InputTask
{
    PlayerController _controllerP1;
    PlayerController _controllerP2;

    PlayerUI _P1UI;
    PlayerUI _P2UI;


    float _angleP1 = 0;
    float temp1 = 0;
    float _oldAngleP1 = 0;
    float _angleP2 = 0;
    float temp2 = 0;
    float _oldAngleP2 = 0;

    bool _hasPassed90 = false;
    bool _hasPassed180 = false;


    bool _isOnFail = false;
    int _timeHaveFail = 0;
    int _timeCanFail = 5;

    [SerializeField] float _JoystickForce = 1;
    [SerializeField] float _storePosPercent = 0;
    [SerializeField] float _storeSpeed = 1;
    [SerializeField] GameObject _store;
    [SerializeField] Transform _startPos;
    [SerializeField] Transform _endPos;

    [SerializeField] Transform _joystickBG;
    
    [SerializeField] Transform _deadZoneP1;
    [SerializeField] Transform _deadZoneP2;
    [SerializeField] StoreDeadZones _deadzone;

    [SerializeField] AnimationCurve pingpong;
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
            float deltaP1 = 0;
            float deltaP2 = 0;
  
            if (!_isOnFail)
            {
                #region P1
                if (_controllerP1.DecrytContext != Vector2.zero)
                {
                    _angleP1 = Mathf.Atan2(_controllerP1.DecrytContext.y, _controllerP1.DecrytContext.x) * Mathf.Rad2Deg;
                    _deadZoneP1.localEulerAngles = new Vector3(0, 0, _angleP1-270f);
                    _angleP1 = _angleP1 - 180f;

                }
                #endregion

                #region P2
                if (_controllerP2.DecrytContext != Vector2.zero)
                {
                    _angleP2 = Mathf.Atan2(_controllerP2.DecrytContext.y, _controllerP2.DecrytContext.x) * Mathf.Rad2Deg;                    
                    _deadZoneP2.localEulerAngles = new Vector3(0, 0, _angleP2 - 270f);
                    _angleP2 = _angleP2 - 180f;
                }
                #endregion

                if(_angleP1 < -90 && _angleP2 > -180)
                {
                    _hasPassed90 = true;
                }
                if (_angleP1 < -180 && _angleP2 > -270 && _hasPassed90)
                {
                    _hasPassed180 = true;
                }

                if (_angleP1 < -270 || _angleP2 < -270 && !_hasPassed180)
                {
                    _deadZoneP2.localEulerAngles = new Vector3(0, 0, _oldAngleP2 - 90f);
                    _deadZoneP1.localEulerAngles = new Vector3(0, 0, _oldAngleP1 - 90f);

                }
                else
                {
                    _oldAngleP2 = _angleP2;
                    _oldAngleP1 = _angleP1;
                    _storePosPercent = Mathf.Abs(_angleP1 + _angleP2) / 2 / 360;
                    _store.transform.position = Vector3.Lerp(_startPos.position, _endPos.position, _storePosPercent);
                }

                print(_storePosPercent + " // " + _angleP1 + " // " + _angleP2 + "//" + _hasPassed180 + "//" + _hasPassed90);

            }

            if (!_deadzone.IsInOtherCollider)
            {
                _deadzone.IsInOtherCollider = true;
                _isOnFail = true;
                _timeHaveFail++;
                print("FAIIIIIIIIIIIIIIL  " + _timeHaveFail);
                if (_hasPassed180)
                {
                    _deadZoneP1.localEulerAngles = new Vector3(0, 0, 90f);
                    _deadZoneP2.localEulerAngles = new Vector3(0, 0, 90f);
                }
                else if (_hasPassed90)
                {
                    _deadZoneP1.localEulerAngles = new Vector3(0, 0, 180f);
                    _deadZoneP2.localEulerAngles = new Vector3(0, 0, 180f);
                }
                else
                {
                    _deadZoneP1.localEulerAngles = new Vector3(0, 0, 266f);
                    _deadZoneP2.localEulerAngles = new Vector3(0, 0, 266f);
                    
                }

                if (_timeHaveFail == _timeCanFail)
                {
                    End(false);
                }
                else
                {
                    StartCoroutine(FailCoroutine());
                }
                return;

            }

            if (_storePosPercent > .9f && _hasPassed180)
            {
                End(true);
            }

        }
    }

    IEnumerator FailCoroutine()
    {
        float time = .75f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            float percent = time / .75f;
            _storePosPercent = percent;
            yield return null;
        }
        _storePosPercent = 0;
        time = .75f * Difficulty;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;

        }
        _isOnFail = false;
    }
}
