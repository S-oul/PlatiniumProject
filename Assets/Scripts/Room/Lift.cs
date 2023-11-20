using System.Collections;
using UnityEngine;

public class Lift : Room
{
    [SerializeField] private Transform _myPos;
    
    [SerializeField] private Transform _teleportPos;

    [SerializeField] float _timeToTeleport = .5f;
    [SerializeField] float _teleporterCooldown = 1f;
    private bool _canTeleport = true;


    public Transform MyPos { get => _myPos; }
    public Transform TeleportPos { get => _teleportPos; set => _teleportPos = value; }

    private GameObject _player;
    private void Awake()
    {
        _myPos = transform;
    }

    public void InteractLift(GameObject player)
    {
        if (_canTeleport)
        {
            _player = player;
            StartCoroutine(TimeToTeleport(_timeToTeleport));
        }
    }

    IEnumerator TimeToTeleport(float time)
    {
        yield return new WaitForSeconds(time);
        if(_player != null)
        {
            _player.transform.position = TeleportPos.position;
        }
        _player = null;
        StartCoroutine(TeleportCooldown(_teleporterCooldown));
    }
    IEnumerator TeleportCooldown(float time)
    {
        _canTeleport = false;
        yield return new WaitForSeconds(time);
        _canTeleport = true;
    }

}
