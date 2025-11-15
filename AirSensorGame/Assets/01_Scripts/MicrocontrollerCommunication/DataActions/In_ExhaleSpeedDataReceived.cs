public class In_ExhaleSpeedDataReceived : DataAction
{

    public override void OnDataReceived(string value)
    {
        BreathingDeviceData.inExhaleSpeed = float.Parse(value);
    }
}
