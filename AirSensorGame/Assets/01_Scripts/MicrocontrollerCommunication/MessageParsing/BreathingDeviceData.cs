using System;
using System.Collections.Generic;
using UnityEngine;

public static class BreathingDeviceData // TODO making internal 
{
    //public system data
    public static bool IsConnected;

    // sensor data
    public enum breathingState { inhaling, exhaling, holdingBreath };
    public static breathingState BreathingState;

    public static bool ExHalingThroughNose;
    public static float inExhaleSpeed;
    public static float AirVelocity;

}

