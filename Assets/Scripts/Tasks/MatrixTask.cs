using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixTask : Task
{
    List<Transform> _posPlayers = new List<Transform>();
    SpriteRenderer _spriteRendererRoom;
    bool _timeIsUp;
    bool _allPlayerAreDead;
    [SerializeField] List<GameObject> playersAlive = new List<GameObject>();
    float _timeRemaining;
    [SerializeField] GameObject _projectilePrefab;

    [Header("Game Settings")]
    [SerializeField] float _timeTask;
    [SerializeField] float _timeBetweenProjectiles;
    [SerializeField] float _projectileSpeed;
    [SerializeField] float _timeBeforeTask;

    Cam _cam;
    public override void Init()
    {
        
        base.Init();
        playersAlive.Clear();
        foreach (GameObject player in PlayersDoingTask) 
        {
            playersAlive.Add(player);
        }
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
        StartCoroutine(TimerBeforeStart());
        SpawnProjectile();
    }

    

    public override void End(bool isSuccessful)
    {
        foreach(GameObject player in PlayersDoingTask)
        {
            player.GetComponent<PlayerController>().IsPlayerDown = false;
        }
    }

    void SpawnProjectile()
    {
        if(!_timeIsUp)
        {
            Vector3 randomPos = FindPosOnRoom();
            GameObject proj = Instantiate(_projectilePrefab, randomPos, Quaternion.identity);
            GameObject randomPlayer = PlayersDoingTask[Random.Range(0, PlayersDoingTask.Count)];
            Vector3 dir = (randomPlayer.transform.position - randomPos).normalized;
            proj.GetComponent<ProjectileMatrix>().Dir = dir;
            proj.GetComponent<ProjectileMatrix>().SpeedProj = _projectileSpeed;
            proj.GetComponent<ProjectileMatrix>().Task = this;
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

    IEnumerator TimerBeforeStart()
    {
        yield return new WaitForSeconds(_timeBeforeTask);
        StartCoroutine(GameTimer());
    }
    IEnumerator ProjectileTimer()
    {
        yield return new WaitForSeconds(_timeBetweenProjectiles);

        SpawnProjectile();
    }

    public void PlayerTouched(GameObject playerTouched)
    {
        if (playersAlive.Contains(playerTouched))
        {
            playersAlive.Remove(playerTouched);
            StartCoroutine(playerTouched.GetComponent<PlayerController>().PlayerDown(_timeRemaining));
        }
        if(playersAlive.Count == 0)
        {
            _allPlayerAreDead = true;
        }
    }
}
