using UnityEngine;
using MyDirs;

namespace MyDirs
{
    public enum dir
    {
        Down,
        Left,
        Up,
        Right
    }
}
public class Laser : MonoBehaviour
{
    [SerializeField] private Transform _toFar;
    [SerializeField] private Transform _spawn;
    [SerializeField] private bool _goLeft = true;

    [SerializeField] private dir _dir;

    [SerializeField] private float _speed = 5;
    [SerializeField] private float _timeToSwap = 1;
    [SerializeField] private float _timePlayerIsDown = 2;

    [SerializeField, Range(0f, 1f)] float _position = 0;

    [SerializeField] LineRenderer _line;
    [SerializeField] GameObject _impact;
    AudioSource _audioSource;
    bool _isActive = false;

    RaycastHit2D ray;

    public float TimePlayerIsDown { get => _timePlayerIsDown; set => _timePlayerIsDown = value; }
    public bool GoLeft { get => _goLeft; set => _goLeft = value; }
    public Transform ToFar { get => _toFar; set => _toFar = value; }
    public Transform Spawn { get => _spawn; set => _spawn = value; }
    public dir Dir { get => _dir; set => _dir = value; }


    public void StartLaser()
    {
        print(_dir.ToString());
        switch (_dir)
        {
            case dir.Down:
                ray = Physics2D.Raycast(transform.position, Vector2.down);
                _impact.transform.localEulerAngles = new Vector3(0, 0, 180f);
                break;
            case dir.Left:
                ray = Physics2D.Raycast(transform.position, Vector2.left);
                _impact.transform.localEulerAngles = new Vector3(0, 0, 270f);
                break;
            case dir.Right:
                ray = Physics2D.Raycast(transform.position, Vector2.right);
                //_impact.transform.localEulerAngles = new Vector3(0, 0, 90f);
                break;
            case dir.Up:
                ray = Physics2D.Raycast(transform.position, Vector2.up);
                //_impact.transform.localEulerAngles = new Vector3(0, 0, 360f);
                break;

        }
            
        _audioSource = GetComponent<AudioSource>();
        _line = GetComponent<LineRenderer>();

        _isActive = true;
        AudioManager.Instance.PlaySFXLoop(AudioManager.Instance.FindClip("LaserConstant"), _audioSource);
        _line.enabled = true;

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
            switch (_dir)
            {
                case dir.Down:
                    ray = Physics2D.Raycast(transform.position, Vector2.down);
                    break;
                case dir.Left:
                    ray = Physics2D.Raycast(transform.position, Vector2.left);
                    break;
                case dir.Right:
                    ray = Physics2D.Raycast(transform.position, Vector2.right);
                    break;
                case dir.Up:
                    ray = Physics2D.Raycast(transform.position, Vector2.up);
                    break;

            }
            //print(ray.transform.name);
            Vector3[] v3s = new Vector3[] { transform.position, ray.point};
            _impact.transform.position = ray.point;
            _line.SetPositions(v3s);
            if (ray.collider.CompareTag("Player"))
            {
                if(ray.collider.GetComponent<PlayerController>().IsPlayerDown == false)
                {
                    AudioManager.Instance.PlaySFXOS("LaserImpact", _audioSource);
                    StartCoroutine(ray.collider.GetComponent<PlayerController>().PlayerDown(TimePlayerIsDown));
                    GameManager.Instance.DaySlider.RemoveValue(.1f);
                }
            }
        }


        if (_goLeft)
        {
            _position += Time.deltaTime * _speed;
            _position = Mathf.Clamp01(_position);
            transform.localPosition = Vector3.Lerp(_spawn.position, ToFar.position, _position);
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
}

