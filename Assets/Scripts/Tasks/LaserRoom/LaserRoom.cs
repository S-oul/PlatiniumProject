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

    List<GameObject> _laserGO = new List<GameObject>();


    Cam _cam;

    [SerializeField] GameObject _laser;

    [SerializeField] GameObject _doorL;
    [SerializeField] GameObject _doorR;

    [SerializeField] Transform _spawnerL;
    [SerializeField] Transform _spawnerR;


    public float _givenTime => 20;
    [SerializeField] float _recuperateTime => 2;

    [SerializeField] float tttt = 5;


    float _actualTime;

    public float GivenTime { get => _givenTime;}


    void Start()
    {
        _actualTime = _givenTime * (_difficulty / 3f);
        print(_actualTime);
        ThisRoom = transform.parent.parent.GetComponent<Room>();
        _cam = Camera.main.GetComponent<Cam>();
    }
    public override void End(bool isSuccessful)
    {
        Debug.Log("END PTDRRRRRRRRR : " + isSuccessful);
        KillAllLaser();
        IsStarted = false;
        IsDone = true;
        _cam.FixOnRoom = false;
        StopAllCoroutines();
        StartCoroutine(BlockDoors(false));
        if (isSuccessful)
        {
            StartCoroutine(RecuperatePlayer());
        }
        else
        {
            StartCoroutine(RecuperatePlayer());
        }
    }
    void KillAllLaser()
    {
        foreach(GameObject g in _laserGO)
        {
            Destroy(g);
        }
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
        print("DOOOOOOOOOOOOOOOORS" + block + " " + b + b2);
        if (block)
        {
            yield return new WaitForSeconds(.5f);
            b.enabled = true;
            b2.enabled = true;
        }
        else
        {
            b.enabled = false;
            b2.enabled = false;
        }
    }
    void StartTask()
    {
        IsStarted = true;
        StartCoroutine(SpawnLaserTimer());
        StartCoroutine(BlockDoors(true));
        StartCoroutine(timeTask());
        _cam.FixOnRoomVoid(ThisRoom);
    }

    bool OnePlayerAlive()
    {
        foreach (PlayerController _controller in _players)
        {
            if (!_controller.IsPlayerDown) 
            {

                return true;
            }
        }
        return false;
    }
    bool AllPlayerAlive()
    {
        return false;
    }
    
    private void SpawnLaser(GameObject go)
    {
        GameObject g = Instantiate(go);
        if (Random.Range(0, 2) == 1)
        {
            g.transform.parent = null;
            g.transform.position = _spawnerR.position;
            g.transform.localScale = new Vector3(.16f, .16f, .16f);
            Laser l = g.GetComponent<Laser>();
            l.ToFar = _spawnerL;
        
        }
        else
        {
            g.transform.parent = null;
            g.transform.position = _spawnerL.position;
            g.transform.localScale = new Vector3(.16f, .16f, .16f);
            Laser l = g.GetComponent<Laser>();
            l.ToFar = _spawnerR;
            l._goLeft = false;
        }

        _laserGO.Add(g);
    }
    IEnumerator RecuperatePlayer()
    {
        yield return new WaitForSeconds(_recuperateTime);
        foreach (PlayerController _controller in _players)
        {
            _controller.EnableMovement();

        }

    }
    IEnumerator SpawnLaserTimer()
    {
        while (IsStarted)
        {
            yield return new WaitForSeconds(6 - (_difficulty / 2));
            SpawnLaser(_laser);
        }
    }
    IEnumerator timeTask()
    {
        while (_actualTime > 0) 
        {
            //Debug.Log(_actualTime);
            if (!OnePlayerAlive())
            {
                End(false);
            }
            _actualTime -= Time.deltaTime;
            yield return null;
        }       
        if (_actualTime <= 0)
        {
            End(true);
            _cam.FixOnRoom = false;
        }

    }
}
