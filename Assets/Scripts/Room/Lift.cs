using System.Collections;
using UnityEngine;

public class Lift : Room
{
    [SerializeField] private Transform _myPos;
    
    [SerializeField] private Transform _teleportPos;

    [SerializeField] float _timeToTeleport = .5f;
    [SerializeField] float _teleporterCooldown = 1f;
    [SerializeField] Animator _animator;
    private bool _canTeleport = true;



    public Transform MyPos { get => _myPos; }
    public Transform TeleportPos { get => _teleportPos; set => _teleportPos = value; }

    private GameObject _player;
    private void Awake()
    {
        _myPos = transform;
    }
    public void AutoLiftInteract(GameObject player)
    {
        print(player + " " + TeleportPos);
        player.transform.position = TeleportPos.position;
    }
    public void InteractLift(GameObject player)
    {
        if (_canTeleport)
        {
            _player = player;
            player.GetComponent<PlayerController>().DisableMovements();
            _animator.SetTrigger("Lift");
            _player.transform.Find("Animation").GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            _player.transform.Find("Animation").GetComponent<SpriteRenderer>().sortingOrder = 1;
            StartCoroutine(TimeToTeleport(_timeToTeleport));
        }
    }

    IEnumerator TimeToTeleport(float time)
    {
        yield return new WaitForSeconds(time);
        if(_player != null)
        {

            _player.transform.position = TeleportPos.position;
            _player.GetComponent<PlayerController>().EnableMovementDisableInputs();
            _player.transform.Find("Animation").GetComponent<SpriteRenderer>().sortingLayerName = "Player";
            _player.transform.Find("Animation").GetComponent<SpriteRenderer>().sortingOrder = 0;


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
