using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using MyDirs;

public class LaserSpawner : MonoBehaviour
{
    [SerializeField] GameObject _laser;

    [SerializeField] Transform _Pos1;
    [SerializeField] Transform _Pos2;

    [SerializeField] int _numberOfLaser = 4;
    [SerializeField] float _timeForSpawn = 2f;

    [SerializeField] LaserRoom _laserRoom;

    [SerializeField] private dir _dir;

    private void SpawnLaser()
    {
        GameObject g = Instantiate(_laser);
        g.transform.parent = null;
        g.transform.position = _Pos1.position;
        g.transform.localScale = new Vector3(.16f, .16f, .16f);
        Laser l = g.GetComponent<Laser>();
        l.BossAnim = GameManager.Instance.FinalRoom.GetComponentInChildren<LaserRoom>().BossAnimator;
        l.ToFar = _Pos2;
        l.Spawn = _Pos1;
        l.Dir = _dir;

        l.StartLaser();
        _laserRoom.LaserGO.Add(g);

    }
    public IEnumerator SpawnLaserTimer()
    {
        int count = 0;
        while (count < _numberOfLaser)
        {
            yield return new WaitForSeconds(_timeForSpawn);
            SpawnLaser();
            count++;
        }
    }
}
