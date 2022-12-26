using Au;

namespace GCScript_Automate.Functions;

//public static class IrParaIncluindoNovoCompromisso
//{
//    public static GCSResponse Start()
//    {
//        try
//        {
//            Settings.NextStep = false;
//            for (int i = 0; i < 10; i++)
//            {
//                if (i > 0) { wait.s(Settings.LA_PositiveWait05); }

//                // WINDOW 1
//                win1 = wnd.find(Settings.LA_WinNegativeWait, win1Name, win1Class, win1Process);
//                if (win1.Is0) { Settings.LastError = $"A janela {win1Name} não foi encontrada!"; continue; }

//                // BUTTON INCLUIR
//                btnIncluir = win1.Elm["WINDOW", prop: "class=TfrmCompromisso"]["BUTTON", "Incluir..."].Find(Settings.LA_NegativeWait10);
//                if (btnIncluir == null)
//                {
//                    Settings.LastError = $"O botão Incluir não foi encontrado!";

//                    // BUTTON ACOMPANHAMENTOS
//                    btnCompromissos = win1.Elm["CLIENT", win1.Name, navig: "la fi3 ch2 fi la fi3 ch2 fi la fi2"]["CLIENT", "Compromissos"].Find(Settings.LA_NegativeWait10);
//                    if (btnCompromissos == null) { return new() { Success = false, Message = $"O botão Compromissos não foi encontrado!", ErrorCode = ListOfErrorCodes.E140883 }; }
//                    btnCompromissos.MouseClick();
//                    continue;
//                }

//                btnIncluir.MouseClick();

//                Settings.NextStep = true; break;
//            }
//            if (!Settings.NextStep) { return new() { Success = false, Message = Settings.LastError, ErrorCode = ListOfErrorCodes.E149898 }; }

//            return new() { Success = true };
//        }
//        catch (Exception error)
//        {
//            return new() { Success = false, Message = error.Message, ErrorCode = ListOfErrorCodes.E143857 };
//        }
//    }
//}



interface IIrParaIncluindoNovoCompromisso
{
    void Run(int attempts = 0);
}

public class IrParaIncluindoNovoCompromisso : IIrParaIncluindoNovoCompromisso
{
    #region WINDOW
    private string winName = "CP-Pro Mais";
    private string winClass = "TfrmCProc";
    private string winProcess = "CProc.exe";
    #endregion

    #region ELEMENTS
    private wnd win;
    private elm? btnIncluir;
    private elm? btnCompromissos;


    #endregion

    public void Start()
    {

        try
        {
            Run();
            SendResponse.Send(new() { Success = true });
        }
        catch (Exception error)
        {
            SendResponse.Send(new() { Success = false, Message = error.Message, ErrorCode = ListOfErrorCodes.E138434 });
        }
    }

    public void Run(int attempts = 0)
    {
        if (attempts == 0) { attempts = Settings.DefaultMaxAttempts; }

        Settings.NextStep = false;
        for (int i = 0; i < attempts; i++)
        {
            #region STEP1
            win = wnd.find(Settings.LA_WinNegativeWait, winName, winClass, winProcess);
            if (win.Is0) { Settings.LastError = $"A janela {winName} não foi encontrada!"; continue; }

            btnIncluir = win.Elm["WINDOW", prop: "class=TfrmCompromisso"]["BUTTON", "Incluir..."].Find(Settings.LA_NegativeWait03);
            if (btnIncluir == null)
            {
                Settings.LastError = $"O botão Incluir não foi encontrado!";

                btnCompromissos = win.Elm["CLIENT", win.Name, navig: "la fi3 ch2 fi la fi3 ch2 fi la fi2"]["CLIENT", "Compromissos"].Find(Settings.LA_NegativeWait10);
                if (btnCompromissos == null)
                {
                    SendResponse.Send(new() { Success = false, Message = $"O botão Compromissos não foi encontrado!", ErrorCode = ListOfErrorCodes.E134695 });
                }

                FunctionsLibreAutomate.ClickOnElement(btnCompromissos);

                continue;
            }

            if (!FunctionsLibreAutomate.ClickOnElement(btnIncluir)) { continue; }
            #endregion

            Settings.NextStep = true; break;
        }
        if (!Settings.NextStep) { SendResponse.Send(new() { Success = false, Message = Settings.LastError, ErrorCode = ListOfErrorCodes.E134695 }); }
    }
}
