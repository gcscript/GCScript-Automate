using Au;
using GCScript_Automate.Models;

namespace GCScript_Automate.Functions;

interface ICadastroDeAndamentoProcessual
{
    void SetElements(int attempts = 0);
    void SetData(int attempts = 0);
    void SetTipo(int attempts = 0);
    void SetSubtipo(int attempts = 0);
    void SetDescricao(int attempts = 0);
    void SaveAndClose(int attempts = 0);
}

public class CadastroDeAndamentoProcessual : ICadastroDeAndamentoProcessual
{
    #region WINDOW
    private readonly string winName = "Cadastro de andamento processual";
    private readonly string winClass = "TfrmProcAcompIncAlt";
    private readonly string winProcess = "CProc.exe";
    #endregion

    #region ELEMENTS
    private wnd win;
    private elm? txtData; // ch2 fi5 la fi la fi ch11 fi3
    private elm? txtTipo; // ch2 fi5 la fi la fi ch2 fi3
    private elm? txtSubtipo; // ch2 fi5 la fi la fi ch13 fi3
    private elm? txtDescricao; // ch2 fi5 la fi ch2 fi3 la fi
    private elm? btnSalvarFechar; // fi2 ch3 fi
    #endregion

    #region MODELS
    private string DataModel = "";
    private string TipoModel = "";
    private string SubtipoModel = "";
    private string DescricaoModel = "";
    #endregion

    #region OTHERS
    private GCSResponse result;
    #endregion

    public void Start(CadastroDeAndamentoProcessualModel model)
    {
        DataModel = string.IsNullOrEmpty(model.Data) || string.IsNullOrWhiteSpace(model.Data) ? "" : model.Data.Trim();
        TipoModel = string.IsNullOrEmpty(model.Tipo) || string.IsNullOrWhiteSpace(model.Tipo) ? "" : model.Tipo.Trim();
        SubtipoModel = string.IsNullOrEmpty(model.Subtipo) || string.IsNullOrWhiteSpace(model.Subtipo) ? "" : model.Subtipo.Trim();
        DescricaoModel = string.IsNullOrEmpty(model.Descricao) || string.IsNullOrWhiteSpace(model.Descricao) ? "." : model.Descricao.Trim();

        try
        {
            SetElements();
            SetData();
            SetTipo();
            SetSubtipo();
            SetDescricao();
            SaveAndClose(3);
            SendResponse.Send(new() { Success = true });
        }
        catch (Exception error)
        {
            SendResponse.Send(new() { Success = false, Message = error.Message, ErrorCode = ListOfErrorCodes.E159767 });
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
            win = wnd.find(Settings.LA_WinNegativeWait, winName, winClass);
            if (win.Is0) { Settings.LastError = $"A janela {winName} não foi encontrada!"; continue; }

            // TEXT DATA
            txtData = win.Elm["CLIENT", win.Name, navig: "ch2 fi5 la fi la fi ch11 fi3"].Find(Settings.LA_NegativeWait10);
            if (txtData is null) { Settings.LastError = $"O campo Data não foi encontrado!"; continue; }

            // TEXT TIPO
            txtTipo = win.Elm["CLIENT", win.Name, navig: "ch2 fi5 la fi la fi ch2 fi3"].Find(Settings.LA_NegativeWait10);
            if (txtTipo is null) { Settings.LastError = $"O campo Tipo não foi encontrado!"; continue; }

            // TEXT SUBTIPO
            txtSubtipo = win.Elm["CLIENT", win.Name, navig: "ch2 fi5 la fi la fi ch13 fi3"].Find(Settings.LA_NegativeWait10);
            if (txtSubtipo is null) { Settings.LastError = $"O campo Subtipo não foi encontrado!"; continue; }

            // TEXT DESCRICAO
            txtDescricao = win.Elm["CLIENT", win.Name, navig: "ch2 fi5 la fi ch2 fi3 la fi"].Find(Settings.LA_NegativeWait10);
            if (txtDescricao is null) { Settings.LastError = $"O campo Descricao não foi encontrado!"; continue; }

            // BUTTON SALVAR E FECHAR
            btnSalvarFechar = win.Elm["CLIENT", win.Name, navig: "fi2 ch3 fi"].Find(Settings.LA_NegativeWait10);
            if (btnSalvarFechar is null) { Settings.LastError = $"O botão Salvar e Fechar não foi encontrado!"; continue; }

            Settings.NextStep = true; break;
        }
        if (!Settings.NextStep) { SendResponse.Send(new() { Success = false, Message = Settings.LastError, ErrorCode = ListOfErrorCodes.E150836 }); }
    }
    public void SetData(int attempts = 0)
    {
        SetValueModel svm = new()
        {
            Element = txtData,
            ElementName = "Data",
            ElementValue = DataModel,
            Mode = ESetValueMode.Clipboard,
            ClearContent = true,
            SetFocusAndSelect = true,
            CheckIfItWasSuccessful = true,
        };

        FunctionsLibreAutomate.SetValueInTextBox(svm, attempts);
    }
    public void SetTipo(int attempts = 0)
    {
        FunctionsLibreAutomate.SetValueInComboBox(txtTipo, "Tipo", TipoModel);
    }
    public void SetSubtipo(int attempts = 0)
    {
        if (!string.IsNullOrEmpty(SubtipoModel))
        {
            FunctionsLibreAutomate.SetValueInComboBox(txtSubtipo, "Subtipo", SubtipoModel);
        }
    }
    public void SetDescricao(int attempts = 0)
    {
        if (DescricaoModel.Length > 2800) { DescricaoModel = DescricaoModel[..2800]; }

        SetValueModel svm = new()
        {
            Element = txtDescricao,
            ElementName = "Descricao",
            ElementValue = DescricaoModel,
            Mode = ESetValueMode.Clipboard,
            ClearContent = true,
            SetFocusAndSelect = true,
            CheckIfItWasSuccessful = true,
        };

        FunctionsLibreAutomate.SetValueInTextBox(svm, attempts);
    }
    public void SaveAndClose(int attempts = 0)
    {
        if (attempts == 0) { attempts = Settings.DefaultMaxAttempts; }

        Settings.NextStep = false;
        for (int i = 0; i < attempts; i++)
        {
            try { btnSalvarFechar.MouseClickD(); } catch (Exception) { }

            win.WaitForClosed(Settings.LA_NegativeWait30);

            if (win.IsAlive)
            {
                Settings.LastError = $"Falha ao clicar no botão Salvar e Fechar!"; continue;
            }
            Settings.NextStep = true; break;
        }
        if (!Settings.NextStep) { SendResponse.Send(new() { Success = false, Message = Settings.LastError, ErrorCode = ListOfErrorCodes.E155745 }); }
    }
}
