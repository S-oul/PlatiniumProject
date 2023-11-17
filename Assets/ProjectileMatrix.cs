using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMatrix : MonoBehaviour
{
    [SerializeField] float _speedProj;
    Rigidbody2D _rb;
    Vector3 _dir;

    public Vector3 Dir { get => _dir; set => _dir = value; }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = _dir * _speedProj;
    }
}
