using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    bool _goLeft = true;
    float _speed = 2;

    void Start()
    {
        
    }


    void Update()
    {
        if (_goLeft)
        {
            transform.position += Vector3.left * Time.deltaTime * _speed;
        }
        else
        {
            transform.position += Vector3.right * Time.deltaTime * _speed;

        }
    }
}
