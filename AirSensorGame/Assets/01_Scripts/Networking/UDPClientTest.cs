using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;

public class UDPClientTest : MonoBehaviour
{
    public TMP_InputField PortField;
    public TMP_InputField SendingMessageField;
    public TMP_InputField IncommingMessageField;

    [SerializeField] private int portNum = 9999;

    UdpClient udp;
    Thread thread;
    bool running = true;

    void Start()
    {
        //SetupUDPClient();
        //thread = new Thread(ReceiveData); 
        //thread.IsBackground = true;
        //thread.Start();
    }

    private void Update()
    {
        if (udp == null) return;
        WaitingForIncomingMessage();
    }

    public void SetupUDPClient(int port)
    {
        if (udp != null)
        {
            Debug.Log("Closing port " + portNum);
            udp.Close();
        }

        portNum = port;
        udp = new UdpClient(portNum);
        Debug.Log("Opening On port " + portNum);
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

    private void WaitingForIncomingMessage()
    {
        // receiving
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);

        byte[] data = udp.Receive(ref ep);

        if(data == null)
        {
            Debug.Log("No Data");
            return;
        }

        string msg = Encoding.ASCII.GetString(data);
        Debug.Log($"Got UDP from {ep.Address}: {msg}");

        // update text
        UpdateIncommingMessageField(msg);
    }
    public void SendData(string text)
    {        
        //sending
        udp.Connect("127.0.0.1", portNum);
        byte[] sendBytes = Encoding.ASCII.GetBytes(text);
        udp.Send(sendBytes, sendBytes.Length);
    }

    private void UpdateIncommingMessageField(string text)
    {

    }

    void OnApplicationQuit()
    {
        running = false;
        udp?.Close();

        thread?.Abort();
    }
}
