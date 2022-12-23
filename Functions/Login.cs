using Au;
using GCScript_Automate.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GCScript_Automate.Functions;

interface ILogin
{
    void SetElements(int attempts = 0);
    void SetUsername(int attempts = 0);
    void SetPassword(int attempts = 0);
}

public class Login : ILogin
{
    #region WINDOW
    private readonly string winName = "CP-Pro - Login de usuário";
    private readonly string winClass = "TfrmLoginMais";
    private readonly string winProcess = "CProc.exe";
    #endregion

    #region ELEMENTS
    private wnd win;
    private elm? txtUsername;
    private elm? txtPassword;
    #endregion

    #region MODELS
    private string UsernameModel = "";
    private string PasswordModel = "";
    #endregion

    public void Start(LoginModel model)
    {
        UsernameModel = string.IsNullOrEmpty(model.Username) || string.IsNullOrWhiteSpace(model.Username) ? "" : model.Username.Trim();
        PasswordModel = string.IsNullOrEmpty(model.Password) || string.IsNullOrWhiteSpace(model.Password) ? "" : model.Password.Trim();

        try
        {
            SetElements();
            SetUsername();
            SetPassword();
            SendResponse.Send(new() { Success = true });
        }
        catch (Exception error)
        {
            SendResponse.Send(new() { Success = false, Message = error.Message, ErrorCode = ListOfErrorCodes.E147248 });
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

            // TEXT USERNAME
            txtUsername = win.Elm["CLIENT", win.Name, navig: "la fi3"].Find(Settings.LA_NegativeWait10);
            if (txtUsername is null) { Settings.LastError = $"O campo Login não foi encontrado!"; continue; }

            // TEXT PASSWORD
            txtPassword = win.Elm["CLIENT", win.Name, navig: "fi4"].Find(Settings.LA_NegativeWait10);
            if (txtPassword is null) { Settings.LastError = $"O campo Senha não foi encontrado!"; continue; }

            Settings.NextStep = true; break;
        }
        if (!Settings.NextStep) { SendResponse.Send(new() { Success = false, Message = Settings.LastError, ErrorCode = ListOfErrorCodes.E144351 }); }
    }
    public void SetUsername(int attempts = 0)
    {
        if (attempts == 0) { attempts = Settings.DefaultMaxAttempts; }

        Settings.NextStep = false;
        for (int i = 0; i < attempts; i++)
        {
            if (i > 0) { wait.s(Settings.LA_PositiveWait10); }

            SetValueModel sv = new()
            {
                Element = txtUsername,
                ElementName = "Usuário",
                ElementValue = UsernameModel,
                Mode = ESetValueMode.Clipboard,
                ClearContent = true,
                SetFocusAndSelect = true,
                CheckIfItWasSuccessful = true,
            };

            if (!FunctionsLibreAutomate.ElementSetValue(sv))
            {
                Settings.LastError = $"Falha ao preencher o campo {sv.ElementName}!"; continue;
            }

            Settings.NextStep = true; break;
        }
        if (!Settings.NextStep) { SendResponse.Send(new() { Success = false, Message = Settings.LastError, ErrorCode = ListOfErrorCodes.E145565 });}
    }
    public void SetPassword(int attempts = 0)
    {
        if (attempts == 0) { attempts = Settings.DefaultMaxAttempts; }

        Settings.NextStep = false;
        for (int i = 0; i < attempts; i++)
        {
            if (i > 0) { wait.s(Settings.LA_PositiveWait10); }

            SetValueModel sv = new()
            {
                Element = txtPassword,
                ElementName = "Senha",
                ElementValue = PasswordModel,
                Mode = ESetValueMode.Clipboard,
                ClearContent = true,
                SetFocusAndSelect = true,
                CheckIfItWasSuccessful = true,
                HitEnterAfter = true
            };

            if (!FunctionsLibreAutomate.ElementSetValue(sv))
            {
                Settings.LastError = $"Falha ao preencher o campo {sv.ElementName}!"; continue;
            }

            Settings.NextStep = true; break;
        }
        if (!Settings.NextStep) { SendResponse.Send(new() { Success = false, Message = Settings.LastError, ErrorCode = ListOfErrorCodes.E148717 }); }
    }
}
