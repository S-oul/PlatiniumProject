using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goat : Task
{
    [SerializeField] float _goatForce = 1;
    [SerializeField] Transform _goatSpawn;
    public override void Init()
    {
        throw new System.NotImplementedException();
    }
    private void StartTask()
    {
        
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _goatSpawn.position, Time.deltaTime*_goatForce);
    }
}

