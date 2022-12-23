using Au;

namespace GCScript_Automate.Functions;

public static class IrParaCadastroDeAndamentoProcessual
{
    #region WINDOW
    private static string win1Name = "CP-Pro Mais";
    private static string win1Class = "TfrmCProc";
    private static string win1Process = "CProc.exe";

    private static string? win2Name = null;
    private static string win2Class = "#32768";
    private static string win2Process = "CProc.exe";
    #endregion

    #region ELEMENTS
    private static wnd win1;
    private static wnd win2;
    private static elm? btnIncluir;
    private static elm? btnAcompanhamentos;
    private static elm? btnIncluirAndamento;
    #endregion

    public static GCSResponse Start()
    {

        try
        {
            Settings.NextStep = false;
            for (int i = 0; i < 10; i++)
            {
                if (i > 0) { wait.s(Settings.LA_PositiveWait10); }

                // WINDOW 1
                win1 = wnd.find(Settings.LA_WinNegativeWait, win1Name, win1Class, win1Process);
                if (win1.Is0) { Settings.LastError = $"A janela {win1Name} não foi encontrada!"; continue; }

                // BUTTON INCLUIR
                btnIncluir = win1.Elm["WINDOW", prop: "class=TfrmAcompanhamento"]["BUTTON", "Incluir"].Find(Settings.LA_NegativeWait10);
                if (btnIncluir == null)
                {
                    Settings.LastError = $"O botão Incluir não foi encontrado!";

                    // BUTTON ACOMPANHAMENTOS
                    btnAcompanhamentos = win1.Elm["CLIENT", win1.Name, navig: "la fi3 ch2 fi la fi3 ch2 fi la fi2"]["CLIENT", "Acompanhamentos"].Find(Settings.LA_NegativeWait10);
                    if (btnAcompanhamentos == null) { return new() { Success = false, Message = $"O botão Acompanhamentos não foi encontrado!", ErrorCode = ListOfErrorCodes.E134695 }; }
                    btnAcompanhamentos.MouseClick();
                    continue;
                }

                btnIncluir.MouseClick();

                // WINDOW 2
                win2 = wnd.find(Settings.LA_NegativeWait05, win2Name, win2Class, win2Process);
                if (win2.Is0)
                {
                    Settings.LastError = $"A janela do botão Incluir Andamento não foi encontrada!";
                    continue;
                }
                wait.s(Settings.LA_PositiveWait03);

                // BUTTON INCLUIR ANDAMENTO...
                btnIncluirAndamento = win2.Elm["MENUITEM", "Incluir Andamento..."].Find(Settings.LA_NegativeWait10);
                if (btnIncluirAndamento == null)
                {
                    Settings.LastError = $"O botão Incluir Andamento não foi encontrado!";
                    continue;
                }

                btnIncluirAndamento.MouseClick();

                Settings.NextStep = true; break;
            }
            if (!Settings.NextStep) { return new() { Success = false, Message = Settings.LastError, ErrorCode = ListOfErrorCodes.E134695 }; }

            return new() { Success = true };
        }
        catch (Exception error)
        {
            return new() { Success = false, Message = error.Message, ErrorCode = ListOfErrorCodes.E138434 };
        }
    }
}
