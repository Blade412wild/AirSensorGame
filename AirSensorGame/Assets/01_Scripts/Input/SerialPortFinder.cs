using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class SerialPortFinder
{
    public Action<SerialPort> OnSerialPortFound;
    private int baudrate = 9600;
    private SerialPort serial;
    private bool portFound = false;

    private float portSwitchInterval = 2000f; // ms
    private int COMCounter = 0;
    private string[] availablePorts;
    private Thread portSearchThread;

    public SerialPortFinder(int baudRate, float portSwitchInterval)
    {
        this.baudrate = baudRate;
        this.portSwitchInterval = portSwitchInterval;
        Setup();
    }
    void Setup()
    {
        availablePorts = SerialPort.GetPortNames();
        foreach (string portName in availablePorts)
            Debug.Log($"Found port: {portName}");

        // Start the search on a separate thread to avoid blocking Unity
        portSearchThread = new Thread(TryToFindPort);
        portSearchThread.Start();
    }

    private void TryToFindPort()
    {
        while (!portFound && COMCounter < availablePorts.Length)
        {
            string portName = availablePorts[COMCounter];
            Debug.Log($"Trying port {portName}");

            if (TryPort(portName))
            {
                Debug.Log($"Port found: {portName}");
                portFound = true;
                break;
            }

            COMCounter++;
            Thread.Sleep((int)portSwitchInterval); // wait before next try
        }

        if (!portFound)
            Debug.LogWarning("No valid COM port found!");
    }

    private bool TryPort(string portName)
    {
        try
        {
            using (SerialPort sp = new SerialPort(portName, baudrate))
            {
                sp.ReadTimeout = 20; // ms
                sp.Open();

                try
                {
                    string line = sp.ReadLine();
                    Debug.Log($"{portName} received: {line}");

                    if (line.Trim() == "1")
                    {
                        sp.Write("1");
                        OnSerialPortFound?.Invoke(sp);
                        return true;
                    }
                }
                catch (TimeoutException)
                {
                    Debug.Log($"{portName} timed out (no data).");
                }

                sp.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Failed to open {portName}: {ex.Message}");
        }

        return false;
    }

    public void OnDisable()
    {
        portSearchThread.Abort();
    }


    
}

