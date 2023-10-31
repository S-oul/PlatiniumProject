using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolleyballTask : InputTask
{
    [SerializeField] List<Transform> _positions;
    public override void StartTask()
    {
        foreach(GameObject player in PlayersDoingTask)
        {
            Transform pos = _positions[(int)Random.Range(0, _positions.Count)];
            player.transform.position = pos.position;
            player.GetComponent<PlayerController>().DisableMovementEnableInputs();
            _positions.Remove(pos);
        }
    }
}
