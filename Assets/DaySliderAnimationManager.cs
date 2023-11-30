using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class DaySliderAnimationManager : MonoBehaviour
{
    //[SerializeField] Vector2 _startPosition;
    [SerializeField] Vector2 _endPosition;

    float distance;
    Vector3 newPos;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 _startPosition = GetComponent<Transform>().position;
        
        distance = _startPosition.x - _endPosition.x;
        //distance = 630.4f + 870f;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GameManager.Instance.DaySlider.DaySliderValue);
        newPos.x =  Mathf.Lerp(-940f,635.4f, GameManager.Instance.DaySlider.DaySliderValue);
        newPos.y = 1.3f;
        GetComponent<RectTransform>().localPosition = newPos;
        //print(GetComponent<RectTransform>().localPosition + " // " + newPos);

        if (GameManager.Instance.DaySlider.DaySliderValue < 0.05f) { this.gameObject.SetActive(false); }

        UpdateColor();
    }

    void UpdateColor()
    {
        GetComponent<Image>().color = GameManager.Instance.DaySlider.SliderColor;
    }

}
