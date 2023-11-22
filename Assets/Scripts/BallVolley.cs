using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

public class BallVolley : MonoBehaviour
{
    Rigidbody2D _rb;

    float _numberOfTouches;

    [SerializeField] float _force;

    VolleyballTask _task;

    GameObject _lastCollisionObject;

    bool _canCheckCollision;

    bool _isFirstShot;

    bool _canCheckTouchAgain;

    public VolleyballTask Task { get => _task; set => _task = value; }

    private void Start()
    {
        _canCheckCollision = true;
        _canCheckTouchAgain = true;
        _isFirstShot = true;
        _numberOfTouches = 0;
        _rb = GetComponent<Rigidbody2D>();
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(_canCheckCollision)
        {
            if (collision.gameObject.tag == "Player")
            {

                _lastCollisionObject = collision.gameObject;
                _task.PlayerTouch = collision.gameObject;
                /*if (_canCheckTouchAgain)
                {
                    StartCoroutine(TimerBeforeCheckAgain());
                    _numberOfTouches++;

                }*/
                Debug.Log(_numberOfTouches);
                _task.ChangeColorPlayers();
                CheckTouches();
                //Vector3 _dir = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
                Vector3 _dir = _task.PointVolley.position;

                //_rb.velocity = Vector2.zero;
                //Vector3 _dir = collision.gameObject.transform.position - gameObject.transform.position;
                /*_rb.AddForce(Vector2.up * 40f);*/
                //_rb.AddForce(_dir.normalized * _numberOfTouches * 2f);
                _rb.AddForce(_dir.normalized * 20f);

            }
            

            
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                CheckBallPosition();
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _task.Squid)
        {
            if (_isFirstShot)
            {
                _isFirstShot = false;
                Vector3 dir = new Vector3(Random.Range(-0.8f, -1f), Random.Range(0.8f, 1f), 0).normalized * _force;
                
                _rb.velocity = Vector2.zero;
                print(dir);
                _rb.AddForce(dir);
            }
            else
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
    }
    void CheckTouches()
    {
        if (_numberOfTouches == 4)
        {
            _canCheckCollision = false;
            
            StartCoroutine(TimerBeforeDestroy(false));
        }
        
        
    }
    
    void CheckBallPosition()
    {
        
        _canCheckCollision = false;
        
        if (gameObject.transform.localPosition.x < 0)
        {
            StartCoroutine(TimerBeforeDestroy(false));
        }
        else if (gameObject.transform.localPosition.x > 0)
        {
            StartCoroutine(TimerBeforeDestroy(true));
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

    public IEnumerator TimerBeforeDestroy(bool isForPlayer)
    {
        if (isForPlayer)
        {
            _task.TextVolleyUI.gameObject.SetActive(true);
            _task.TextVolleyUI.text = "Point for players!";
        }
        else
        {
            _task.TextVolleyUI.gameObject.SetActive(true);
            _task.TextVolleyUI.text = "Point for squid!";

        }
        StartCoroutine(_task.TextAnimation());
        foreach (GameObject player in _task.PlayersDoingTask)
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
        _task.Point(isForPlayer);
        Destroy(gameObject);
    }

    public IEnumerator TimerBeforeCheckAgain()
    {
        _canCheckTouchAgain = false;
        yield return new WaitForSeconds(0.1f);
        _canCheckTouchAgain = true;
    }

}
