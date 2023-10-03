using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;  

public class CarrionScript : MonoBehaviour
{

    public float speed = 5;
    public float distRaycast = 5;
    public float breakDist = 5;

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

        hit.Clear();
        hit = new List<RaycastHit2D>();

        RaycastHit2D hitr = Physics2D.Raycast(transform.position, new Vector2(0, 1), distRaycast);
        if (hitr) hit.Add(hitr);
        hitr = Physics2D.Raycast(transform.position, new Vector2(-1, 1), distRaycast);
        if (hitr) hit.Add(hitr);
        hitr = Physics2D.Raycast(transform.position, new Vector2(1, 1)  , distRaycast);
        if (hitr) hit.Add(hitr);
        hitr = Physics2D.Raycast(transform.position, new Vector2(0, -1) , distRaycast);
        if (hitr) hit.Add(hitr);
        hitr = Physics2D.Raycast(transform.position, new Vector2(-1, -1), distRaycast);
        if (hitr) hit.Add(hitr);
        hitr = Physics2D.Raycast(transform.position, new Vector2(1, -1) , distRaycast);
        if (hitr) hit.Add(hitr);
    }
    /*    void Check()
        {
            foreach(RaycastHit2D h in hit)
            {
                if(h == false)
                {
                    break;
                }
                if (Vector2.Distance(h.transform.position, transform.position) > MaxDist)
                {
                    hit.Remove(h);
                    ConnectOne();
                }
            }
        }*/
    void ConnectOne()
    {
/*        int Angle = Random.Range(0, 361);
        Vector2 polaire = new Vector2(Mathf.Cos(Angle), Mathf.Sin(Angle)) * distRaycast;
        hit.Add(Physics2D.Raycast(transform.position, polaire));
        if (hit[hit.Count - 1])
        {
            print("Hit");
        }
        else
        {
            //ConnectOne();
        }*/
    }
    void ConnectAll()
    {
       /* hit.Clear();

        for (int i = 0; i < nbdeLiane; i++)
        {
            //hit.Add(Physics2D.Raycast(transform.position, Vector2.zero));
            int Angle = Random.Range(0, 361);
            Vector2 polaire = new Vector2(Mathf.Cos(Angle), Mathf.Sin(Angle)) * distRaycast;
            hit.Add(Physics2D.Raycast(transform.position, polaire));
            if (hit[i])
            {
                print("Hit");
            }           
        }*/
    }
    private void OnDrawGizmos()
    {
        Handles.color = new Color(255, 0, 0, 1f);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, breakDist);

        Handles.color = new Color(0, 255, 0, 1f);
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
