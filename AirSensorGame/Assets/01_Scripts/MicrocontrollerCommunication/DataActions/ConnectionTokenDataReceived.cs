public class ConnectionTokenDataReceived : DataAction
{

    public override void OnDataReceived(string value)
    {
        BreathingDeviceData.IsConnected = bool.Parse(value);
    }
}
