using Au;

namespace GCScript_Automate.Models;

public class SetValueModel
{
    public elm Element { get; set; }
    public string ElementName { get; set; } = "";
    public string ElementValue { get; set; } = "";
    public ESetValueMode Mode { get; set; } = ESetValueMode.Clipboard;
    public bool ClearContent { get; set; } = false;
    public bool SetFocusAndSelect { get; set; } = false;
    public bool HitEnterAfter { get; set; } = false;
    public bool ClickBefore { get; set; } = false;
    public bool CheckIfItWasSuccessful { get; set; } = false;
}

public enum ESetValueMode
{
    Clipboard = 0,
    SendKeys = 1,
    KeysSend = 2
}
