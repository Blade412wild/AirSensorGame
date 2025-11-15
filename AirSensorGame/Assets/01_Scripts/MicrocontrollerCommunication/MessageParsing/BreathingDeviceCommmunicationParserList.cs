using System.Collections.Generic;
using UnityEngine;

public class BreathingDeviceCommmunicationParserList
{

    //[SerializeField] private sbyte[] sbyteArray;
    //[SerializeField] private DataAction[] DataActions;


    //public static Dictionary<sbyte, DataAction> DeviceDataHandleDic = new Dictionary<sbyte, DataAction>() //TODO make internal
    //{
    //    {AirVelocity, new  AirVelocityDataReceived()},
    //    {ExHalingThroughNose, new  ExhalingThroughNoseDataReceived()},
    //    {In_ExhaleSpeed, new  In_ExhaleSpeedDataReceived()}
    //}; 

    //connection 
    public const sbyte ConnectionToken = -1;
    public const sbyte ConnectionLostToken = -2;

    //Variables
    public const sbyte BreathingState = 1;

    public const sbyte AirVelocity = 3;
    public const sbyte ExHalingThroughNose = 2;
    public const sbyte In_ExhaleSpeed = 4;

}
