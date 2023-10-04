using UnityEngine;

public class Hook : MonoBehaviour
{
    public Transform ToConnect; // Start point

    public int segments = 5; // Number of segments in the rope


    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        lineRenderer.positionCount = segments+1;
/*        for(float i = 0; i <= segments; i++)
        {
            print(i/segments + "  " + i +"/"+ segments);
            lineRenderer.SetPosition((int)i, Vector2.Lerp(new Vector2(transform.position.x, i/segments*Physics2D.gravity.y + transform.position.y), ToConnect.position, i/segments));
        }*/


        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, ToConnect.position);
    }
}
