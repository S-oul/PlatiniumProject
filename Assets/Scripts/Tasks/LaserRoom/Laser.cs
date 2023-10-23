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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
        if(hit.collider.tag == "Player")
        {
            hit.collider.GetComponent<PlayerController>().DisableMovement();
        }

        if (_goLeft)
        {
            transform.position += Vector3.left * Time.deltaTime * _speed;
            if(transform.position.x > ToFar.position.x)
            {
                Destroy(this);
            }
        }
        else
        {
            transform.position += Vector3.right * Time.deltaTime * _speed;
            if (transform.position.x < ToFar.position.x)
            {
                Destroy(this);
            }
        }
    }

}
