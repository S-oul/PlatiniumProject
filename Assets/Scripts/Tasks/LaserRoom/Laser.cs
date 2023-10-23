using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Transform ToFar;
    public bool _goLeft = true;
    public float _speed = 5;



    void Update()
    {
        print(transform.localPosition.x + " //// " + ToFar.position.x);
        if (_goLeft)
        {
            transform.localPosition += Vector3.left * Time.deltaTime * _speed;
            if (transform.localPosition.x < ToFar.position.x)
            {
                Debug.Log("AHAAAAAAAAAAAAAAAAA");
                Destroy(gameObject);
            }
        }
        else
        {
            transform.localPosition += Vector3.right * Time.deltaTime * _speed;
            if (transform.localPosition.x > ToFar.position.x)
            {
                Debug.Log("AHAAAAAAAAAAAAAAAAA");
                Destroy(gameObject);
            }
        }
    }
}
