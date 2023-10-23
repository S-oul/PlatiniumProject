using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    private void SpawnLaser(GameObject go)
    {
        GameObject g = Instantiate(go, transform);
    }
}
