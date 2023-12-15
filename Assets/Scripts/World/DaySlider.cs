using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class DaySlider : MonoBehaviour
{
    Slider _slider;
    [SerializeField] float _unclampedValue = 1;
    [SerializeField] float _speedTimer = .3f;
    [SerializeField] float _speedUnclamped = .1f;
    [Tooltip("In Percent")]
    [SerializeField] float _OnRoomLoose = .20f;
    [SerializeField] float _OnRoomWin = .10f;
    [SerializeField] Gradient _gradient = new Gradient();
    [SerializeField] Color _startColor = Color.green;
    [SerializeField] Color _endColor = Color.red;

    [SerializeField] Image FillImageComponent;

    float _totalValue = 0;

    Color _currentColor;

    private bool _isOnCrunch = false;
    bool _doSlider = true;
    public float OnRoomLoose { get => _OnRoomLoose; set => _OnRoomLoose = value; }
    public float OnRoomWin { get => _OnRoomWin; set => _OnRoomWin = value; }
    public bool IsOnCrunch { get => _isOnCrunch; set => _isOnCrunch = value; }
    public float DaySliderValue { get => _unclampedValue; }
    public Color SliderColor { get => _currentColor; }

    public bool DoSlider { get => _doSlider; set => _doSlider = value; }
    public float TotalValue { get => _totalValue; set => _totalValue = value; }

    private void Start()
    {
        _slider = GetComponent<Slider>();
        Color _currentColor = _startColor;
        TotalValue = _slider.value;
    }
    private void Update()
    {
        _unclampedValue = Mathf.Clamp01(_unclampedValue);
        if (DoSlider)
        {
            RemoveValue(_speedTimer * Time.deltaTime);
        }
        if (_unclampedValue <= 0)
        {
            GetFired();
        }
        _slider.value = Mathf.Lerp(_slider.value, _unclampedValue, _speedUnclamped);

        UpdateColor();

    }
    public float SetValue(float val)
    {
        if (!_isOnCrunch)
        {
            _unclampedValue = val;
        }
        return _unclampedValue;

    }
    public float AddValue(float val)
    {
        if (!_isOnCrunch)
        {
            _unclampedValue += val;
        }
        return _unclampedValue;
    }
    public float RemoveValue(float val)
    {
        _unclampedValue -= val;
        return _unclampedValue;
    }

    public float GetValue()
    {
        return _unclampedValue;
    }

    public void GetFired()
    {
        SceneManager.LoadScene(3);
    }

    private void SetColor()
    {
        //_slider. 
        return;
    }

    [Button]
    public void Add20()
    {
        AddValue(.2f);
    }
    [Button]
    public void Remove20()
    {
        RemoveValue(.2f);
    }
    public void UpdateColor()
    {
        _currentColor = _gradient.Evaluate(_unclampedValue);
        FillImageComponent.color = _currentColor;
    }
}
