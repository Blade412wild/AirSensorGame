using System.Collections.Generic;

public class BreathingDeviceData
{
    //public system data
    public bool IsConnected;


    // sensor data
    public enum breathingState { inhaling, exhaling, holdingBreath };
    public breathingState BreathingState;

    public bool ExHalingThroughNose;
    public float inExhaleSpeed;
    public float AirVelocity;

    public const string connectionToken = "Connected";
    public const string AirVelocityName = "AirVelocity";
    public const string inExhaleSpeedName = "inExhaleSpeed";
    public const string ExHalingThroughNoseName = "ExHalingThroughNose";
    public const string BreathingStateName = "BreathingState";

    

}

