using UnityEngine;

public class AirVelocityDataReceived : DataAction
{
    public override void OnDataReceived(string value)
    {
        try
        {
            data.AirVelocity = float.Parse(value);

        }
        catch (System.Exception ex)
        {
            Debug.LogError("AirVelocityData went wrong : " + ex.Message);
        }
    }
}
