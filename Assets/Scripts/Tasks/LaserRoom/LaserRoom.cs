using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    float _actualTime;

    public float GivenTime { get => _givenTime;}


    void Start()
    {
        //_gameManager = GameManager.Instance;
        //NumberOfPlayers = _gameManager.PlayerCount;
        _actualTime = _givenTime * (Difficulty / 3f);
        //print(_actualTime);
        RoomTask = transform.parent.parent.GetComponent<Room>();
        _cam = Camera.main.GetComponent<Cam>();
    }
    public override void End(bool isSuccessful)
    {
        StopAllCoroutines();
        IsStarted = false;
        IsDone = true;
        _cam.FixOnRoom = false;
        StartCoroutine(BlockDoors(false));
        if (isSuccessful)
        {
            StartCoroutine(RecuperatePlayer());
        }
        else
        {
            StartCoroutine(RecuperatePlayer());
        }
        base.End(isSuccessful);
    }
    void KillAllLaser()
    {
        for(int i = _laserGO.Count - 1; i > 0;i--)
        {
            Destroy(_laserGO[i]);
            print(i);
        }
    }
    public override void Init()
    {
        base.Init();
        if(RoomTask.ListPlayer.Count >= NumberOfPlayers)
        {
            foreach (GameObject p in RoomTask.ListPlayer)
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
        
        if (block)
        {
            yield return new WaitForSeconds(.25f);
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
        _cam.FixOnRoomVoid(RoomTask);
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
        string t = "";
        foreach (PlayerController _controller in _players)
        {
            if (!_controller.IsPlayerDown)
            {
                t+= "t";
            }
        }
        if(t == "tttt")
        {
            return true;
        }
        return false;
    }
    
    private void SpawnLaser(GameObject go)
    {
        int r = Random.Range(0, 3);
        if (r == 2)
        {
            GameObject g = Instantiate(go);
            g.transform.parent = null;
            g.transform.position = _spawnerR.position;
            g.transform.localScale = new Vector3(.16f, .16f, .16f);
            Laser l = g.GetComponent<Laser>();
            l.ToFar = _spawnerL;
            _laserGO.Add(g);

            GameObject g2 = Instantiate(go);
            g2.transform.parent = null;
            g2.transform.position = _spawnerL.position;
            g2.transform.localScale = new Vector3(.16f, .16f, .16f);
            Laser l2 = g2.GetComponent<Laser>();
            l2.ToFar = _spawnerR;
            l2.GoLeft = false;
            _laserGO.Add(g);

        }
        else if (r == 1)
        {
            GameObject g = Instantiate(go);
            g.transform.parent = null;
            g.transform.position = _spawnerL.position;
            g.transform.localScale = new Vector3(.16f, .16f, .16f);
            Laser l = g.GetComponent<Laser>();
            l.ToFar = _spawnerR;
            l.GoLeft = false;
            _laserGO.Add(g);

        }
        else
        {
            GameObject g = Instantiate(go);
            g.transform.parent = null;
            g.transform.position = _spawnerR.position;
            g.transform.localScale = new Vector3(.16f, .16f, .16f);
            Laser l = g.GetComponent<Laser>();
            l.ToFar = _spawnerL;
            _laserGO.Add(g);
        }
    }
    IEnumerator RecuperatePlayer()
    {
        yield return new WaitForSeconds(_recuperateTime);
        foreach (PlayerController _controller in _players)
        {
            _controller.EnableMovementDisableInputs();

        }
        KillAllLaser();
    }
    IEnumerator SpawnLaserTimer()
    {
        while (IsStarted)
        {
            yield return new WaitForSeconds(Random.Range(6 - Difficulty - 0.25f, 6 - Difficulty + 0.25f));
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
