using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;

public class SerialPortFinder : MonoBehaviour
{
    public int baudrate = 9600;
    private SerialPort serial;
    private bool portFound = false;

    public float portSwitchInterval = 2000f; // ms
    private float currentTime = 0f;
    private int COMCounter = 0;
    private string[] availablePorts;

    void Start()
    {
        availablePorts = SerialPort.GetPortNames();
        foreach (string portName in availablePorts)
            Debug.Log($"Found port: {portName}");

        // Start the search on a separate thread to avoid blocking Unity
        Thread portSearchThread = new Thread(TryToFindPort);
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
                Debug.Log($"✅ Port found: {portName}");
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
                sp.ReadTimeout = 500; // ms
                sp.Open();

                try
                {
                    string line = sp.ReadLine();
                    Debug.Log($"{portName} received: {line}");

                    if (line.Trim() == "1")
                    {
                        sp.Write("1");
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
        catch (System.Exception ex)
        {
            Debug.LogWarning($"Failed to open {portName}: {ex.Message}");
        }

        return false;
    }
}

