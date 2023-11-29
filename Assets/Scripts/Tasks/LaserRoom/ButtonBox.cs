using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBox : MonoBehaviour
{
    LaserRoom _laserRoom;

    [SerializeField] bool _isOn;

    [SerializeField] SpriteRenderer _spriteRenderer;
    
    [SerializeField] Sprite _spriteOn;
    [SerializeField] Sprite _spriteOff;



    public bool IsOn { get => _isOn; set => _isOn = value; }

    private void OnEnable()
    {
        _laserRoom = FindObjectOfType<LaserRoom>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print("IIIIIIIIIIIIIIIIIIIINNNNNNNNNNNNNN");
            _isOn = true;
            _laserRoom.CheckPhase();
            _spriteRenderer.color = Color.green;
            //_spriteRenderer.sprite = _spriteOn;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _isOn = false;
            _spriteRenderer.color = Color.red;

            //_spriteRenderer.sprite = _spriteOff;
        }

    }

}
