using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CowboyNPC : NPC, IChattyNPC
{

    [SerializeField] List<string> _dialogues = new List<string>();
    public List<string> dialogueTexts { get => _dialogues; set =>  _dialogues = value; }

    [SerializeField] GameObject _bulletPrefab;

    SpriteRenderer _spriteIdle;
    SpriteRenderer _spriteSpe;

    Transform _firePoint;

    [SerializeField] GameObject _player;

    [SerializeField] GameObject _animationIdle;
    [SerializeField] GameObject _animationOther;

    public GameObject Player { get => _player; set => _player = value; }

    private void Start()
    {
        _firePoint = transform.GetChild(0).transform;
        SwitchToSpeAnimations(false);
        _spriteIdle = transform.Find("Cowboy").GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        _spriteSpe = transform.Find("Animation").GetComponent<SpriteRenderer>();
    }
    public void Talk(string text)
    {
        string currentDialogue = dialogueTexts[Random.Range(0, dialogueTexts.Count)];
    }


    public void Fire()
    {
        SwitchToSpeAnimations(true);
        GetComponentInChildren<Animator>().SetTrigger("Fire");
        AudioManager.instance.PlaySFXOS("SherifShootgun", gameObject.GetComponent<AudioSource>());
    }

  
    public void SwitchToSpeAnimations(bool value)
    {
       
        _animationOther.SetActive(value);
        _animationIdle.SetActive(!value);
    }

    public void FlipNPC()
    {
        
        Vector2 _vectorFromPlayer = Player.transform.position - transform.position;
        float xValue = _vectorFromPlayer.x;
        if (xValue > 0)
        {
            _spriteSpe.flipX = true;
            _spriteIdle.flipX = true;
        }
        else
        {
            _spriteSpe.flipX = false;
            _spriteIdle.flipX = false;
        }


    }
}

