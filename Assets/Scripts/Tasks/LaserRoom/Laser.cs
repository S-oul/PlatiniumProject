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

    [SerializeField] LineRenderer _line;
    BoxCollider2D _boxCollider;
    AudioSource _audioSource;
    bool _isActive = false;

    public float TimePlayerIsDown { get => _timePlayerIsDown; set => _timePlayerIsDown = value; }
    public Transform ToFar { get => _toFar; set => _toFar = value; }
    public bool GoLeft { get => _goLeft; set => _goLeft = value; }

    private void Start()
    {
    }

    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _line = GetComponent<LineRenderer>();
        _isActive = false;
        StartCoroutine(SwapperOn());
        
    }

    void Update()
    {
        _line.enabled = _isActive;
        if (_isActive)
        {
            RaycastHit2D ray = Physics2D.Raycast(transform.localPosition, Vector2.down, 2f);
            _line.SetPosition(1,ray.point);
        }


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
        _isActive = true;
        AudioManager.instance.PlaySFXLoop(AudioManager.instance.FindClip("LaserConstant"), _audioSource);
        _line.enabled = true;
        _boxCollider.enabled = true;
        StartCoroutine(SwapperOff());
    }
    IEnumerator SwapperOff()
    {
        yield return new WaitForSeconds(_timeToSwap/2);
        AudioManager.instance.StopSource(_audioSource);
        _isActive = false;
        _line.enabled = false;
        _boxCollider.enabled = false;
        StartCoroutine(SwapperOn());

    }
}
