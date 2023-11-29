using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRoom : Task
{
    //[SerializeField] List<GameObject> _listPlayer = new List<GameObject>();
    List<PlayerController> _players = new List<PlayerController>();

    List<GameObject> _laserGO = new List<GameObject>();

    [SerializeField] GameObject _toActivate1;
    [SerializeField] GameObject _toActivate2;
    [SerializeField] GameObject _toActivate3;
    [SerializeField] GameObject _toActivate4;

    [SerializeField] List<ButtonBox> _buttonPhase1 = new List<ButtonBox>();
    [SerializeField] List<ButtonBox> _buttonPhase2 = new List<ButtonBox>();
    [SerializeField] List<ButtonBox> _buttonPhase3 = new List<ButtonBox>();
    [SerializeField] List<ButtonBox> _buttonPhase4 = new List<ButtonBox>();

    [SerializeField] List<LaserSpawner> _phase1 = new List<LaserSpawner>();
    [SerializeField] List<LaserSpawner> _phase2 = new List<LaserSpawner>();

    List<bool> _isPhase = new List<bool>();

    int _actPhase = 0;

    Cam _cam;

    [SerializeField] GameObject _laser;

    [SerializeField] GameObject _doorL;
    [SerializeField] GameObject _doorR;

    [SerializeField] Transform _spawnerL;
    [SerializeField] Transform _spawnerR;


    //public float _givenTime => 45;
    [SerializeField] float _recuperateTime => 2;

    public List<GameObject> LaserGO { get => _laserGO; set => _laserGO = value; }

    float _actualTime;

    void Start()
    {
        // _actualTime = _givenTime;
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
        }
    }
    public override void Init()
    {
        base.Init();
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayMusic("LaserRoomMusic");
        }
        if(RoomTask.ListPlayer.Count >= NumberOfPlayers)
        {
            foreach (GameObject p in RoomTask.ListPlayer)
            {
                _players.Add(p.GetComponent<PlayerController>());
            }
            StartTask();
            RoomTask.BoxCollider.enabled = false;
        }
    }
    private void Update()
    {
        if(IsStarted && !IsDone)
        {

            if (!OnePlayerAlive())
            {
                End(false);
            }
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
        _cam.FixOnRoomVoid(RoomTask);
        foreach (LaserSpawner ls in _phase1)
        {
            StartCoroutine(ls.SpawnLaserTimer());
        }
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

    /*bool AllPlayerAlive()
    {
        string t = "";
        foreach (PlayerController _controller in _players)
        {
            if (!_controller.IsPlayerDown)
            {
                t += "t";
            }
        }
        if (t == "tttt")
        {
            return true;
        }
        return false;
    }*/

    public bool CheckPhase()
    {
        print("BBBBBBBBBBBBBBcccccccccccccc       " + _actPhase);
        switch (_actPhase)
        {
            case 0:
                foreach (ButtonBox b in _buttonPhase1)
                {
                    if (b.IsOn == false)
                    {
                        return false;
                    }
                }
                _actPhase++;
                KillAllLaser();
                _toActivate1.SetActive(false);
                _toActivate2.SetActive(true);
                foreach (LaserSpawner ls in _phase2)
                {
                    StartCoroutine(ls.SpawnLaserTimer());
                }
                print("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
                return true;
            case 1:
                foreach (ButtonBox b in _buttonPhase2)
                {
                    if (b.IsOn == false)
                    {
                        return false;
                    }
                }
                _actPhase++;
                KillAllLaser();
                _toActivate2.SetActive(false);
                _toActivate3.SetActive(true);
                return true;
            case 2:
                foreach (ButtonBox b in _buttonPhase3)
                {
                    if (b.IsOn == false)
                    {
                        return false;
                    }
                }
                _actPhase++;
                KillAllLaser();
                _toActivate3.SetActive(false);
                _toActivate4.SetActive(true);
                return true;
            case 3:
                _toActivate4.SetActive(false);
                KillAllLaser();
                return true;
        }
        return false;
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

}
