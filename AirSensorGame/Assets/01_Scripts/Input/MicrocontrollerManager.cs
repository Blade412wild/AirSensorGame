using System.IO.Ports;
using Unity.VisualScripting;
using UnityEngine;

public class MicrocontrollerManager : MonoBehaviour
{
    public bool test;
    public SerialPort SerialPort { get; private set; }

    public bool SerialPortIsOpen { get; private set; }

    [SerializeField] private int baudrate = 9600;
    [SerializeField] private float portSwitchInterval = 250; // ms
    private SerialPortFinder portFinder;

    public void Awake()
    {
    }

    private void Update()
    {
        if (!test) return;
        TryToFindNewPort();
        test = false;
    }


    private void TryToFindNewPort()
    {
        if(SerialPort != null)
        {
            portFinder.OnDisable();
        }

        portFinder = new SerialPortFinder(baudrate, portSwitchInterval);
        portFinder.OnSerialPortFound += SetSerialPort;

    }
    private void SetSerialPort(SerialPort serialPort)
    {
        SerialPort = serialPort;
        SerialPortIsOpen = true;
        Debug.Log("serial port is set on com : " + SerialPort.PortName);
    }

}

