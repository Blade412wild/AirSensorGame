public class ExhalingThroughNoseDataReceived : DataAction
{
    public override void OnDataReceived(string value)
    {
        BreathingDeviceData.ExHalingThroughNose = bool.Parse(value);
    }

}
