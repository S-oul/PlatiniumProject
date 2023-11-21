using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class VolleyballTask : Task
{
    List<Transform> _posPlayerList = new List<Transform>();

    Transform _spawnBallPos;

    [SerializeField] float _timeBeforeStart;

    [SerializeField] GameObject _ballPrefab;

    [SerializeField] [Range(0, 100)] int _squidChanceToHit = 100;

    [SerializeField] int _pointsToWin;

    GameObject _net;

    GameObject _squid;

    GameObject _playerTouch;

    Cam _cam;



    int _playersPoints = 0;
    int _squidPoints = 0;

    public GameObject Net { get => _net; set => _net = value; }
    public GameObject Squid { get => _squid; set => _squid = value; }
    public int SquidChanceToHit { get => _squidChanceToHit; set => _squidChanceToHit = value; }
    public GameObject PlayerTouch { get => _playerTouch; set => _playerTouch = value; }

    public override void End(bool isSuccessful)
    {

        base.End(isSuccessful);
        foreach (GameObject player in PlayersDoingTask)
        {
            player.GetComponent<PlayerController>().ChangeMobiltyFactor(1, 1);
        }
    }
    public override void Init()
    {

        base.Init();
        PlayerTouch = null;
        foreach (Transform pos in RoomTask.transform.Find("PlayerPositions"))
        {
            _posPlayerList.Add(pos);
        }
        foreach (GameObject player in PlayersDoingTask)
        {
            Transform newPos = _posPlayerList[Random.Range(0, _posPlayerList.Count)];
            _posPlayerList.Remove(newPos);
            player.transform.position = newPos.position;
            player.GetComponent<PlayerController>().ChangeMobiltyFactor(1.5f, 2);
        }
        _spawnBallPos = RoomTask.transform.Find("BallStartPos");
        StartCoroutine(TimerBeforeBall(_timeBeforeStart));
        Squid = RoomTask.transform.Find("Squid").gameObject;
        Net = RoomTask.transform.Find("Net").gameObject;
        _cam = Camera.main.GetComponent<Cam>();
        _cam.FixOnRoomVoid(ThisRoom);
    }

    IEnumerator TimerBeforeBall(float time)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            //Feedback Canvas timer
            yield return null;
        }
        
        SpawnVolleyBall();
    }

    void SpawnVolleyBall()
    {
        GameObject ball = Instantiate(_ballPrefab, _spawnBallPos.position, Quaternion.identity, RoomTask.transform);
        ball.GetComponent<BallVolley>().Task = this;


    }

    public void Point(bool isForPlayer)
    {
      
        if (isForPlayer)
        {
            _playersPoints++;
        }
        else
        {
            _squidPoints++;
        }
        if (_squidPoints < _pointsToWin && _playersPoints < _pointsToWin)
        {
            StartCoroutine(TimerBeforeBall(2f));
        }
        else if (_squidPoints == _pointsToWin)
        {
            Debug.Log("Defeat");
        }
        else if (_playersPoints == _pointsToWin)
        {
            Debug.Log("Win");
        }
    }

    public void ChangeColorPlayers()
    {
        
        foreach (GameObject player in PlayersDoingTask)
        {
            print(player.name + " // " + PlayerTouch.name);
            if (player != null)
            {
                return;
            }
            if(player == PlayerTouch)
            {
                
                player.transform.Find("Animation").GetComponent<SpriteRenderer>().color = new Color(127, 127, 127);
            }
            else
            {
                player.transform.Find("Animation").GetComponent<SpriteRenderer>().color = Color.white;
            }

        }
    }
}