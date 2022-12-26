using Au;

namespace GCScript_Automate.Functions;

interface IIrParaCadastroDeAndamentoProcessual
{
    void Run(int attempts = 0);
}

public class IrParaCadastroDeAndamentoProcessual : IIrParaCadastroDeAndamentoProcessual
{
    #region WINDOW
    private string winName = "CP-Pro Mais";
    private string winClass = "TfrmCProc";
    private string winProcess = "CProc.exe";

    private string? win2Name = null;
    private string win2Class = "#32768";
    private string win2Process = "CProc.exe";
    #endregion

    #region ELEMENTS
    private wnd win;
    private wnd win2;
    private elm? btnIncluir;
    private elm? btnAcompanhamentos;
    private elm? btnIncluirAndamento;


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

            btnIncluir = win.Elm["WINDOW", prop: "class=TfrmAcompanhamento"]["BUTTON", "Incluir"].Find(Settings.LA_NegativeWait03);
            if (btnIncluir == null)
            {
                Settings.LastError = $"O botão Incluir não foi encontrado!";

                btnAcompanhamentos = win.Elm["CLIENT", win.Name, navig: "la fi3 ch2 fi la fi3 ch2 fi la fi2"]["CLIENT", "Acompanhamentos"].Find(Settings.LA_NegativeWait10);
                if (btnAcompanhamentos == null)
                {
                    SendResponse.Send(new() { Success = false, Message = $"O botão Acompanhamentos não foi encontrado!", ErrorCode = ListOfErrorCodes.E134695 });
                }

                FunctionsLibreAutomate.ClickOnElement(btnAcompanhamentos);

                continue;
            }

            if (!FunctionsLibreAutomate.ClickOnElement(btnIncluir)) { continue; }
            #endregion

            #region STEP2
            win2 = wnd.find(Settings.LA_NegativeWait05, win2Name, win2Class, win2Process);
            if (win2.Is0)
            {
                Settings.LastError = $"A janela do botão Incluir Andamento não foi encontrada!";
                continue;
            }

            btnIncluirAndamento = win2.Elm["MENUITEM", "Incluir Andamento..."].Find(Settings.LA_NegativeWait10);
            if (btnIncluirAndamento == null)
            {
                Settings.LastError = $"O botão Incluir Andamento não foi encontrado!";
                continue;
            }

            if (!FunctionsLibreAutomate.ClickOnElement(btnIncluirAndamento)) { continue; }
            #endregion

            Settings.NextStep = true; break;
        }
        if (!Settings.NextStep) { SendResponse.Send(new() { Success = false, Message = Settings.LastError, ErrorCode = ListOfErrorCodes.E134695 }); }
    }
}
