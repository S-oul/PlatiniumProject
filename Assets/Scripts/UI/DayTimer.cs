using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class DayTimer : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] float _dayTime = 500;
    TimeSpan _time;

    TextMeshProUGUI _text;

    [SerializeField]bool _doTimer = false;

    bool _subMinute() { return _time.Minutes <= 0; }
    bool _wasSubMinute = false;
    bool _hasEnd = false;

    public float DayTime { get => _dayTime; set => _dayTime = value; }
    public bool DoTimer { get => _doTimer; set => _doTimer = value; }

    void Start()
    {
        gameManager = GameManager.Instance;
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _time = TimeSpan.FromSeconds((int)DayTime);
        string m = "";
        string s = "";
        if(_time.Minutes < 10) { m = 0 + _time.Minutes.ToString(); } else { m = _time.Minutes.ToString(); }
        if(_time.Seconds < 10) { s = 0 + _time.Seconds.ToString(); } else { s = _time.Seconds.ToString(); }

        _text.text =  m + " : " + s;

        if (_doTimer)
        {
            DayTime -= Time.deltaTime;
            DayTime = Mathf.Clamp(DayTime, 0, 999);
            if (!_wasSubMinute && _subMinute()) 
            {
                _wasSubMinute = true;
                print("DATIMER UNDER A MINUTE");
            }
            if (!_hasEnd && _time.Minutes == 0 && _time.Seconds == 0) 
            { 
                _hasEnd = true;
                gameManager.Animator.SetTrigger("DoCrunchAnim");
                print("DATIMER ISDONE"); 
            }

        }
    }

}
