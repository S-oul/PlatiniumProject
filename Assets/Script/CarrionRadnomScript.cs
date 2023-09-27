using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;  

public class CarrionRandomScript : MonoBehaviour
{

    public float speed = 5;
    public float distRaycast = 5;
    public int nbdeLiane = 6;
    [SerializeField] GameObject hook;

    public List<RaycastHit2D> hit = new List<RaycastHit2D>();

    public List<GameObject> hooks = new List<GameObject>();

   
    

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
            GameObject newHook = Instantiate(hook, transform.position, Quaternion.identity);
            
            hooks.Add(newHook);
            if (hitr)
            {
                hit.Add(hitr);
                newHook.SetActive(true);

                
            }
            else
            {
                hit.Add(Physics2D.Raycast(transform.position,Vector2.zero));
                newHook.SetActive(false);
            }
            newHook.GetComponent<Tentacle>().dir = hitr.point;
        }
        
    }

    void ReconnectOne(RaycastHit2D hitToReco)
    {
        GameObject hook = hooks[hit.IndexOf(hitToReco)];
        hook.GetComponent<Tentacle>().dir = transform.position;
        hook.SetActive(false);
        hit.Remove(hitToReco);

        float randAngle = Random.Range(0, 361);
        Vector2 randDir = new Vector2(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * distRaycast;
        hitToReco = Physics2D.Raycast(transform.position, randDir, distRaycast);

        hit.Insert(hooks.IndexOf(hook), hitToReco);
        hook.transform.position = transform.position;
        hook.SetActive(true);
        hook.GetComponent<Tentacle>().dir = hitToReco.point;





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
        Handles.color = new Color(255, 0, 0, .6f);
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
