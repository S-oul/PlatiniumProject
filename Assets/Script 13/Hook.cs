using UnityEngine;

public class Hook : MonoBehaviour
{
    public Transform ToConnect; // Start point

    public int segments = 10; // Number of segments in the rope


    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        lineRenderer.positionCount = 5;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, Vector2.Lerp(transform.position, ToConnect.position + Vector3.down * .25f, .25f));
        lineRenderer.SetPosition(2, Vector2.Lerp(transform.position, ToConnect.position + Vector3.down * .75f, .5f));
        lineRenderer.SetPosition(3, Vector2.Lerp(transform.position, ToConnect.position + Vector3.down * .25f, .75f));
        lineRenderer.SetPosition(4, ToConnect.position);

        //lineRenderer.positionCount = segments;
/*       for (int i = 0; i < segments; i++)
        {
            lineRenderer.SetPosition(i, new Vector2(Mathf.Lerp(posA.position.x, posB.position.x, i / segments), Mathf.Lerp(posA.position.y, posB.position.y, i / segments)));
        }*/
    }
}
