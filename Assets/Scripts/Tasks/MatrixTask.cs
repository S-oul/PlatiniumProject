using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class MatrixTask : Task
{
    List<Transform> _posPlayers = new List<Transform>();
    SpriteRenderer _spriteRendererRoom;
    bool _timeIsUp;
    bool _allPlayerAreDead;

    float _timeRemaining;
    [SerializeField] GameObject _projectilePrefab;

    [Header("Game Settings")]
    [SerializeField] float _timeTask;
    [SerializeField] float _timeBetweenProjectiles;
    [SerializeField] float _projectileSpeed;

    Cam _cam;
    public override void Init()
    {
        
        base.Init();
        _cam = Camera.main.GetComponent<Cam>();
        _cam.FixOnRoomVoid(ThisRoom);
        _timeRemaining = _timeTask;
        _spriteRendererRoom = RoomTask.transform.Find("Texture").GetComponent<SpriteRenderer>();
        Debug.Log(RoomTask);
        foreach (Transform pos in RoomTask.transform.Find("PlayerPositions"))
        {
            _posPlayers.Add(pos);
        }
        foreach (GameObject player in PlayersDoingTask)
        {
            Transform newPos = _posPlayers[Random.Range(0, _posPlayers.Count)];
            _posPlayers.Remove(newPos);
            player.transform.position = newPos.position;
            player.GetComponent<PlayerController>().ChangeMobiltyFactor(1.5f, 2);
        }
        StartCoroutine(GameTimer());
        SpawnProjectile();
    }

    

    public override void End(bool isSuccessful)
    {
        base.End(isSuccessful);
        foreach(GameObject player in PlayersDoingTask)
        {
            player.GetComponent<PlayerController>().IsPlayerDown = false;
        }
    }

    void SpawnProjectile()
    {
        Vector3 randomPos = FindPosOnRoom();
        GameObject proj = Instantiate(_projectilePrefab, randomPos, Quaternion.identity);
        GameObject randomPlayer = PlayersDoingTask[Random.Range(0, PlayersDoingTask.Count)];
        Vector3 dir = (randomPlayer.transform.position - randomPos).normalized;
        proj.GetComponent<ProjectileMatrix>().Dir = dir;
        proj.GetComponent<ProjectileMatrix>().SpeedProj = _projectileSpeed;
        proj.GetComponent<ProjectileMatrix>().Task = this;
        if (!_timeIsUp)
        {
            StartCoroutine(ProjectileTimer());
        }
        
    }

    Vector3 FindPosOnRoom()
    {
        float width = _spriteRendererRoom.bounds.size.x;
        float height = _spriteRendererRoom.bounds.size.y;
        
        float xMin = RoomTask.transform.position.x - (width / 2);
        float xMax = RoomTask.transform.position.x + (width / 2);
        float yMin = RoomTask.transform.position.y - (height / 2);
        float yMax = RoomTask.transform.position.y + (height / 2);
        Vector2[] randomPos = { new Vector2(xMin, Random.Range(yMin, yMax)), new Vector2(xMax, Random.Range(yMin, yMax)), new Vector2(Random.Range(xMin, xMax), yMax) };
        return randomPos[Random.Range(0, randomPos.Length)];
    }

    IEnumerator GameTimer()
    {
        _timeRemaining = _timeTask;
        while(!_allPlayerAreDead && _timeRemaining > 0)
        {
            _timeRemaining -= Time.deltaTime;
            yield return null;
        }
        _timeIsUp = true;
        if (!_allPlayerAreDead)
        {
            End(true);
        }
        else
        {
            End(false);
        }
    }
    IEnumerator ProjectileTimer()
    {
        yield return new WaitForSeconds(_timeBetweenProjectiles);

        SpawnProjectile();
    }

    public void PlayerTouched(GameObject playerTouched)
    {
        foreach(GameObject player in PlayersDoingTask)
        {
            if(player == playerTouched)
            {
                player.GetComponent<PlayerController>().PlayerDown(_timeRemaining);
            }
        }
    }

}
