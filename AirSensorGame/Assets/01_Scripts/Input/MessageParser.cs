using System;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class MessageParser
{
    private static string varDevider = "|";
    private static string valueDevider = ":";
    private static string typeDevider = "/";

    public static void ParseMessage(string message, BreathingDeviceData datacontainer)
    {
        string[] messageParts = message.Split(varDevider);

        foreach (string messagePart in messageParts)
        {
            if (messagePart == "") continue;

            string[] variables = messagePart.Split(valueDevider);
            //string[] vars = variable.Split(valueDevider);
            SetBreathingData(variables[0], variables[1], datacontainer);
        }
        
    }
    public static void ParseMessage2(string message, BreathingDeviceData datacontainer)
    {
        DateTime now = DateTime.Now;

        //Debug.Log(message);
        string[] messageParts = message.Split(varDevider);

        foreach (string messagePart in messageParts)
        {


            //Debug.Log(messagePart);
            string[] types = messagePart.Split(typeDevider);
            for (int i = 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    string[] vars = types[i].Split(valueDevider);
                    SetBreathingData(vars[0], vars[1], datacontainer);
                }
            }
        }

        DateTime end = DateTime.Now;
        TimeSpan timeSpan = end - now;
        Debug.Log("timepassed = " + timeSpan.TotalMilliseconds);

    }


    private static void SetBreathingData(string name, string value, BreathingDeviceData datacontainer) // to do create a non string based parser.
    {
        switch (name)
        {
            case BreathingDeviceData.AirVelocityName: datacontainer.AirVelocity = float.Parse(value); break;
            case BreathingDeviceData.inExhaleSpeedName: datacontainer.inExhaleSpeed = float.Parse(value); break;
            case BreathingDeviceData.BreathingStateName: datacontainer.BreathingState = (BreathingDeviceData.breathingState)int.Parse(value); break;
            case BreathingDeviceData.ExHalingThroughNoseName: datacontainer.ExHalingThroughNose = bool.Parse(value); break;
        }
    }
}
