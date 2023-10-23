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
    List<PlayerController> _players = new List<PlayerController>();

    Cam _cam;

    [SerializeField] GameObject _laser;

    [SerializeField] GameObject _doorL;
    [SerializeField] GameObject _doorR;

    [SerializeField] Transform _spawnerL;
    [SerializeField] Transform _spawnerR;


    [SerializeField] public float _givenTime => 20;
    float _actualTime;

    public float GivenTime { get => _givenTime;}


    void Start()
    {
        print(_difficulty);
        _actualTime = _givenTime * _difficulty;
        ThisRoom = transform.parent.parent.GetComponent<Room>();
        _cam = Camera.main.GetComponent<Cam>();
    }
    public override void End(bool isSuccessful)
    {
        Debug.Log("LA FIN DU CACA");
        IsStarted = false;
        StopCoroutine(timeTask());
        BlockDoors(false);
    }

    public override void Init()
    {
        Debug.Log(ThisRoom.ListPlayer.Count + " " + NumberOfPlayers);
        if(ThisRoom.ListPlayer.Count >= NumberOfPlayers)
        {
            foreach (GameObject p in ThisRoom.ListPlayer)
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
        IsStarted = true;
        StartCoroutine(BlockDoors(true));
        _cam.FixOnRoomVoid(ThisRoom);
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
    IEnumerator SpawnLaser()
    {
        yield return new WaitForSeconds(6 - (_difficulty/2));
        SpawnLaser(_laser);
    }
    private void SpawnLaser(GameObject go)
    {
        if (Random.Range(0, 2) == 1)
        {
            GameObject g = Instantiate(go, transform);
            Laser l = g.GetComponent<Laser>();
            l.ToFar = _spawnerL;
        }
        else
        {
            GameObject g = Instantiate(go, transform);
            Laser l = g.GetComponent<Laser>();
            l.ToFar = _spawnerR;
            l._goLeft = false;
        }


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
