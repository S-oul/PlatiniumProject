using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaserRoom : Task , ITimedTask
{
    //[SerializeField] List<GameObject> _listPlayer = new List<GameObject>();

    Room _room;

    List<PlayerController> _players = new List<PlayerController>();

    Cam _cam;

    [SerializeField] GameObject _laser;

    [SerializeField] GameObject _doorL;
    [SerializeField] GameObject _doorR;

    [SerializeField] public float _givenTime => 20;
    float _actualTime;

    public float GivenTime { get => _givenTime;}


    void Start()
    {
        print(_difficulty);
        _actualTime = _givenTime * _difficulty;
        _room = transform.parent.parent.GetComponent<Room>();
        _cam = Camera.main.GetComponent<Cam>();
    }
    public override void End(bool isSuccessful)
    {
        Debug.Log("LA FIN DU CACA");
        StopCoroutine(timeTask());
        BlockDoors(false);
    }

    public override void Init()
    {
        Debug.Log(_room.ListPlayer.Count + " " + NumberOfPlayers);
        if(_room.ListPlayer.Count >= NumberOfPlayers)
        {
            foreach (GameObject p in _room.ListPlayer)
            {
                _players.Add(p.GetComponent<PlayerController>());
            }
            StartTask();

        }
    }

    IEnumerator BlockDoors(bool block)
    {
        BoxCollider2D b = _doorL.GetComponent<BoxCollider2D>();
        BoxCollider2D b2 = _doorR.GetComponent<BoxCollider2D>();
        yield return new WaitForSeconds(.5f);
        if (block)
        {
            b.enabled = true;
            b2.enabled = true;
        }
        else
        {
            print("BLOCKED");
            b.enabled = false;
            b2.enabled = false;
        }
    }
    void StartTask()
    {
        StartCoroutine(BlockDoors(true));
        _cam.FixOnRoomVoid(_room);
        StartCoroutine(timeTask());
    }

    bool OnePlayerAlive()
    {
        foreach (PlayerController _controller in _players)
        {
            if (_controller.CanMove) 
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator timeTask()
    {
        while (_actualTime > 0) 
        {
            if (!OnePlayerAlive())
            {
                End(false);
            }
            _actualTime -= Time.deltaTime;
            Debug.Log(_actualTime);
            yield return null;
        }       
        if (_actualTime <= 0)
        {
            _cam.FixOnRoom = false;
        }

    }
}
