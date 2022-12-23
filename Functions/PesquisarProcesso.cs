using Au;
using GCScript_Automate.Models;

namespace GCScript_Automate.Functions;

interface IPesquisarProcesso
{
    void SetElements(int attempts = 0);
    void Search(int attempts = 0);
}

public class PesquisarProcesso : IPesquisarProcesso
{
    #region WINDOW
    private readonly string winName = "CP-Pro Mais";
    private readonly string winClass = "TfrmCProc";
    private readonly string winProcess = "CProc.exe";
    #endregion

    #region ELEMENTS
    private wnd win;
    private elm? txtPesquisa;
    private elm? btnPesquisar;
    private elm? btnLimparPesquisa;
    private elm? lblQuantidadePasta;
    #endregion

    #region MODELS
    private string PalavraChaveModel = "";
    #endregion

    public void Start(PesquisarProcessoModel model)
    {
        PalavraChaveModel = string.IsNullOrEmpty(model.PalavraChave) || string.IsNullOrWhiteSpace(model.PalavraChave) ? "" : model.PalavraChave.Trim();

        try
        {
            SetElements();
            Search();
            SendResponse.Send(new() { Success = true });
        }
        catch (Exception error)
        {
            SendResponse.Send(new() { Success = false, Message = error.Message, ErrorCode = ListOfErrorCodes.E112803 });
        }
    }

    public void SetElements(int attempts = 0)
    {
        if (attempts == 0) { attempts = Settings.DefaultMaxAttempts; }

        Settings.NextStep = false;
        for (int i = 0; i < attempts; i++)
        {
            if (i > 0) { wait.s(Settings.LA_PositiveWait10); }

            // WINDOW
            win = wnd.find(Settings.LA_WinNegativeWait, winName, winClass, winProcess);
            if (win.Is0) { Settings.LastError = $"A janela {winName} não foi encontrada!"; continue; }

            // TEXT PESQUISAR
            txtPesquisa = win.Elm["CLIENT", win.Name, navig: "la fi3 ch2 fi la fi3 la fi la fi5"].Find(Settings.LA_NegativeWait10);
            if (txtPesquisa is null) { Settings.LastError = $"O campo de pesquisa não foi encontrado!"; continue; }

            // BUTTON PESQUISAR
            btnPesquisar = win.Elm["CLIENT", win.Name, navig: "la fi3 ch2 fi la fi3 la fi la fi ch4 fi"].Find(Settings.LA_NegativeWait10);
            if (btnPesquisar is null) { Settings.LastError = $"O botão Pesquisar não foi encontrado!"; continue; }

            // BUTTON LIMPAR PESQUISA
            btnLimparPesquisa = win.Elm["CLIENT", win.Name, navig: "la fi3 ch2 fi la fi3 la fi la fi ch3 fi"].Find(Settings.LA_NegativeWait10);
            if (btnLimparPesquisa is null) { Settings.LastError = $"O botão Limpar Pesquisa não foi encontrado!"; continue; }

            // LABEL QUANTIDADE PASTA
            lblQuantidadePasta = win.Elm["CLIENT", win.Name, navig: "la fi la fi5"].Find(Settings.LA_NegativeWait10);
            if (lblQuantidadePasta is null) { Settings.LastError = $"A Quantidade de Pastas não foi encontrada!"; continue; }

            Settings.NextStep = true; break;
        }
        if (!Settings.NextStep) { SendResponse.Send(new() { Success = false, Message = Settings.LastError, ErrorCode = ListOfErrorCodes.E129695 }); }
    }

    public void Search(int attempts = 0)
    {
        if (attempts == 0) { attempts = Settings.DefaultMaxAttempts; }

        Settings.NextStep = false;
        for (int i = 0; i < attempts; i++)
        {
            if (i > 0) { wait.s(Settings.LA_PositiveWait10); }

            btnLimparPesquisa.MouseClickD();

            lblQuantidadePasta.WaitFor(Settings.LA_NegativeWait03, o => Tools.OnlyLettersAndNumbers(o.WndContainer.ControlText) == "PASTA0DE0");

            string currentControlValue = Tools.OnlyLettersAndNumbers(lblQuantidadePasta.WndContainer.ControlText);

            if (currentControlValue != "PASTA0DE0") { Settings.LastError = $"Falha ao limpar o campo Pesquisar!"; continue; }

            SetValueModel svm = new()
            {
                Element = txtPesquisa,
                ElementName = "Pesquisa",
                ElementValue = PalavraChaveModel,
                Mode = ESetValueMode.Clipboard,
                ClearContent = true,
                SetFocusAndSelect = true,
                CheckIfItWasSuccessful = true,
                HitEnterAfter = true
            };

            if (!FunctionsLibreAutomate.ElementSetValue(svm))
            {
                Settings.LastError = $"Falha ao preencher o campo {svm.ElementName}!"; continue;
            }

            lblQuantidadePasta.WaitFor(Settings.LA_NegativeWait10, o => Tools.OnlyLettersAndNumbers(o.WndContainer.ControlText).StartsWith("PASTA1DE"));

            currentControlValue = Tools.OnlyLettersAndNumbers(lblQuantidadePasta.WndContainer.ControlText);

            if (currentControlValue == "PASTA0DE0")
            {
                GCSResponse alertsResult = CheckAlerts.Start();
                if (alertsResult.Success)
                {
                    Settings.LastError = $"Falha ao pesquisar processo!"; continue;
                }
                else
                {
                    SendResponse.Send(alertsResult);;
                }
            }

            if (!currentControlValue.StartsWith("PASTA1DE"))
            {
                Settings.LastError = $"Falha ao pesquisar processo!"; continue;
            }

            Settings.NextStep = true; break;
        }
        if (!Settings.NextStep) { SendResponse.Send(new() { Success = false, Message = Settings.LastError, ErrorCode = ListOfErrorCodes.E112682 }); }
    }
}
