using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GCScript_Automate.Functions
{
    public static class SendResponse
    {
        public static void Send(GCSResponse response)
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
                    Environment.Exit(0);
                }
            }
            catch (Exception)
            {
                Settings.trayIcon.Dispose();
                //if (Settings.trayIcon.Visible) { Settings.trayIcon.Visible = false; }
                Environment.Exit(0);
                Application.Exit();
            }
        }
    }
}
