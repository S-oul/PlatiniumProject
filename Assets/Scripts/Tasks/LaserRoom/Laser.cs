using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Laser : MonoBehaviour
{
    [SerializeField] private Transform _toFar;
    [SerializeField] private Transform _spawn;
    [SerializeField] private bool _goLeft = true;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _timeToSwap = 1;
    [SerializeField] private float _timePlayerIsDown = 2;

    [SerializeField, Range(0f, 1f)] float _position = 0;

    [SerializeField] LineRenderer _line;
    BoxCollider2D _boxCollider;
    AudioSource _audioSource;
    bool _isActive = false;

    RaycastHit2D ray;

    public float TimePlayerIsDown { get => _timePlayerIsDown; set => _timePlayerIsDown = value; }
    public Transform ToFar { get => _toFar; set => _toFar = value; }
    public bool GoLeft { get => _goLeft; set => _goLeft = value; }
    public Transform Spawn { get => _spawn; set => _spawn = value; }

    private void Start()
    {
    }

    private void OnEnable()
    {
        ray = Physics2D.Raycast(transform.position, Vector2.down);
        _audioSource = GetComponent<AudioSource>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _line = GetComponent<LineRenderer>();
        _isActive = false;
        StartCoroutine(SwapperOn());
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, Vector2.down);
    }
    void Update()
    {
        _line.enabled = _isActive;
        if (_isActive)
        {
            ray = Physics2D.Raycast(transform.position, Vector2.down);
            print(ray.transform.name);
            Vector3[] v3s = new Vector3[] {transform.position, ray.point};
            _line.SetPositions(v3s);
        }


        if (_goLeft)
        {
            _position += Time.deltaTime * _speed;
            _position = Mathf.Clamp01(_position);
            transform.localPosition = Vector3.Lerp(_spawn.position,ToFar.position, _position);
            if (_position >= 1)
            {
                _position = 0;
                _goLeft = false;
            }
        }
        else
        {
            _position += Time.deltaTime * _speed;
            _position = Mathf.Clamp01(_position);
            transform.localPosition = Vector3.Lerp(ToFar.position, _spawn.position, _position);
            if (_position >= 1)
            {
                _position = 0;
                _goLeft = true;
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
