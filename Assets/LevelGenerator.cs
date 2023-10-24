using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] List<DecryptageEnemySt> enemyList;
    public GameObject enemy;
    public Vector2 spawnLocation;

    public void RunByInspector()
    {
        enemyList.Add(new DecryptageEnemySt(1, 0f, enemy));
        Instantiate(enemyList[-1].enemyObject);
    }

/*
    Let GD add new obstacle on any layer
    Let GD adjust the obstacl's 
    Let GD adjust speed of specific obstacle
    Let GD save obstacle preset 
*/

}

public struct DecryptageEnemySt
{
    public int LayerGroup;
    public float rotationLocation; 
    public GameObject enemyObject;

    public DecryptageEnemySt (int layerGroup, float rotationLocation, GameObject enemyObject)
    {
        this.LayerGroup = layerGroup;
        this.rotationLocation = rotationLocation;
        this.enemyObject = enemyObject;
    }
}
