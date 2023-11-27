using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyNPC : NPC, IChattyNPC
{

    [SerializeField] List<string> _dialogues = new List<string>();
    public List<string> dialogueTexts { get => _dialogues; set =>  _dialogues = value; }

    [SerializeField] GameObject _bulletPrefab;

    SpriteRenderer _sprite;

    Transform _firePoint;

    [SerializeField] GameObject _player;



    public GameObject Player { get => _player; set => _player = value; }
    public SpriteRenderer SpriteNPC { get => _sprite; set => _sprite = value; }

    private void Start()
    {
        _firePoint = transform.GetChild(0).transform;
        _sprite = transform.Find("Animation").GetComponent<SpriteRenderer>();

    }
    public void Talk(string text)
    {
        string currentDialogue = dialogueTexts[Random.Range(0, dialogueTexts.Count)];
    }


    public void Fire()
    {
        GetComponentInChildren<Animator>().SetTrigger("Fire");

    }



    public void FlipNPC()
    {
        Vector2 _vectorFromPlayer = Player.transform.position - transform.position;
        float xValue = _vectorFromPlayer.x;
        if (xValue > 0)
        {
            _sprite.flipX = true;
        }
        else
        {
            _sprite.flipX = false;
        }


    }
}

