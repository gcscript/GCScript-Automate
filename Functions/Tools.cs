using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GCScript_Automate.Functions
{
    public static class Tools
    {
        public static string TreatText(string text, bool trim = true, bool toUpper = true, bool removeAccents = true, bool removeSpaces = true)
        {
            if (trim)
                text = text.Trim();
            if (toUpper)
                text = text.ToUpper();
            if (removeAccents)
                text = RemoveAccents(text);
            if (removeSpaces)
                text = RemoveSpaces(text);
            return text;
        }

        public static string RemoveAccents(string texto)
        {
            var stringBuilder = new StringBuilder();
            StringBuilder sbReturn = stringBuilder;
            var arrayText = texto.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        public static string RemoveSpaces(string texto)
        {
            texto = Regex.Replace(texto, @"\s", "");
            texto = texto.Trim();

            return texto;
        }

        public static string OnlyLetters(string? text)
        {
            if (text is null) { return ""; }
            text = text.ToUpper();
            text = Regex.Replace(text, @"[^a-zA-Z]", "");
            return text;
        }
        public static string OnlyLettersAndNumbers(string? text)
        {
            if (text is null) { return "";}
            text = text.ToUpper();
            text = Regex.Replace(text, @"[^a-zA-Z0-9]", "");
            return text;
        }
    }
}
