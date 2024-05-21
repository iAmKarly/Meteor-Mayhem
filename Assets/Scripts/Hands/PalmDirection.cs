using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmDirection : MonoBehaviour
{
    LineRenderer lineRenderer;
    public HandTracker handTracker;
    public int handNum = 0;
    public Transform firepoint;
    public float extensionLength = 5.0f;
    public Vector3 destination;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 destination = new Vector3();
        if(handNum == 0)
        {
            calcPath(handTracker.perpendicularDirection0);
        }
        else
        {
            calcPath(handTracker.perpendicularDirection1);
        }
        
        // Set the line origin and destination
        lineRenderer.SetPosition(0, firepoint.position);
        lineRenderer.SetPosition(1, destination);
    }
    void calcPath(Vector3 perpendicularDirection)
    {
        // calculate path of aim
        Ray ray = new Ray(firepoint.position, perpendicularDirection);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            destination = hit.point;
        }
        else
        {
            destination = ray.GetPoint(1000);
        }
    }
}
