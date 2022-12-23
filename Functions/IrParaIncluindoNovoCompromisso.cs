using Au;

namespace GCScript_Automate.Functions;

public static class IrParaIncluindoNovoCompromisso
{
    #region WINDOW
    private static string win1Name = "CP-Pro Mais";
    private static string win1Class = "TfrmCProc";
    private static string win1Process = "CProc.exe";
    #endregion

    #region ELEMENTS
    private static wnd win1;
    private static elm? btnIncluir;
    private static elm? btnCompromissos;
    #endregion

    public static GCSResponse Start()
    {
        try
        {
            Settings.NextStep = false;
            for (int i = 0; i < 10; i++)
            {
                if (i > 0) { wait.s(Settings.LA_PositiveWait05); }

                // WINDOW 1
                win1 = wnd.find(Settings.LA_WinNegativeWait, win1Name, win1Class, win1Process);
                if (win1.Is0) { Settings.LastError = $"A janela {win1Name} não foi encontrada!"; continue; }

                // BUTTON INCLUIR
                btnIncluir = win1.Elm["WINDOW", prop: "class=TfrmCompromisso"]["BUTTON", "Incluir..."].Find(Settings.LA_NegativeWait10);
                if (btnIncluir == null)
                {
                    Settings.LastError = $"O botão Incluir não foi encontrado!";

                    // BUTTON ACOMPANHAMENTOS
                    btnCompromissos = win1.Elm["CLIENT", win1.Name, navig: "la fi3 ch2 fi la fi3 ch2 fi la fi2"]["CLIENT", "Compromissos"].Find(Settings.LA_NegativeWait10);
                    if (btnCompromissos == null) { return new() { Success = false, Message = $"O botão Compromissos não foi encontrado!", ErrorCode = ListOfErrorCodes.E140883 }; }
                    btnCompromissos.MouseClick();
                    continue;
                }

                btnIncluir.MouseClick();

                Settings.NextStep = true; break;
            }
            if (!Settings.NextStep) { return new() { Success = false, Message = Settings.LastError, ErrorCode = ListOfErrorCodes.E149898 }; }

            return new() { Success = true };
        }
        catch (Exception error)
        {
            return new() { Success = false, Message = error.Message, ErrorCode = ListOfErrorCodes.E143857 };
        }
    }
}
