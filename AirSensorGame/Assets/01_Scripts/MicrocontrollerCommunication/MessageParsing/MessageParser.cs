public class MessageParser
{
    private static string varDevider = "|";
    private static string valueDevider = ":";
    private static string typeDevider = "/";

    public static void ParseMessage(string message)
    {
        string[] messageParts = message.Split(varDevider);

        foreach (string messagePart in messageParts)
        {
            if (messagePart == "") continue;

            string[] variables = messagePart.Split(valueDevider);
            //string[] vars = variable.Split(valueDevider);
            if (sbyte.TryParse(variables[0], out sbyte messageToken))
            {
                SetBreathingData(messageToken, variables[1]);
            }
        }

    }

    private static void SetBreathingData(sbyte messageToken, string value) // to do create a non string based parser.
    {
        //if (BreathingDeviceCommmunicationParserList.DeviceDataHandleDic.TryGetValue(messageToken, out DataAction action))
        //{
        //    action.OnDataReceived(value);
        //}
    }


}
