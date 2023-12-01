using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRoom : Task
{
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
    [SerializeField] List<LaserSpawner> _phase3 = new List<LaserSpawner>();
    [SerializeField] List<LaserSpawner> _phase4 = new List<LaserSpawner>();

    int _actPhase = 0;

    Cam _cam;

    [SerializeField] List<Transform> _transform = new List<Transform>();

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
        print(NumberOfPlayers + " " + RoomTask.ListPlayer.Count);
        if(RoomTask.ListPlayer.Count >= NumberOfPlayers)
        {
            foreach (GameObject p in RoomTask.ListPlayer)
            {
                PlayerController pc = p.GetComponent<PlayerController>();
                pc.ChangeMobiltyFactor(1.5f, 2f);
                _players.Add(pc);
            }
            _cam.FixOnRoomVoid(RoomTask);

            _toActivate1.SetActive(true);
            IsStarted = true;
            foreach (LaserSpawner ls in _phase1)
            {
                StartCoroutine(ls.SpawnLaserTimer());
            }
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
                for(int i = 0; i < 4; i++)
                {
                    GameManager.Instance.Players[i].transform.position = _transform[i].position;
                }
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
                for (int i = 0; i < 4; i++)
                {
                    GameManager.Instance.Players[i].transform.position = _transform[i].position;
                }
                foreach (LaserSpawner ls in _phase3)
                {
                    StartCoroutine(ls.SpawnLaserTimer());
                }
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
                for (int i = 0; i < 4; i++)
                {
                    GameManager.Instance.Players[i].transform.position = _transform[i].position;
                }
                foreach (LaserSpawner ls in _phase4)
                {
                    StartCoroutine(ls.SpawnLaserTimer());
                }

                return true;
            case 3:
                foreach (ButtonBox b in _buttonPhase4)
                {
                    if (b.IsOn == false)
                    {
                        return false;
                    }
                }
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
