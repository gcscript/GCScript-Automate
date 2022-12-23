using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCScript_Automate
{
    public static class Settings
    {
        public static NotifyIcon trayIcon = new();
        public static string AppMyDocumentsDirectory { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GCScript Automate");
        public static string AppJsonRequestFile { get; } = $"{AppMyDocumentsDirectory}\\request.json";
        public static string AppJsonResponseFile { get; } = $"{AppMyDocumentsDirectory}\\response.json";
        public static int DefaultSleep { get; } = 1000; // Milliseconds
        public static int CurrentAttempt { get; set; } = 0;
        public static int DefaultMaxAttempts { get; } = 10;
        public static int DelayInAttempts { get; } = 1000; // Milliseconds
        public static int WindowWaitTimeout { get; } = 5; // Seconds
        public static int ControlWaitTimeout { get; } = 5; // Seconds
        public static int ComboBoxAttempts { get; set; } = 0;
        public static int ComboBoxMaxAttempts { get; } = 3;

        // LIBRE AUTOMATE
        public static int LA_WinNegativeWait { get; } = -10; // Seconds
        public static int LA_NegativeWait03 { get; } = -3; // Seconds
        public static int LA_NegativeWait05 { get; } = -5; // Seconds
        public static int LA_NegativeWait10 { get; } = -10; // Seconds
        public static int LA_NegativeWait15 { get; } = -15; // Seconds
        public static int LA_NegativeWait20 { get; } = -20; // Seconds
        public static int LA_NegativeWait30 { get; } = -30; // Seconds
        public static int LA_NegativeWait60 { get; } = -60; // Seconds
        public static int LA_NegativeWait90 { get; } = -90; // Seconds

        public static int LA_PositiveWait01 { get; } = 1; // Seconds
        public static int LA_PositiveWait03 { get; } = 3; // Seconds
        public static int LA_PositiveWait05 { get; } = 5; // Seconds
        public static int LA_PositiveWait10 { get; } = 10; // Seconds
        public static int LA_PositiveWait15 { get; } = 15; // Seconds
        public static int LA_PositiveWait20 { get; } = 20; // Seconds
        public static int LA_PositiveWait30 { get; } = 30; // Seconds

        // CONTROLE DE FLUXO

        public static bool NextStep { get; set; } = false;
        public static string LastError { get; set; } = "";


    }
}
