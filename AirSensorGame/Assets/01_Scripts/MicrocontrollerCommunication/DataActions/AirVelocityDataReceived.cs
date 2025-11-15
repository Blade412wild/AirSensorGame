public class AirVelocityDataReceived : DataAction
{

    public override void OnDataReceived(string value)
    {
        BreathingDeviceData.AirVelocity = float.Parse(value);
    }
}
