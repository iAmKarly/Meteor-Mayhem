using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTracker : MonoBehaviour
{
    // Start is called before the first frame update
    public UDPReceive udpReceive;
    public GameObject[] handPoints;
    public Vector3 perpendicularDirection0;
    public Vector3 perpendicularDirection1;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Extract data
        string data = udpReceive.data;
        
        // Check for hand
        if (data.Length > 50)
        {
            // Pre process data
            data = data.Remove(0,1);
            data = data.Remove(data.Length-1,1);
            string[] points = data.Split(',');

            // Extract first hand landmarks
            inputHandLM1(points);
            
            // Check for second hand
            if(points.Length > 64)
            {
                // Extract second hand landmarks
                inputHandLM2(points);
            }
            else   
            {
                // Reset second hand landmarks
                resetHandLM2();
                
            }
            perpDir2();
        }
        else   
        {
            // Reset first hand landmarks
            resetHandLM1();
        }
        perpDir1();
    }
    void inputHandLM1(string[] points)
    {
        for(int i=0; i<21; i++)
        {
            float x = float.Parse(points[i*3])/100;
            float y = float.Parse(points[i*3 + 1])/100;
            float z = float.Parse(points[i*3 + 2])/100;

            // Assign landmarks to ingame points
            handPoints[i].transform.localPosition = new Vector3(x,y,z);
        }
    }
    void inputHandLM2(string[] points)
    {
        for(int i=21; i<42; i++)
        {
            float x = float.Parse(points[i*3 + 1])/100;
            float y = float.Parse(points[i*3 + 2])/100;
            float z = float.Parse(points[i*3 + 3])/100;

            // Assign landmarks to ingame points
            handPoints[i].transform.localPosition = new Vector3(x,y,z);
        }
    }
    void resetHandLM1()
    {
        for(int i=0; i<21; i++)
            {
                float x = 0;
                float y = 0;
                float z = 0;

                handPoints[i].transform.localPosition = new Vector3(x,y,z);
            }
    }
    void resetHandLM2()
    {
        for(int i=21; i<42; i++)
            {
                float x = 0;
                float y = 0;
                float z = 0;

                handPoints[i].transform.localPosition = new Vector3(x,y,z);
            }
    }
    void perpDir1()
    {
        // Extract Vector from coordinates
        Vector3 pointA = handPoints[0].transform.localPosition;
        Vector3 pointB = handPoints[5].transform.localPosition;
        Vector3 pointC = handPoints[17].transform.localPosition;

        // Calculate the direction from the origin to the destination
        Vector3 directionAB = (pointB - pointA).normalized;
        Vector3 directionBC = (pointC - pointA).normalized;

        // Calculate a perpendicular direction
        perpendicularDirection0 = Vector3.Cross(directionAB, directionBC).normalized;
        if (perpendicularDirection0.z > 0)
        {
            perpendicularDirection0 = -perpendicularDirection0;
        }
    }
    void perpDir2()
    {
        // Extract Vector from coordinates
        Vector3 pointA = handPoints[21].transform.localPosition;
        Vector3 pointB = handPoints[26].transform.localPosition;
        Vector3 pointC = handPoints[38].transform.localPosition;

        // Calculate the direction from the origin to the destination
        Vector3 directionAB = (pointB - pointA).normalized;
        Vector3 directionBC = (pointC - pointA).normalized;

        // Calculate a perpendicular direction
        perpendicularDirection1 = Vector3.Cross(directionAB, directionBC).normalized;
        if (perpendicularDirection1.z > 0)
        {
            perpendicularDirection1 = -perpendicularDirection1;
        }
    }
}
