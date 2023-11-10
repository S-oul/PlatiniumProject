using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallVolley : MonoBehaviour
{
    Rigidbody2D _rb;

    float _numberOfTouches;

    [SerializeField] float _force;

    VolleyballTask _task;

    GameObject _lastCollisionObject;

    
    public VolleyballTask Task { get => _task; set => _task = value; }

    private void Start()
    {
        _numberOfTouches = 0;
        _rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            _lastCollisionObject = collision.gameObject;
            
            _numberOfTouches++;
            CheckTouches();
            Vector3 _dir = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            _rb.velocity = Vector3.zero;
            //Vector3 _dir = collision.gameObject.transform.position - gameObject.transform.position;
            _rb.AddForce(_dir.normalized * _numberOfTouches * 2f);
            _rb.AddForce(Vector2.up * 40f);
        }
        if(collision.gameObject == _task.Net)
        {
            CheckBallPosition();


        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            CheckBallPosition();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _task.Squid)
        {
            if (CheckChanceToHit())
            {
                _lastCollisionObject = collision.gameObject;
                Vector3 dir = new Vector3(Random.Range(-0.8f, -1f), Random.Range(0.8f, 1f), 0).normalized * _force;
                _rb.velocity = Vector2.zero;
                _rb.AddForce(dir);
            }
            
        }
    }
    void CheckTouches()
    {
        if(_numberOfTouches == 3)
        {
            _task.Point(gameObject, false);
        }
    }
    
    void CheckBallPosition()
    {
        if (gameObject.transform.localPosition.x < 0)
        {
            _task.Point(gameObject, false);
        }
        else if (gameObject.transform.localPosition.x > 0)
        {
            _task.Point(gameObject, true);
        }
        
    }

    bool CheckChanceToHit()
    {
        int random = Random.Range(0, 100);
        if(random >= _task.SquidChanceToHit)
        {
            return false;
        }
        else { return true; }
    }

    public IEnumerator TimerBeforeDestroy()
    {
        foreach(GameObject player in _task.PlayersDoingTask)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider, true);
        }
        yield return new WaitForSeconds(2f);
        foreach (GameObject player in _task.PlayersDoingTask)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider, false);
        }
        Destroy(gameObject);
    }
}
