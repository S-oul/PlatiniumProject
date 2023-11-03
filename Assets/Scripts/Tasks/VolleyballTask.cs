using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class VolleyballTask : Task
{
    List<Transform> _posPlayerList = new List<Transform>();

    Transform _spawnBallPos;

    [SerializeField] float _timeBeforeStart;

    [SerializeField] GameObject _ball;
    public override void Init()
    {
        base.Init();
        foreach (Transform pos in RoomTask.transform.Find("PlayerPositions"))
        {
            _posPlayerList.Add(pos);
        }
        foreach (GameObject player in PlayersDoingTask)
        {
            Transform newPos = _posPlayerList[Random.Range(0, _posPlayerList.Count)];
            _posPlayerList.Remove(newPos);
            player.transform.position = newPos.position;
        }
        _spawnBallPos = RoomTask.transform.Find("BallStartPos");
        StartCoroutine(TimerBeforeStart(_timeBeforeStart));
    }

    IEnumerator TimerBeforeStart(float time)
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
        Instantiate(_ball, _spawnBallPos.position, Quaternion.identity, RoomTask.transform);
    }
}