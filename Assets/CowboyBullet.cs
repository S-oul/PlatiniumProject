using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyBullet : MonoBehaviour
{
    Vector3 _destination;
    Vector3 _originPos;

    float _dist;
    public Vector3 Destination { get => _destination; set => _destination = value;}
    private void Start()
    {
        _originPos = transform.position;
        _dist = Vector3.Distance(_originPos, _destination);
    }
    void Update()
    {
        
    }
}
