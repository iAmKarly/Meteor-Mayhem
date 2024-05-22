using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPExample : MonoBehaviour
{
    Thread recieveThread;
    UdpClient client;
    public int port = 5052;
    public bool startRecieving = true;
    public bool printToConsole = false;
    public string data;

    public void Start()
    {
        recieveThread = new Thread(new ThreadStart(ReceiveData));
        recieveThread.IsBackground = true;
        recieveThread.Start();
    }

    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (startRecieving)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = client.Receive(ref anyIP);
                data = Encoding.UTF8.GetString(dataByte);

                if (printToConsole)
                {
                    print(data);
                }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }
}

