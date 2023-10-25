using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Transform ToFar;
    public bool _goLeft = true;
    public float _speed = 5;
    public float _timeToSwap = 1;

    LineRenderer _lineRenderer;
    BoxCollider2D _boxCollider;

    private void OnEnable()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(SwapperOn());
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
                StopAllCoroutines();
                Destroy(gameObject);
            }
        }
        else
        {
            transform.localPosition += Vector3.right * Time.deltaTime * _speed;
            if (transform.localPosition.x > ToFar.position.x)
            {
                StopAllCoroutines();
                Destroy(gameObject);
            }
        }
    }

    IEnumerator SwapperOn()
    {
        yield return new WaitForSeconds(_timeToSwap);
        _lineRenderer.enabled = true;
        _boxCollider.enabled = true;
        StartCoroutine(SwapperOff());
    }
    IEnumerator SwapperOff()
    {
        yield return new WaitForSeconds(_timeToSwap/2);
        _lineRenderer.enabled = false;
        _boxCollider.enabled = false;
        StartCoroutine(SwapperOn());

    }
}
