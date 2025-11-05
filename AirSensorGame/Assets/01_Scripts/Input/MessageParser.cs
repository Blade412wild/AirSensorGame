using System;
using Unity.VisualScripting;
using UnityEngine;

public class MessageParser : MonoBehaviour
{
    string message = "";
    string varDevider = "|";
    string valueDevider = ":";
    string typeDevider = "/";


    public void Awake()
    {
        DateTime now = DateTime.Now;
        message = "Velocity:10/f|Temperature:32/i|ademIn:0/b";

        Debug.Log(message);
        string[] messageParts = message.Split(varDevider);
        foreach (string messagePart in messageParts)
        {
            //Debug.Log(messagePart);
            string[] types = messagePart.Split(typeDevider);
            for (int i = 0; i < types.Length; i++)
            {
                //Debug.Log(types[i]);

                if (i == 0)
                {
                    string[] vars = types[i].Split(valueDevider);
                    Debug.Log("Name : " + vars[0]);
                    Debug.Log("value : " + vars[1]);

                }

                if (i == 1)
                {
                    Debug.Log(" type = " + types[i]);
                }

            }


        }

        DateTime end = DateTime.Now;
        TimeSpan timeSpan = end - now;
        Debug.Log("timepassed = " + timeSpan.TotalMilliseconds);

    }
}

