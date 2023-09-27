using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public class CarrionRandomScript : MonoBehaviour
{

    public float speed = 5;
    public float distRaycast = 5;
    public int nbdeLiane = 6;

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
            transform.position += new Vector3(speed * Time.deltaTime,0,0);
            //Check();
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            //Check();
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
            RaycastHit2D hitr = Physics2D.Raycast(transform.position, randDir, distRaycast);
            if (hitr)
            {
                hit.Add(hitr);
            }
            else
            {
                hit.Add(Physics2D.Raycast(transform.position,Vector2.zero));
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
        hitToReco = Physics2D.Raycast(transform.position, randDir, distRaycast);
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
        Gizmos.color = new Color(0, 255, 0, .6f);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back ,distRaycast);

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
