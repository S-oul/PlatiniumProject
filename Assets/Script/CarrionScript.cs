using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public class CarrionScript : MonoBehaviour
{

    public float speed = 5;
    public float distRaycast = 5;
    public int nbdeLiane = 6;
    public float MaxDist = 5;

    public List<RaycastHit2D> hit = new List<RaycastHit2D>(1);

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
            Check();
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            Check();
        }


    }
    void Check()
    {
        foreach(RaycastHit2D h in hit)
        {
            if (Vector2.Distance(h.transform.position, transform.position) > MaxDist)
            {
                hit.Remove(h);
                ConnectOne();
            }
        }
    }
    void ConnectOne()
    {
        int Angle = Random.Range(0, 361);
        Vector2 polaire = new Vector2(Mathf.Cos(Angle), Mathf.Sin(Angle)) * distRaycast;
        hit.Add(Physics2D.Raycast(transform.position, polaire));
        if (hit[hit.Count - 1])
        {
            print("Hit");
        }
        else
        {
            ConnectOne();
        }
    }
    void ConnectAll()
    {
        hit.Clear();
        
        for (int i = 0; i < nbdeLiane; i++)
        {
            hit.Add(Physics2D.Raycast(transform.position, Vector2.zero));
            while (!hit[i])
            {
                int Angle = Random.Range(0, 361);
                Vector2 polaire = new Vector2(Mathf.Cos(Angle), Mathf.Sin(Angle)) * distRaycast;
                hit.Add(Physics2D.Raycast(transform.position, polaire));
                if (hit[i])
                {
                    print("Hit");
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 255, 0, .6f);
        Gizmos.DrawSphere(transform.position, MaxDist);

        Gizmos.color = Color.red;
        for (int i = 0; i < hit.Count; i++)
        {
            Gizmos.DrawCube(hit[i].point, new Vector2(.3f, .3f));
        }
    }
}
