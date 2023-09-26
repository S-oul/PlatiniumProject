using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarrionScript : MonoBehaviour
{

    public float speed = 5;
    public float distRaycast = 5;
    public int nbdeLiane = 6;

    public Vector2 velocity = Vector2.zero;

    public List<RaycastHit2D> hit = new List<RaycastHit2D>();



    void Start()
    {
        ConnectAll();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        }

        for (int i = 0; i < nbdeLiane; i++)
        {
            Check(hit[i]);
        }

    }

    void ConnectAll()
    {
        for (int i = 0; i < nbdeLiane; i++)
        {
            float randAngle = Random.Range(0, 361);
            Vector2 randDir = new Vector2(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * distRaycast;
            RaycastHit2D hitr = Physics2D.Raycast((Vector2)transform.position + velocity, randDir, distRaycast);
            if (hitr)
            {
                hit.Add(hitr);
            }
            else
            {
                hit.Add(Physics2D.Raycast(transform.position, Vector2.zero));
            }
        }
        for (int i = 0; i < hit.Count; i++)
        {
            Debug.Log(hit[i].point);
        }
    }

    void ReconnectOne(RaycastHit2D hitToReco)
    {
        hit.Remove(hitToReco);
        float randAngle = Random.Range(0, 361);
        Vector2 randDir = new Vector2(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * distRaycast;
        hitToReco = Physics2D.Raycast((Vector2)transform.position + velocity, randDir, distRaycast);
        hit.Add(hitToReco);


    }
    void Check(RaycastHit2D hit)
    {
        if (hit)
        {
            if (Vector2.Distance(hit.point, transform.position) > distRaycast)
            {
                ReconnectOne(hit);
            }
        }
        else
        {
            ReconnectOne(hit);
        }

    }
    private void OnDrawGizmos()
    {
        Handles.color = new Color(255, 0, 0, 1);
        UnityEditor.Handles.DrawWireDisc((Vector2)transform.position + velocity, Vector3.back, distRaycast);

        Gizmos.color = Color.red;
        for (int i = 0; i < hit.Count; i++)
        {
            if (hit[i] == false)
            {
                break;
            }
            Gizmos.DrawCube(hit[i].point, new Vector2(.3f, .3f));
        }
    }
}
