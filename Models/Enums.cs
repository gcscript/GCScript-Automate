namespace GCScript_Automate.Models;

public static partial class Enums
{
    public enum EResponseStatus { Running = 0, Finished = 1 }

    public enum ELASetTextMode { KeysSend = 0, SendKeys = 1 }

    public enum EFlaUISearchButton { ByAutomationId = 0, ByClassName = 1, ByName = 2, ByText = 3, ByValue = 4 }

    public enum EAutoItSetTextMode { ControlSetText = 0, ControlSend = 1, ControlCommand = 2, }
}
