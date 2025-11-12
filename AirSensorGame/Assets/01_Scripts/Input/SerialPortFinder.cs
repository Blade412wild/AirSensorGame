using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;
using Unity.VisualScripting;
using JetBrains.Annotations;
using static UnityEngine.Rendering.DebugUI.Table;

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

            Debug.Log("----------");
            TryPortOpeningPort(portName);
            serial.Close();

            COMCounter++;
            //Thread.Sleep((int)portSwitchInterval); // wait before next try
        }

        if (!portFound)
            Debug.LogWarning("No valid COM port found!");
    }

    private bool TryPortOpeningPort(string portName)
    {
        Debug.Log("Try Opening " + portName + "...");
        try
        {
            using (serial = new SerialPort(portName, baudrate))
            {
                serial.ReadTimeout = 20;
                serial.Open();

                Debug.Log("Opening Port : Succes");

                TryReadingData(serial);
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Failed to open {portName}: {ex.Message}");
        }

        return false;
    }

    public void TryReadingData(SerialPort serial)
    {
        DateTime start = DateTime.Now;
        int milliSecondesPassed = 0;
        Debug.Log("Try Reading " + serial.PortName);

        while (milliSecondesPassed <= 200)
        {
            Debug.Log(milliSecondesPassed);

            try
            {
                string message = serial.ReadLine();
                Debug.Log(message);

                int.TryParse(message, out int connectionToken);
                if (connectionToken == 1)
                {
                    Debug.Log("Connection is made");
                    serial.Write("1");
                    portFound = true;
                    break;

                }

            }
            catch (TimeoutException)
            {
                Debug.Log($"{serial.PortName} Couldn't read data");
            }

            DateTime end = DateTime.Now;
            TimeSpan timespan = end - start;
            milliSecondesPassed += (int)(end - start).TotalMilliseconds;

        }




    }

    public void OnDisable()
    {
        if (serial != null)
            serial.Close();

        if (portSearchThread.IsAlive)
            portSearchThread.Abort();
    }



}

