using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Daycycle : MonoBehaviour
{
    [SerializeField] private Light2D _lightDay;
    [SerializeField] private Gradient _dayColor;
    [SerializeField] private bool _pauseTime;
    [Space]
    [SerializeField] private Light2D _lightNight;
    [SerializeField] private Gradient _nightColor;
    [Space]
    [Range(0, 100)]
    [Tooltip("In percent %")]
    [SerializeField] private float _timeOfDay_Percent;
    [Space]
    [SerializeField] private float _maxDayTime = 60;
    [SerializeField] private float _timeOfDay = 0;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!_pauseTime)
        {
            _timeOfDay_Percent = (_timeOfDay * 100) / _maxDayTime;
            _timeOfDay += Mathf.Round(Time.deltaTime * 1000) / 1000;
            if (_timeOfDay >= _maxDayTime)
            {
                _timeOfDay = 0;
            }
            UpdateLight();
        }

    }
    public void UpdateLight()
    {
        _lightDay.color = _dayColor.Evaluate(_timeOfDay_Percent / 100);
        //_lightNight.color = _nightColor.Evaluate(_timeOfDay_Percent / 100);
        //transform.localEulerAngles = new Vector3(-18, -_timeOfDay_Percent * 3.6f + 180, 0f);
    }
    private void OnValidate()
    {
        _timeOfDay = (_timeOfDay_Percent / 100) * _maxDayTime;
        UpdateLight();

    }
}
