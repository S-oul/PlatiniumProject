using System.Collections;
using UnityEngine;

public class BallVolleyInterPlayer : MonoBehaviour
{
    Rigidbody2D _rb;

    float _numberOfTouches;

    [SerializeField] float _force;

    VolleyballTwoVsTwo _match;

    GameObject _lastCollisionPlayer;

    bool _canCheckCollision;

    bool _isFirstShot;

    bool _canCheckTouchAgain;

    public VolleyballTwoVsTwo Match { get => _match; set => _match = value; }

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
        if (_canCheckCollision)
        {
            if (collision.gameObject.tag == "Player")
            {
                GameManager.Instance.LastPlayerToFail = collision.gameObject;
                _lastCollisionPlayer = collision.gameObject;
                _match.PlayerTouch = collision.gameObject;
                AudioManager.Instance.PlaySFXOS("PlayerHitVolleyBall", collision.gameObject.GetComponent<AudioSource>());
                Vector3 _dir = _match.PointVolley.position;
                _rb.AddForce(_dir.normalized * 20f);


            }



            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                CheckBallPosition();
            }
        }

    }

    void CheckBallPosition()
    {

        _canCheckCollision = false;

        if (gameObject.transform.localPosition.x < 0)
        {
            StartCoroutine(TimerBeforeDestroy(false));
            _lastCollisionPlayer = _match.Players[Random.Range(0, _match.Players.Count)];
        }
        else if (gameObject.transform.localPosition.x > 0)
        {
            StartCoroutine(TimerBeforeDestroy(true));
        }



    }

    

    public IEnumerator TimerBeforeDestroy(bool isForPlayer)
    {
        _match.CheckPoints(isForPlayer);
        string text = "";
        if (isForPlayer)
        {
            _match.TextVolleyUI.gameObject.SetActive(true);
            text = "Point for red team!";
            
           
        }
        else
        {
            _match.TextVolleyUI.gameObject.SetActive(true);
            text = "Point for blue team!";

        }
        StartCoroutine(_match.TextAnimation(text));
        foreach (GameObject player in _match.Players)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider, true);
        }
        yield return new WaitForSeconds(2f);
        foreach (GameObject player in _match.Players)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider, false);
        }
        _match.Point();
        Destroy(gameObject);
    }

    public IEnumerator TimerBeforeCheckAgain()
    {
        _canCheckTouchAgain = false;
        yield return new WaitForSeconds(0.1f);
        _canCheckTouchAgain = true;
    }

}
