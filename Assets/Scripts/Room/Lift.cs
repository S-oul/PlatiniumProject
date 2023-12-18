using System.Collections;
using System.Collections.Generic;
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
    public bool CanTeleport { get => _canTeleport; set => _canTeleport = value; }

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

        List<GameObject> players = new List<GameObject>();
        
        if (CanTeleport)
        {
            foreach(GameObject p in ListPlayer)
            {
                //_player = player;
                p.GetComponent<PlayerController>().DisableMovements();
                _animator.SetTrigger("Lift");
                AudioSource.pitch = 6f;
                AudioManager.Instance.PlaySFXOS("ElevatorDoorClose", AudioSource);
                p.transform.Find("Animation").GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                //p.transform.Find("Animation").GetComponent<SpriteRenderer>().sortingOrder = 1;
                players.Add(p);
            }
            StartCoroutine(TimeToTeleport(_timeToTeleport, players));
        }
    }

    IEnumerator TimeToTeleport(float time, List<GameObject> players)
    {

        yield return new WaitForSeconds(time);
        foreach(GameObject p in players)
        {
            p.transform.position = TeleportPos.position;
            p.GetComponent<PlayerController>().EnableMovementInteractDisableInputs();
            p.transform.Find("Animation").GetComponent<SpriteRenderer>().sortingLayerName = "Player";
            //p.transform.Find("Animation").GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
            AudioSource.pitch = 1f;
            AudioManager.Instance.PlaySFXOS("ElevatorDing", TeleportPos.gameObject.GetComponent<AudioSource>());
        //_player = null;
        StartCoroutine(TeleportCooldown(_teleporterCooldown));
    }
    IEnumerator TeleportCooldown(float time)
    {
        CanTeleport = false;
        yield return new WaitForSeconds(time);
        CanTeleport = true;
    }

}
