using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPClientTest : MonoBehaviour
{
    [SerializeField] private int portNum = 9999;

    UdpClient udp;
    Thread thread;
    bool running = true;

    void Start()
    {
        udp = new UdpClient(portNum);
        thread = new Thread(ReceiveData); 
        thread.IsBackground = true;
        thread.Start();
        Debug.Log("UDP listening on port " + portNum);
    }

    void ReceiveData()
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
        while (running)
        {
            try
            {
                byte[] data = udp.Receive(ref ep);
                string msg = Encoding.ASCII.GetString(data);
                Debug.Log($"Got UDP from {ep.Address}: {msg}");
            }
            catch (SocketException e)
            {
                Debug.LogWarning("Socket exception: " + e.Message);
            }
        }
    }

    void OnApplicationQuit()
    {
        running = false;
        udp?.Close();
        thread?.Abort();
    }
}
