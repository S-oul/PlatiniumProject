using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Cam : MonoBehaviour
{
    public Camera cam;

    public List<GameObject> targets = new List<GameObject>();
    public Vector3 offset;
    public Vector3 velocity;
    public float SmoothTime = .5f;

    public float minZoom = 5;
    public float maxZoom = 20;
    public float zoomLimiter = 50;

    private void LateUpdate()
    {
        if(targets.Count == 0) { return; }
        Move();
        Zoom();
    }
    private void Update()
    {
    }
    private void Zoom()
    {
        float newZoom = Mathf.Lerp(minZoom, maxZoom,GetMaxDist()/zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }
    float GetMaxDist()
    {
        var bounds = new Bounds(targets[0].transform.position, Vector3.zero);
        foreach (GameObject target in targets)
        {
            bounds.Encapsulate(target.transform.position);
        }
        if(bounds.size.x > bounds.size.y)
        {
            return bounds.size.x;
        }
        else
        {
            return bounds.size.y;
        }
    }
    private void Move()
    {
        Vector3 centralPoint = GetCentralPoint();
        Vector3 NewPos = centralPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, NewPos, ref velocity, SmoothTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);

    }

    Vector3 GetCentralPoint()
    {
        if(targets.Count == 1) { return targets[0].transform.position; }

        var bounds = new Bounds(targets[0].transform.position, Vector3.zero);
        foreach (GameObject target in targets)
        {
            bounds.Encapsulate(target.transform.position);
        }
        return bounds.center;
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawLine(cam.ScreenToWorldPoint(new Vector2(cam.scaledPixelWidth / 10,cam.scaledPixelHeight/10)), cam.ScreenToWorldPoint(new Vector2(cam.scaledPixelWidth / 10, cam.scaledPixelHeight - cam.scaledPixelHeight / 10)));
        Handles.DrawLine(cam.ScreenToWorldPoint(new Vector2(cam.scaledPixelWidth / 10,cam.scaledPixelHeight/10)), cam.ScreenToWorldPoint(new Vector2(cam.scaledPixelWidth - cam.scaledPixelWidth / 10,cam.scaledPixelHeight / 10)));

        Handles.DrawLine(cam.ScreenToWorldPoint(new Vector2(cam.scaledPixelWidth - cam.scaledPixelWidth / 10, cam.scaledPixelHeight / 10)), cam.ScreenToWorldPoint(new Vector2(cam.scaledPixelWidth - cam.scaledPixelWidth / 10, cam.scaledPixelHeight - cam.scaledPixelHeight / 10)));
        Handles.DrawLine(cam.ScreenToWorldPoint(new Vector2(cam.scaledPixelWidth / 10, cam.scaledPixelHeight - cam.scaledPixelHeight / 10)), cam.ScreenToWorldPoint(new Vector2(cam.scaledPixelWidth - cam.scaledPixelWidth / 10, cam.scaledPixelHeight - cam.scaledPixelHeight / 10)));
    }
}
