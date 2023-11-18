using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMatrix : MonoBehaviour
{
    float _speedProj;
    Rigidbody2D _rb;
    Vector3 _dir;
    MatrixTask _task;
    public Vector3 Dir { get => _dir; set => _dir = value; }
    public float SpeedProj { get => _speedProj; set => _speedProj = value; }
    public MatrixTask Task { get => _task; set => _task = value; }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = _dir * SpeedProj;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _task.PlayerTouched(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
