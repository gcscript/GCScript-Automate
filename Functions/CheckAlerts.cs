using Au;

namespace GCScript_Automate.Functions;

public static class CheckAlerts
{
    #region WINDOW
    private static string winName = "CP-Pro Mais";
    private static string winClass = "TfrmMsg";
    private static string winProcess = "CProc.exe";
    #endregion

    #region ELEMENTS
    private static wnd win;
    private static elm? txtMessage;
    #endregion

    public static GCSResponse Start()
    {
        try
        {
            // WINDOW
            win = wnd.find(Settings.LA_NegativeWait03, winName, winClass, winProcess);
            if (win.Is0) { return new GCSResponse() { Success = true }; } // A JANELA NÃO EXISTE

            // TEXT MESSAGE
            txtMessage = win.Elm["CLIENT", win.Name, navig: "ch2 ch1 ch1 ch1 ch2 ch1"].Find(Settings.LA_NegativeWait03);
            if (txtMessage is null)
            {
                return new() { Success = false, Message = "Não foi possível obter a mensagem do alerta!", ErrorCode = ListOfErrorCodes.E101832 };
            }

            string message = txtMessage.WndContainer.ControlText;

            if (Tools.OnlyLettersAndNumbers(message).Contains("NENHUMREGISTROENCONTRADONAPESQUISA"))
            {
                message = "Processo não encontrado!";
            }

            win.Close();
            return new GCSResponse() { Success = false, Message = message, ErrorCode = ListOfErrorCodes.E101782 };
        }
        catch (Exception error)
        {
            return new() { Success = false, Message = error.Message, ErrorCode = ListOfErrorCodes.E102340 };
        }
    }
}
