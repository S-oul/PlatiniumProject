using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private Transform _toFar;
    [SerializeField] private bool _goLeft = true;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _timeToSwap = 1;
    [SerializeField] private float _timePlayerIsDown = 2;

    [SerializeField] SpriteRenderer _sprite;
    BoxCollider2D _boxCollider;

    public float TimePlayerIsDown { get => _timePlayerIsDown; set => _timePlayerIsDown = value; }
    public Transform ToFar { get => _toFar; set => _toFar = value; }
    public bool GoLeft { get => _goLeft; set => _goLeft = value; }

    private void OnEnable()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(SwapperOn());
    }

    void Update()
    {
/*        _sprite.SetPosition(0,transform.position);
        _sprite.SetPosition(1, transform.position - new Vector3(0,5,1));*/

        //print(transform.localPosition.x + " //// " + ToFar.position.x);
        if (_goLeft)
        {
            transform.localPosition += Vector3.left * Time.deltaTime * _speed;
            if (transform.localPosition.x < _toFar.position.x)
            {
                StopAllCoroutines();
                Destroy(gameObject);
            }
        }
        else
        {
            transform.localPosition += Vector3.right * Time.deltaTime * _speed;
            if (transform.localPosition.x > _toFar.position.x)
            {
                StopAllCoroutines();
                Destroy(gameObject);
            }
        }
    }

    IEnumerator SwapperOn()
    {
        yield return new WaitForSeconds(_timeToSwap);
        _sprite.enabled = true;
        _boxCollider.enabled = true;
        StartCoroutine(SwapperOff());
    }
    IEnumerator SwapperOff()
    {
        yield return new WaitForSeconds(_timeToSwap/2);
        _sprite.enabled = false;
        _boxCollider.enabled = false;
        StartCoroutine(SwapperOn());

    }
}
