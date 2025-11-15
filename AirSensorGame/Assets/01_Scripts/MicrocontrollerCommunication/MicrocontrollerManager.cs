using System;
using System.IO.Ports;
using System.Threading;
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
    private Thread microControllerThread;
    private bool tryReadingData = false;

    public void Awake()
    {

    }

    private void Update()
    {
        //Debug.Log("Airvelocity : " + dataContainer.AirVelocity);

        if (parse)
        {
            MessageParser.ParseMessage(message);
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

        if (microControllerThread != null)
        {
            if (microControllerThread.IsAlive)
            {
                microControllerThread.Join();
            }

        }

        Debug.Log("Starthandeling incomming messages");
        OpenMicrocontrollerThread();
    }

    private void HandleMicrocontrollerInput()
    {
        if (!SerialPort.IsOpen)
        {
            SerialPort.ReadTimeout = 10;
            SerialPort.Open();
            Debug.Log("Opened microcontrollerPort");
        }
        else
        {
            Debug.Log(" microcontrollerPort was already opened");
        }

        DateTime TryReadMessage = DateTime.Now;
        DateTime lastTimeMessageRead = DateTime.Now;

        while (tryReadingData)
        {
            try
            {
                TryReadMessage = DateTime.Now;

                string message = SerialPort.ReadLine()?.Trim();

                if (message.Contains("1"))
                {
                    SerialPort.Write("1");
                    continue;
                }

                if (!string.IsNullOrEmpty(message))
                {
                    Debug.Log(message);
                    TimeSpan timeSpan = TryReadMessage - lastTimeMessageRead;
                    lastTimeMessageRead = DateTime.Now;

                    Debug.Log("timeBetween read messages : " + timeSpan.TotalMilliseconds + " ms");
                }
                else
                {
                    Debug.Log("-");
                }


            }
            catch (TimeoutException ex)
            {
                //Debug.Log("caught timeoutExepction");
            }

        }
    }

    private void OpenMicrocontrollerThread()
    {
        CloseMicrocontrollerThread();

        microControllerThread = new Thread(HandleMicrocontrollerInput);
        tryReadingData = true;
        microControllerThread.Start();
    }

    private void CloseMicrocontrollerThread()
    {
        if (microControllerThread != null && microControllerThread.IsAlive)
        {
            tryReadingData = false;
            microControllerThread.Join();
        }

    }

    private void OnDisable()
    {
        CloseMicrocontrollerThread();

        if (portFinder != null)
        {
            portFinder.OnDisable();
        }
    }

}

