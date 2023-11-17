using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixTask : Task
{
    List<Transform> _posPlayers;
    List<Transform> _posProj;
    Transform _posProjL;
    Transform _posProjR;

    [SerializeField] GameObject _projectilePrefab;
    public override void Init()
    {
        base.Init();
        foreach (Transform pos in RoomTask.transform.Find("PlayerPositions"))
        {
            _posPlayers.Add(pos);
        }
        foreach (GameObject player in PlayersDoingTask)
        {
            Transform newPos = _posPlayers[Random.Range(0, _posPlayers.Count)];
            _posPlayers.Remove(newPos);
            player.transform.position = newPos.position;
            //player.GetComponent<PlayerController>().ChangeMobiltyFactor(1.5f, 2);
        }
    }

    public override void End(bool isSuccessful)
    {
        base.End(isSuccessful);
    }

    void SpawnProjectile()
    {
        Transform randPos = _posProj[Random.Range(0, _posProj.Count)];
        
        GameObject proj = Instantiate(_projectilePrefab, _posProjL.position, Quaternion.identity);
        
        Vector3 dir = (randPos.localPosition - Vector3.zero).normalized;
    }
}
