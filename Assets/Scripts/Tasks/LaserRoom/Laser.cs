using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Transform ToFar;
    public bool _goLeft = true;
    public float _speed = 5;

    LineRenderer _lineRenderer;

    private void OnEnable()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        _lineRenderer.SetPosition(0,transform.position);
        _lineRenderer.SetPosition(1, transform.position - new Vector3(0,5,1));

        //print(transform.localPosition.x + " //// " + ToFar.position.x);
        if (_goLeft)
        {
            transform.localPosition += Vector3.left * Time.deltaTime * _speed;
            if (transform.localPosition.x < ToFar.position.x)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            transform.localPosition += Vector3.right * Time.deltaTime * _speed;
            if (transform.localPosition.x > ToFar.position.x)
            {
                Destroy(gameObject);
            }
        }
    }
}
