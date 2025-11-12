using System.IO.Ports;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class MicrocontrollerManager : MonoBehaviour
{
    public bool test;
    public string message;
    public bool parse;
    public SerialPort SerialPort { get; private set; }

    public bool SerialPortIsOpen { get; private set; }

    [SerializeField] private int baudrate = 9600;
    [SerializeField] private float portSwitchInterval = 250; // ms
    private SerialPortFinder portFinder;
    private BreathingDeviceData dataContainer;
    private Thread microControllerThread;

    public void Awake()
    {
        dataContainer = new BreathingDeviceData();
    }

    private void Update()
    {
        //Debug.Log("Airvelocity : " + dataContainer.AirVelocity);

        if (parse)
        {
            MessageParser.ParseMessage(message, dataContainer);
            parse = false;
        }

        if (test)
        {
            TryToFindNewPort();
            test = false;
        }
    }


    public void TryToFindNewPort()
    {
        if (SerialPort != null)
        {
            portFinder.OnDisable();
        }

        portFinder = new SerialPortFinder(baudrate, portSwitchInterval);
        portFinder.OnSerialPortFound += OnSerialPortFounded;

    }
    private void OnSerialPortFounded(SerialPort serialPort)
    {
        SerialPort = serialPort;
        SerialPortIsOpen = true;

        if (microControllerThread.IsAlive)
        {
            microControllerThread.Abort();
        }
        microControllerThread = new Thread(HandleMicrocontrollerInput);
    }

    private void HandleMicrocontrollerInput()
    {
        string message = SerialPort.ReadLine();

        //if (message == "") return;
        Debug.Log("message : " + message);
    }

    private void OnDisable()
    {
        if (microControllerThread != null && microControllerThread.IsAlive)
            microControllerThread.Abort();

        if(portFinder != null)
        {
            portFinder.OnDisable();
        }
    }

}

