using Au;
using GCScript_Automate.Models;

namespace GCScript_Automate.Functions;

interface IIncluindoNovoCompromisso
{
    void SetElements(int attempts = 0);
    void SetTipo(int attempts = 0);
    void SetSubtipo(int attempts = 0);
    void SetDtPublicacao(int attempts = 0);
    void SetDescricao(int attempts = 0);
    void SetDiaInteiro(int attempts = 0);
    void SaveAndClose(int attempts = 0);
}

public class IncluindoNovoCompromisso : IIncluindoNovoCompromisso
{
    #region WINDOW
    private static string winName = "Incluindo novo compromisso";
    private static string winClass = "TfrmCadastroCompromisso";
    private static string winProcess = "CProc.exe";
    #endregion

    #region ELEMENTS
    private static wnd win;
    private static elm? txtTipo; // ch3 fi3 ch3 fi3 ch6 fi3
    private static elm? txtSubtipo; // ch3 fi3 ch3 fi3 ch5 fi3
    private static elm? txtDtPublicacao; // ch3 fi3 ch3 fi3 ch9 fi3
    private static elm? txtDescricao; // ch3 fi3 ch3 fi3 ch2 fi3
    private static elm? chkDiaInteiro; // ch3 fi3 la fi ch8 fi
    private static elm? txtHorarioInicio; // ch3 ch1 ch1 ch1 ch4 ch1 ch9 ch1 ch4 ch1 ch1 ch1
    private static elm? btnSalvarFechar; // ch2 fi ch6 fi
    #endregion

    #region MODELS
    private static string? TipoModel;
    private static string? SubtipoModel;
    private static string? DtPublicacaoModel;
    private static string? DescricaoModel;
    #endregion

    #region OTHERS
    private static GCSResponse result;
    #endregion

    public void Start(IncluindoNovoCompromissoModel model)
    {
        TipoModel = string.IsNullOrEmpty(model.Tipo) || string.IsNullOrWhiteSpace(model.Tipo) ? "" : model.Tipo.Trim();
        SubtipoModel = string.IsNullOrEmpty(model.Subtipo) || string.IsNullOrWhiteSpace(model.Subtipo) ? "" : model.Subtipo.Trim();
        DtPublicacaoModel = string.IsNullOrEmpty(model.DtPublicacao) || string.IsNullOrWhiteSpace(model.DtPublicacao) ? "" : model.DtPublicacao.Trim();
        DescricaoModel = string.IsNullOrEmpty(model.Descricao) || string.IsNullOrWhiteSpace(model.Descricao) ? "." : model.Descricao.Trim();

        try
        {
            SetElements();
            SetDtPublicacao();
            SetTipo();
            SetSubtipo();
            SetDescricao();
            if (model.DiaInteiro) { SetDiaInteiro(); }
            SaveAndClose(3);
            SendResponse.Send(new() { Success = true });
        }
        catch (Exception error)
        {
            SendResponse.Send(new() { Success = false, Message = error.Message, ErrorCode = ListOfErrorCodes.E114147 });
        }
    }

