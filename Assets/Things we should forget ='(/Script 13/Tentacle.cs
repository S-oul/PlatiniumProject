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
    //private List<RopeSection> allRopeSections = new List<RopeSection>();
    private float ropeSectionLength = 0.5f;


    public int vertexCount = 2;

    List<GameObject> nodes = new List<GameObject>();

    bool isDone = false;
    void Start()
    {
        carrion = GameObject.FindGameObjectWithTag("Carrion");
        print(carrion);

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = vertexCount;
        lastNode = transform.gameObject;

        nodes.Add(transform.gameObject);
/*
        Vector3 ropeSectionPos = dir;

        for (int i = 0; i < 15; i++)
        {
            allRopeSections.Add(new RopeSection(ropeSectionPos));

            ropeSectionPos.y -= ropeSectionLength;
        }*/
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
/*
        */RenderLine();/*
        
        
        DisplayRope();
        z
        //Move what is hanging from the rope to the end of the rope
        //allRopeSections[allRopeSections.Count - 1].pos;

        //Make what's hanging from the rope look at the next to last rope position to make it rotate with the rope
        carrion.transform.LookAt(allRopeSections[allRopeSections.Count - 2].pos);*/
    }
/*    private void UpdateRopeSimulation()
    {
        Vector3 gravityVec = new Vector3(0f, -9.81f, 0f);

        float t = Time.fixedDeltaTime;


        //Move the first section to what the rope is hanging from
        RopeSection firstRopeSection = allRopeSections[0];

        firstRopeSection.pos = dir;

        allRopeSections[0] = firstRopeSection;*/


/*        //Move the other rope sections with Verlet integration
        for (int i = 1; i < allRopeSections.Count; i++)
        {
            RopeSection currentRopeSection = allRopeSections[i];

            //Calculate velocity this update
            Vector3 vel = currentRopeSection.pos - currentRopeSection.oldPos;

            //Update the old position with the current position
            currentRopeSection.oldPos = currentRopeSection.pos;

            //Find the new position
            currentRopeSection.pos += vel;

            //Add gravity
            currentRopeSection.pos += gravityVec * t;

            //Add it back to the array
            allRopeSections[i] = currentRopeSection;
        }


        //Make sure the rope sections have the correct lengths
        for (int i = 0; i < 20; i++)
        {
            ImplementMaximumStretch();
        }
    }*/
    void RenderLine()
    {
        lineRenderer.SetPosition(0, dir);
        lineRenderer.SetPosition(1, Vector3.Lerp(dir,carrion.transform.position,.5f));
        lineRenderer.SetPosition(2, carrion.transform.position);

    }

    /*private void ImplementMaximumStretch()
    {
        for (int i = 0; i < allRopeSections.Count - 1; i++)
        {
            RopeSection topSection = allRopeSections[i];

            RopeSection bottomSection = allRopeSections[i + 1];

            //The distance between the sections
            float dist = (topSection.pos - bottomSection.pos).magnitude;

            //What's the stretch/compression
            float distError = Mathf.Abs(dist - ropeSectionLength);

            Vector3 changeDir = Vector3.zero;

            //Compress this sections
            if (dist > ropeSectionLength)
            {
                changeDir = (topSection.pos - bottomSection.pos).normalized;
            }
            //Extend this section
            else if (dist < ropeSectionLength)
            {
                changeDir = (bottomSection.pos - topSection.pos).normalized;
            }
            //Do nothing
            else
            {
                continue;
            }


            Vector3 change = changeDir * distError;

            if (i != 0)
            {
                bottomSection.pos += change * 0.5f;

                allRopeSections[i + 1] = bottomSection;

                topSection.pos -= change * 0.5f;

                allRopeSections[i] = topSection;
            }
            //Because the rope is connected to something
            else
            {
                bottomSection.pos += change;

                allRopeSections[i + 1] = bottomSection;
            }
        }
    }*/

    //Display the rope with a line renderer
/*    private void DisplayRope()
    {
        float ropeWidth = 0.2f;

        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;

        //An array with all rope section positions
        Vector3[] positions = new Vector3[allRopeSections.Count];

        for (int i = 0; i < allRopeSections.Count; i++)
        {
            positions[i] = allRopeSections[i].pos;
        }

        lineRenderer.positionCount = positions.Length;

        lineRenderer.SetPositions(positions);
    }*/

    //A struct that will hold information about each rope section
/*    public struct RopeSection
    {
        public Vector3 pos;
        public Vector3 oldPos;

        //To write RopeSection.zero
        public static readonly RopeSection zero = new RopeSection(Vector3.zero);

        public RopeSection(Vector3 pos)
        {
            this.pos = pos;

            this.oldPos = pos;
        }
    }*/

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
