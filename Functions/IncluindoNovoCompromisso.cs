using Au;
using GCScript_Automate.Models;

namespace GCScript_Automate.Functions;

public static class IncluindoNovoCompromisso
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

    public static GCSResponse Start(IncluindoNovoCompromissoModel model)
    {
        TipoModel = string.IsNullOrEmpty(model.Tipo) || string.IsNullOrWhiteSpace(model.Tipo) ? "" : model.Tipo.Trim();
        SubtipoModel = string.IsNullOrEmpty(model.Subtipo) || string.IsNullOrWhiteSpace(model.Subtipo) ? "" : model.Subtipo.Trim();
        DtPublicacaoModel = string.IsNullOrEmpty(model.DtPublicacao) || string.IsNullOrWhiteSpace(model.DtPublicacao) ? "" : model.DtPublicacao.Trim();
        DescricaoModel = string.IsNullOrEmpty(model.Descricao) || string.IsNullOrWhiteSpace(model.Descricao) ? "." : model.Descricao.Trim();

        try
        {

            #region SET ELEMENTS
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
            if (!Settings.NextStep) { return new() { Success = false, Message = Settings.LastError, ErrorCode = ListOfErrorCodes.E150836 }; }
            #endregion

            #region SET TIPO
            result = FunctionsLibreAutomate.SetValueInComboBox(txtTipo, "Tipo", TipoModel);
            if (!result.Success) { return result; }
            #endregion

            #region SET SUBTIPO
            if (!string.IsNullOrEmpty(SubtipoModel))
            {
                result = FunctionsLibreAutomate.SetValueInComboBox(txtSubtipo, "Subtipo", SubtipoModel);
                if (!result.Success) { return result; }
            }
            #endregion

            #region SET DT PUBLICACAO
            SetValueModel svmDtPublicacao = new()
            {
                Element = txtDtPublicacao,
                ElementName = "Dt Publicacao",
                ElementValue = DtPublicacaoModel,
                Mode = ESetValueMode.Clipboard,
                ClearContent = true,
                SetFocusAndSelect = true,
                CheckIfItWasSuccessful = true,
            };

            result = FunctionsLibreAutomate.SetValueInTextBox(svmDtPublicacao);
            if (!result.Success) { return result; }
            #endregion

            #region SET DESCRICAO
            if (DescricaoModel.Length > 2800) { DescricaoModel = DescricaoModel[..2800]; }
            SetValueModel svmDescricao = new()
            {
                Element = txtDescricao,
                ElementName = "Descricao",
                ElementValue = DescricaoModel,
                Mode = ESetValueMode.Clipboard,
                ClearContent = true,
                SetFocusAndSelect = true,
                CheckIfItWasSuccessful = true,
            };

            result = FunctionsLibreAutomate.SetValueInTextBox(svmDescricao);
            if (!result.Success) { return result; }
            #endregion

            #region CLICK DIA INTEIRO
            if (model.DiaInteiro)
            {
                Settings.NextStep = false;
                for (int i = 0; i < Settings.DefaultMaxAttempts; i++)
                {
                    var horarioInicioIsVisible = txtHorarioInicio.WndContainer.IsVisible;

                    if (horarioInicioIsVisible)
                    {
                        try { chkDiaInteiro.MouseClick(); } catch (Exception) { }
                        Settings.LastError = $"Falha ao definir Dia Inteiro!"; continue;
                    }

                    Settings.NextStep = true; break;
                }
                if (!Settings.NextStep) { return new() { Success = false, Message = Settings.LastError, ErrorCode = ListOfErrorCodes.E159779 }; }
            }

            #endregion

            #region CLICK SALVAR E FECHAR
            Settings.NextStep = false;
            for (int i = 0; i < 3; i++)
            {
                try { btnSalvarFechar.MouseClickD(); } catch (Exception) { }
                win.WaitForClosed(Settings.LA_NegativeWait30);
                if (win.IsAlive)
                {
                    Settings.LastError = $"Falha ao clicar no botão Salvar e Fechar!"; continue;
                }
                Settings.NextStep = true; break;
            }
            if (!Settings.NextStep) { return new() { Success = false, Message = Settings.LastError, ErrorCode = ListOfErrorCodes.E155745 }; }
            #endregion

            return new() { Success = true };
        }
        catch (Exception error)
        {
            return new() { Success = false, Message = error.Message, ErrorCode = ListOfErrorCodes.E114147 };
        }
    }
}
