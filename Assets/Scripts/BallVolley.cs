using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallVolley : MonoBehaviour
{
    Rigidbody2D _rb;

    float _numberOfTouches;

    [SerializeField] float _force;

    VolleyballTask _task;

    public VolleyballTask Task { get => _task; set => _task = value; }

    private void Start()
    {
        _numberOfTouches = 0;
        _rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == _task.Squid)
        {
            Vector3 dir = new Vector3(Random.Range(-0.5f, -1f), Random.Range(0.8f, 1f), 0).normalized * _force;
            _rb.velocity = Vector2.zero;
            _rb.AddForce(dir);
        }
        if (collision.gameObject.tag == "Player")
        {
            CheckTouches();
            _numberOfTouches++;
            Vector3 _dir = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            //Vector3 _dir = collision.gameObject.transform.position - gameObject.transform.position;
            _rb.AddForce(_dir.normalized * 20f * _numberOfTouches);
            _rb.AddForce(Vector2.up * 30);
        }
        if(collision.gameObject == _task.Net)
        {
            _task.Fault(this.gameObject);
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("Rebond");
        }
    }

    void CheckTouches()
    {
        if(_numberOfTouches == 3)
        {
            _task.Fault(this.gameObject);
        }
    }
}
