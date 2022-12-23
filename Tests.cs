using Au;
using Au.Types;
using GCScript_Automate.Functions;
using Microsoft.VisualBasic.ApplicationServices;
using System.Drawing;
using System.Net;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Security.Policy;
using System.Text;
using System.Xml.Linq;
using static Au.screen;
using static GCScript_Automate.Models.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace GCScript_Automate
{
    public static class Tests
    {
        
        public static void Test()
        {
            string win1Name = "Cadastro de andamento processual";
            string win1Class = "TfrmProcAcompIncAlt";
            wnd win1;
            elm? txtTipo; // ch2 fi5 la fi la fi ch2 fi3
            elm? txtSubtipo; // ch2 fi5 la fi la fi ch13 fi3
            string mdlTipo = "PRAZO DACASA - ATIVA";
            string mdlSubtipo = "AGRAVO INTERNO";

            win1 = wnd.find(Settings.LA_WinNegativeWait, win1Name, win1Class);
            //if (win1.Is0) { Settings.LastError = $"A janela {win1Name} não foi encontrada!"; continue; }

            txtTipo = win1.Elm["CLIENT", win1.Name, navig: "ch2 fi5 la fi la fi ch2 fi3"].Find(Settings.LA_NegativeWait10);
            txtSubtipo = win1.Elm["CLIENT", win1.Name, navig: "ch2 fi5 la fi la fi ch13 fi3"].Find(Settings.LA_NegativeWait10);
            //if (txtTipo is null) { Settings.LastError = $"O campo Tipo não foi encontrado!"; continue; }
            //FunctionsLibreAutomate.ElementSetTextByClipboard(txtTipo, "???", true, false, true, false, true);

            FunctionsLibreAutomate.ElementSetTextComboboxMode1(txtTipo, mdlTipo);
            wait.s(5);
            FunctionsLibreAutomate.ElementSetTextComboboxMode1(txtSubtipo, mdlSubtipo);

        }

    }
}
