using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float Force = 5;
    RaycastHit2D m_Hit;

    public GameObject sandbag;
    Vector2 dir = Vector2.zero;
    void Start()
    {
        m_Hit = new RaycastHit2D();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Attack()   
    {
        dir = sandbag.transform.position - transform.position;
        Rigidbody2D rb = sandbag.GetComponent<Rigidbody2D>();
        rb.velocity = dir * Force;
    }
    private void OnDrawGizmos()
    {

        Gizmos.DrawRay(transform.position, dir);

    }
}
