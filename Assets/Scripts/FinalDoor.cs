using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    private GameObject _door1;
    private GameObject _door2;

    [SerializeField] Sprite _volleyDoorL;
    [SerializeField] Sprite _volleyDoorR;
    [SerializeField] Sprite _matrixDoorL;
    [SerializeField] Sprite _matrixDoorR;
    [SerializeField] Sprite _laserDoorL;
    [SerializeField] Sprite _laserDoorR;

    bool _isOpened = false;

    float _d1PosX;
    float _d2PosX;
    public bool IsOpened { get => _isOpened; set => _isOpened = value; }

    void Start()
    {
        _door1 = transform.GetChild(0).gameObject;
        _door2 = transform.GetChild(1).gameObject;
        _d1PosX = _door1.transform.localPosition.x;
        _d2PosX = _door2.transform.localPosition.x;
        switch(GameManager.Instance.DayIndex)
        {
            case 0:
                _door1.GetComponent<SpriteRenderer>().sprite = _volleyDoorL;
                _door2.GetComponent<SpriteRenderer>().sprite = _volleyDoorR;
                break;
            case 1:
                _door1.GetComponent<SpriteRenderer>().sprite = _matrixDoorL;
                _door2.GetComponent<SpriteRenderer>().sprite = _matrixDoorR;
                break;
            case 2:
                _door1.GetComponent<SpriteRenderer>().sprite = _laserDoorL;
                _door2.GetComponent<SpriteRenderer>().sprite = _laserDoorR;
                break;
        }
    }

    public IEnumerator OpenDoor()
    {
        GameManager.Instance.DaySlider.DoSlider = false;
        while (_d1PosX > -0.75f && _d2PosX < 0.75f)
        {
            _d1PosX -= Time.deltaTime;
            _d2PosX += Time.deltaTime;
            _door1.transform.localPosition = new Vector3(_d1PosX, _door1.transform.localPosition.y, _door1.transform.localPosition.z);
            _door2.transform.localPosition = new Vector3(_d2PosX, _door2.transform.localPosition.y, _door2.transform.localPosition.z);
            yield return null;
        }
        _isOpened = true;
    }

    public void EnterInTheDoor()
    {
        if (_isOpened)
        {
            GameManager.Instance.GoToFinalRoom();
        }

    }
}
