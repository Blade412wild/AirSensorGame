using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
public class UDPClientTest : MonoBehaviour
{
    private UdpClient udpClient;
    private Thread receiveThread;
    private bool running = true;

    public int listenPort = 9999;

    void Start()
    {
        udpClient = new UdpClient(listenPort);
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
        Debug.Log("UDP Receiver started on port " + listenPort);
    }

    void ReceiveData()
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, listenPort);

        while (running)
        {
            try
            {
                byte[] data = udpClient.Receive(ref remoteEndPoint);
                string message = Encoding.ASCII.GetString(data);
                Debug.Log("Received: " + message);
            }
            catch { }
        }
    }

    void OnApplicationQuit()
    {
        running = false;
        if (udpClient != null) udpClient.Close();
        if (receiveThread != null) receiveThread.Abort();
    }

}
