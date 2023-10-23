using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyNPC : NPC, IChattyNPC
{

    [SerializeField] List<string> _dialogues = new List<string>();
    public List<string> dialogueTexts { get => _dialogues; set =>  _dialogues = value; }

    [SerializeField] GameObject _bulletPrefab;

    Transform _firePoint;

    GameObject _player;

    public GameObject Player { get => _player; }

    private void Start()
    {
        _firePoint = transform.GetChild(0).transform;
    }
    public void Talk(string text)
    {
        string currentDialogue = dialogueTexts[Random.Range(0, dialogueTexts.Count)];
        Debug.Log(currentDialogue);
    }

    public override void Interact(GameObject player)
    {
        return;
    }

    public void Fire()
    {
        Debug.Log("Pan");
    }
}
