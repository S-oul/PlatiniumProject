using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    [SerializeField] GameObject _laser;

    [SerializeField] Transform _Pos1;
    [SerializeField] Transform _Pos2;

    [SerializeField] int _numberOfLaser = 4;
    [SerializeField] float _timeForSpawn = 2f;
    
    LaserRoom _laserRoom;

    private void Start()
    {
        OnEnable();
    }
    private void OnEnable()
    {
        _laserRoom = FindObjectOfType<LaserRoom>();
    }

    private void SpawnLaser()
    {
        GameObject g = Instantiate(_laser);
        g.transform.parent = null;
        g.transform.position = _Pos1.position;
        g.transform.localScale = new Vector3(.16f, .16f, .16f);
        Laser l = g.GetComponent<Laser>();
        l.ToFar = _Pos2;
        l.Spawn = _Pos1;
        //_laserRoom.LaserGO.Add(g);

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
