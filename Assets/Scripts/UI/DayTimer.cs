using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class DayTimer : MonoBehaviour
{
    public float DayTime = 500;

    TextMeshProUGUI _text;

    public bool doTimer = false;
    
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        TimeSpan time = TimeSpan.FromSeconds((int)DayTime);
        string m = "";
        string s = "";
        if(time.Minutes < 10) { m = 0 + time.Minutes.ToString(); } else { m = time.Minutes.ToString(); }
        if(time.Seconds < 10) { s = 0 + time.Seconds.ToString(); } else { s = time.Seconds.ToString(); }

        _text.text =  m + " : " + s;

        if(doTimer) DayTime -= Time.deltaTime;
    }

}
