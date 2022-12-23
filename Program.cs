namespace GCScript_Automate;

using Au;
using GCScript_Automate.Functions;
using GCScript_Automate.Models;
using System.Drawing; // esse é só pra encurtar o uso de uma classe de ícone
using System.Reflection;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        try
        {
            if (File.Exists(Settings.AppJsonResponseFile))
            {
                try
                {
                    File.Delete(Settings.AppJsonResponseFile);
                }
                catch (Exception)
                {
                    WriteResponse(new GCSResponse() { Success = false, Message = "Falha ao deletar o arquivo response.json", ErrorCode = ListOfErrorCodes.E149381 });
                    return;
                }
            }

            if (!File.Exists(Settings.AppJsonRequestFile))
            {
                WriteResponse(new GCSResponse() { Success = false, Message = "O arquivo request.json não existe", ErrorCode = ListOfErrorCodes.E114198 });
                return;
            }

            var requestJson = File.ReadAllText(Settings.AppJsonRequestFile, Encoding.UTF8);

            if (string.IsNullOrEmpty(requestJson))
            {
                WriteResponse(new GCSResponse() { Success = false, Message = "O arquivo request.json é vazio ou nulo", ErrorCode = ListOfErrorCodes.E110003 });
                return;
            }

            requestJson = requestJson.Replace("\"true\"", "true", StringComparison.OrdinalIgnoreCase)
                                     .Replace("\"false\"", "false", StringComparison.OrdinalIgnoreCase);

            #region TRAYICON
            //NotifyIcon trayIcon = new();
            Settings.trayIcon.Text = "GCScript Automate";
            Settings.trayIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            Settings.trayIcon.ContextMenuStrip = new();
            Settings.trayIcon.Click += new EventHandler(CloseApp_Click);
            Settings.trayIcon.DoubleClick += new EventHandler(CloseApp_Click);
            //Settings.trayIcon.ContextMenuStrip.Items.Add("Sair").Click += new System.EventHandler(CloseApp_Click);
            Settings.trayIcon.Visible = true;
            #endregion

            if (args.Length == 1)
            {
                string function = args[0].Trim();

                if (function == "Login")
                {
                    var model = JsonSerializer.Deserialize<LoginModel>(requestJson);
                    Login task = new();
                    task.Start(model);
                    return;
                }
                else if (function == "PesquisarProcesso")
                {
                    var model = JsonSerializer.Deserialize<PesquisarProcessoModel>(requestJson);
                    PesquisarProcesso task = new();
                    task.Start(model);
                    return;
                }
                else if (function == "IrParaCadastroDeAndamentoProcessual")
                {
                    WriteResponse(IrParaCadastroDeAndamentoProcessual.Start());
                    return;
                }
                else if (function == "CadastroDeAndamentoProcessual")
                {
                    var model = JsonSerializer.Deserialize<CadastroDeAndamentoProcessualModel>(requestJson);
                    CadastroDeAndamentoProcessual task = new();
                    task.Start(model);
                    return;
                }
                else if (function == "IrParaIncluindoNovoCompromisso")
                {
                    WriteResponse(IrParaIncluindoNovoCompromisso.Start());
                    return;
                }
                else if (function == "IncluindoNovoCompromisso")
                {
                    Clipboard.SetText(requestJson);
                    var model = JsonSerializer.Deserialize<IncluindoNovoCompromissoModel>(requestJson);

                    WriteResponse(IncluindoNovoCompromisso.Start(model));
                    return;
                }
                else
                {
                    WriteResponse(new GCSResponse() { Success = false, Message = $"Função inválida!", ErrorCode = ListOfErrorCodes.E111983 });
                    return;
                }
            }
            else
            {
                //Tests.Test();


                #region Login
                //LoginModel model = new() { Username = "DTI", Password = "Fix@2021" };
                //Login task = new();
                //task.Start(model);
                #endregion

                #region PesquisarProcesso
                var model = new PesquisarProcessoModel() { PalavraChave = "5000191-81.2022.8.08.0036" };
                PesquisarProcesso task = new();
                task.Start(model);
                #endregion

                #region CadastroDeAndamentoProcessual
                //var model = new CadastroDeAndamentoProcessualModel()
                //{
                //    Data = "19/12/2022",
                //    Tipo = "Publicação",
                //    Subtipo = "",
                //    Descricao = "Descrição de Teste Descrição de Teste Descrição de Teste Descrição de Teste Descrição de Teste Descrição de Teste Descrição de Teste"

                //};
                //WriteResponse(CadastroDeAndamentoProcessual.Start(model));
                //return;
                #endregion

                #region Login
                //WriteResponse(IrParaIncluindoNovoCompromisso.Start());
                //return;
                #endregion

                #region IncluindoNovoCompromisso
                //var model = new IncluindoNovoCompromissoModel()
                //{
                //    Tipo = "PRE MANDADO",
                //    Subtipo = "EMENDA INICIAL",
                //    DtPublicacao = "19/12/2022",
                //    Descricao = "Descrição de Teste Descrição de Teste Descrição de Teste Descrição de Teste Descrição de Teste Descrição de Teste Descrição de Teste",
                //    DiaInteiro = true,
                //};
                //WriteResponse(IncluindoNovoCompromisso.Start(model));
                //return;
                #endregion


            }
        }
        catch (Exception error)
        {
            WriteResponse(new() { Success = false, Message = error.Message, ErrorCode = ListOfErrorCodes.E143857 });
            return;
        }

    }
    private static void CloseApp_Click(object Sender, EventArgs e)
    {
        try
        {
            Application.Exit();
            Environment.Exit(0);
        }
        catch (Exception error)
        {
            MessageBox.Show(error.Message);
        }
    }

    private static void WriteResponse(GCSResponse response)
    {
        try
        {
            if (response is not null)
            {
                GCSResponse finalResponse = new();
                finalResponse.Success = response.Success;
                finalResponse.Message = response.Message;
                finalResponse.ErrorCode = response.ErrorCode;

                if (response.Message is null) { finalResponse.Message = ""; }
                if (response.ErrorCode is null) { finalResponse.ErrorCode = ""; }

                var json = JsonSerializer.Serialize(finalResponse);
                File.WriteAllText(Settings.AppJsonResponseFile, json);

                Settings.trayIcon.Dispose();
                //if (Settings.trayIcon.Visible) { Settings.trayIcon.Visible = false;}

                Application.Exit();
            }
        }
        catch (Exception)
        {
            Settings.trayIcon.Dispose();
            //if (Settings.trayIcon.Visible) { Settings.trayIcon.Visible = false; }
            Application.Exit();
        }
    }
}