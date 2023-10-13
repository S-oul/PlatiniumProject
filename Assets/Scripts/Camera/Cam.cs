using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Cam : MonoBehaviour
{
    public Camera cam;

    [SerializeField] private List<GameObject> targets = new List<GameObject>();
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _velocity;
    [SerializeField] private float _SmoothTime = .5f;

    [SerializeField] private float _minZoom = 5;
    [SerializeField] private float _maxZoom = 20;
    [SerializeField] private float _zoomLimiter = 50;
    [SerializeField] private AnimationCurve _zoomCurve;

    [SerializeField] private bool _fixeOnZ = true;

    private void LateUpdate()
    {
        if(targets.Count == 0) { return; }
        Move();
        Zoom();
    }

    private void Zoom()
    {
        float MaxDist = GetMaxDist();
        float newZoom = Mathf.Lerp(_minZoom, _maxZoom, _zoomCurve.Evaluate(MaxDist / _zoomLimiter));
        //print(MaxDist / _zoomLimiter + " ::::::::: " + _zoomCurve.Evaluate(MaxDist / _zoomLimiter));
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
        Vector3 NewPos = centralPoint + _offset;

        transform.position = Vector3.SmoothDamp(transform.position, NewPos, ref _velocity, _SmoothTime);
        if (_fixeOnZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
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
