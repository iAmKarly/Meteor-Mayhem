using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerCounter : MonoBehaviour
{
    // Start is called before the first frame update
    public UDPReceive udpReceive;

    // Initialize finger numbers
    public int finger0;
    public int finger1;
    void Start()
    {
        finger0 = 0;
        finger1 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Extract data
        string data = udpReceive.data;
        if (data.Length > 10)
        {
            // Pre process data
            data = data.Remove(0,1);
            data = data.Remove(data.Length-1,1);
            string[] udpData = data.Split(',');

            // Extract finger up counts
            finger0 = int.Parse(udpData[63]);
            if(udpData.Length > 64)
            {
                finger1 = int.Parse(udpData[127]);
            }
            else   
            {
                finger1 = 0;
            }
        }
        else
        {
            finger1 = 0;
            finger1 = 0;
        }
    }
}
