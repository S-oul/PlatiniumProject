using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallVolley : MonoBehaviour
{
    Rigidbody2D _rb;

    [SerializeField] float _force;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "SquidVolley")
        {
            Vector3 dir = new Vector3(Random.Range(-0.5f, -1f), Random.Range(0.5f, 1f), 0).normalized * _force;
            _rb.AddForce(dir);
        }
        if (collision.gameObject.tag == "Player")
        {
            Vector3 dir = new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), 0).normalized * _force;
            _rb.AddForce(dir);
        }
    }
}
