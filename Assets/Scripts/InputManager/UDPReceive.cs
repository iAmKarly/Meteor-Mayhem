using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour
{
    private Thread receiveThread;
    private UdpClient client; 
    [SerializeField] private int port = 5052;
    public bool startReceiving = true;
    public bool printToConsole = false;
    public string data;
    public static UDPReceive instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Start()
    {
        Init();
    }

    private void Init()
    {
        if (receiveThread == null)
        {
            receiveThread = new Thread(new ThreadStart(ReceiveData))
            {
                IsBackground = true
            };
            receiveThread.Start();
        }
    }

    private void ReceiveData()
    {
        client = new UdpClient(port);
        IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);

        try
        {
            while (startReceiving)
            {
                try
                {
                    byte[] dataByte = client.Receive(ref anyIP);
                    data = Encoding.UTF8.GetString(dataByte);

                    if (printToConsole)
                    {
                        Debug.Log(data);
                    }
                }
                catch (SocketException sockEx)
                {
                    Debug.LogError($"SocketException: {sockEx.Message}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Exception: {ex}");
                }
            }
        }
        finally
        {
            client.Close();
            Debug.Log("UDP client closed.");
        }
    }

    private void OnApplicationQuit()
    {
        startReceiving = false;
        if (receiveThread != null)
        {
            receiveThread.Join(500); // Wait for the thread to terminate
            receiveThread.Abort();
        }

        if (client != null)
        {
            client.Close();
        }
    }

    private void OnDestroy()
    {
        startReceiving = false;
        if (receiveThread != null)
        {
            receiveThread.Join(500); // Wait for the thread to terminate
            receiveThread.Abort();
        }

        if (client != null)
        {
            client.Close();
        }
    }
}