    public void SetElements(int attempts = 0)
    {
        Settings.NextStep = false;
        for (int i = 0; i < Settings.DefaultMaxAttempts; i++)
        {
            if (i > 0) { wait.s(Settings.LA_PositiveWait10); }

            // WINDOW
            win = wnd.find(Settings.LA_WinNegativeWait, winName, winClass);
            if (win.Is0) { Settings.LastError = $"A janela {winName} não foi encontrada!"; continue; }

            // TEXT TIPO
            txtTipo = win.Elm["CLIENT", win.Name, navig: "ch3 fi3 ch3 fi3 ch6 fi3"].Find(Settings.LA_NegativeWait10);
            if (txtTipo is null) { Settings.LastError = $"O campo Tipo não foi encontrado!"; continue; }

            // TEXT SUBTIPO
            txtSubtipo = win.Elm["CLIENT", win.Name, navig: "ch3 fi3 ch3 fi3 ch5 fi3"].Find(Settings.LA_NegativeWait10);
            if (txtSubtipo is null) { Settings.LastError = $"O campo Subtipo não foi encontrado!"; continue; }

            // TEXT DATA
            txtDtPublicacao = win.Elm["CLIENT", win.Name, navig: "ch3 fi3 ch3 fi3 ch9 fi3"].Find(Settings.LA_NegativeWait10);
            if (txtDtPublicacao is null) { Settings.LastError = $"O campo Dt Publicacao não foi encontrado!"; continue; }

            // TEXT DESCRICAO
            txtDescricao = win.Elm["CLIENT", win.Name, navig: "ch3 fi3 ch3 fi3 ch2 fi3"].Find(Settings.LA_NegativeWait10);
            if (txtDescricao is null) { Settings.LastError = $"O campo Descricao não foi encontrado!"; continue; }

            // CHECK DIA INTEIRO
            chkDiaInteiro = win.Elm["CLIENT", win.Name, navig: "ch3 fi3 la fi ch8 fi"].Find(Settings.LA_NegativeWait10);
            if (chkDiaInteiro is null) { Settings.LastError = $"O campo Dia Inteiro não foi encontrado!"; continue; }

            // TEXT HORARIO INICIO
            txtHorarioInicio = win.Elm["CLIENT", win.Name, navig: "ch3 ch1 ch1 ch1 ch4 ch1 ch9 ch1 ch4 ch1 ch1 ch1"].Find(Settings.LA_NegativeWait10);
            if (txtHorarioInicio is null) { Settings.LastError = $"O campo Horario de Inicio não foi encontrado!"; continue; }

            // BUTTON SALVAR E FECHAR
            btnSalvarFechar = win.Elm["CLIENT", win.Name, navig: "ch2 fi ch6 fi"].Find(Settings.LA_NegativeWait10);
            if (btnSalvarFechar is null) { Settings.LastError = $"O botão Salvar e Fechar não foi encontrado!"; continue; }

            Settings.NextStep = true; break;
        }
        if (!Settings.NextStep) { SendResponse.Send(new() { Success = false, Message = Settings.LastError, ErrorCode = ListOfErrorCodes.E161293 }); }
    }
    public void SetTipo(int attempts = 0)
    {
        //FunctionsLibreAutomate.SetValueInComboBox(txtTipo, "Tipo", TipoModel);

        SetValueModel svm = new()
        {
            Element = txtTipo,
            ElementName = "Tipo",
            ElementValue = TipoModel,
            Mode = ESetValueMode.Clipboard,
            ClearContent = true,
            SetFocusAndSelect = true,
            CheckIfItWasSuccessful = true,
            CheckIfTheApplicationCrashedCtrlF2 = true,
            HitEnterAfter = true
        };

        FunctionsLibreAutomate.ElementSetTextComboboxMode1(svm);

    }
    public void SetSubtipo(int attempts = 0)
    {
        if (!string.IsNullOrEmpty(SubtipoModel))
        {
            //FunctionsLibreAutomate.SetValueInComboBox(txtSubtipo, "Subtipo", SubtipoModel);


            SetValueModel svm = new()
            {
                Element = txtSubtipo,
                ElementName = "Subtipo",
                ElementValue = SubtipoModel,
                Mode = ESetValueMode.Clipboard,
                ClearContent = true,
                SetFocusAndSelect = true,
                CheckIfItWasSuccessful = true,
                CheckIfTheApplicationCrashedCtrlF2 = true,
                HitEnterAfter = true

            };

            FunctionsLibreAutomate.ElementSetTextComboboxMode1(svm);
        }
    }
    public void SetDtPublicacao(int attempts = 0)
    {
        SetValueModel svm = new()
        {
            Element = txtDtPublicacao,
            ElementName = "Dt Publicacao",
            ElementValue = DtPublicacaoModel,
            Mode = ESetValueMode.Clipboard,
            ClearContent = true,
            SetFocusAndSelect = true,
            CheckIfItWasSuccessful = true,
        };

        FunctionsLibreAutomate.SetValueInTextBox(svm, attempts);
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
    public void SetDiaInteiro(int attempts = 0)
    {
        if (attempts == 0) { attempts = Settings.DefaultMaxAttempts; }

        Settings.NextStep = false;
        for (int i = 0; i < attempts; i++)
        {
            var horarioInicioIsVisible = txtHorarioInicio.WndContainer.IsVisible;

            if (horarioInicioIsVisible)
            {
                try { chkDiaInteiro.MouseClick(); } catch (Exception) { }
                Settings.LastError = $"Falha ao definir Dia Inteiro!"; continue;
            }

            Settings.NextStep = true; break;
        }
        if (!Settings.NextStep) { SendResponse.Send(new() { Success = false, Message = Settings.LastError, ErrorCode = ListOfErrorCodes.E159779 }); }
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
