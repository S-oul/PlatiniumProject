using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Tentacle : MonoBehaviour
{
    public Vector2 dir;

    [SerializeField] float speed = 1;

    [SerializeField] float distance = 2;

    [SerializeField] GameObject nodePrefab;

     GameObject carrion;

     GameObject lastNode;

    [SerializeField] LineRenderer lineRenderer;

    int vertexCount = 2;

    List<GameObject> nodes = new List<GameObject>();

    bool isDone = false;
    void Start()
    {
        carrion = GameObject.FindGameObjectWithTag("Carrion");

        lineRenderer = GetComponent<LineRenderer>();

        lastNode = transform.gameObject;

        nodes.Add(transform.gameObject);
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, dir, speed);

        /*if ((Vector2)transform.position != dir)
        {
            if (Vector2.Distance(carrion.transform.position, lastNode.transform.position) > distance)
            {
                CreateNode();
            }
        }
        else if (isDone == false)
        {
            isDone = true;

            lastNode.GetComponent<HingeJoint2D>().connectedBody = carrion.GetComponent<Rigidbody2D>();
        }*/

        RenderLine();
    }

    void RenderLine()
    {
        /*lineRenderer.SetVertexCount(vertexCount);

        int i;
        for (i = 0; i < nodes.Count; i++)
        {
            lineRenderer.SetPosition(i, nodes[i].transform.position);
        }

        lineRenderer.SetPosition(i, carrion.transform.position);*/
        lineRenderer.SetPosition(0, dir);
        lineRenderer.SetPosition(1, carrion.transform.position);

    }

    /*void CreateNode()
    {
        Vector2 posToCreate = carrion.transform.position - lastNode.transform.position;
        posToCreate.Normalize();
        posToCreate *= distance;
        posToCreate += (Vector2)lastNode.transform.position;

        GameObject newNode = (GameObject)Instantiate(nodePrefab, posToCreate, Quaternion.identity);

        newNode.transform.SetParent(transform);

        lastNode.GetComponent<HingeJoint2D>().connectedBody = newNode.GetComponent<Rigidbody2D>();

        lastNode = newNode;

        nodes.Add(lastNode);

        vertexCount++;
    }*/


}
