using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BreathingDevice/DataContainer", fileName = "DataContainer")]
public class BreathingDeviceData : ScriptableObject
{
    //public system data
    public bool IsConnected;

    // sensor data
    public enum breathingState { inhaling, exhaling, holdingBreath };
    public breathingState BreathingState;

    public bool ExHalingThroughNose;
    public float inExhaleSpeed;
    public float AirVelocity;

}

