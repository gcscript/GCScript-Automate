using Au;
using Au.Types;
using GCScript_Automate.Models;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using static GCScript_Automate.Models.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace GCScript_Automate.Functions
{
    public static class FunctionsLibreAutomate
    {
        public static GCSResponse ElementSetText(ELASetTextMode mode, elm element, string elementName, string elementValue,
                                            bool literalText = true, bool clickBefore = false, bool focusBefore = false,
                                            bool selectBefore = false, bool ctrlABefore = false)
        {
            try
            {
                if (literalText) { elementValue = $"!{elementValue}"; }
                if (clickBefore) { element.MouseClick(); }
                if (focusBefore) { element.Focus(); }
                if (selectBefore) { element.Select(); }

                if (mode == ELASetTextMode.SendKeys)
                {
                    if (ctrlABefore) { element.SendKeys("Ctrl+A"); }
                    element.SendKeys(elementValue);
                }
                else
                {
                    if (ctrlABefore) { keys.send("Ctrl+A"); }
                    keys.send(elementValue);
                }
                element.WaitFor(Settings.LA_NegativeWait10, o => o.WndContainer.ControlText.Trim().Eq(elementValue.Trim(), true));

                string currentControlValue = element.WndContainer.ControlText;

                if (currentControlValue.Trim() != elementValue.Trim())
                {
                    return new()
                    {
                        Success = false,
                        Message = $"Erro ao definir o valor em {elementName}!",
                        ErrorCode = ListOfErrorCodes.E125669
                    };
                };

                return new() { Success = true };
            }
            catch (Exception error)
            {
                return new() { Success = false, Message = error.Message, ErrorCode = ListOfErrorCodes.E121977, };
            }
        }

        public static bool ElementSetValue(SetValueModel sv)
        {
            try
            {
                sv.ElementValue = sv.ElementValue.Trim();

                if (sv.SetFocusAndSelect) { SetFocusAndSelectElement(sv.Element); }
                if (sv.ClearContent) { ClearElementContents(sv.Element); }
                if (sv.ClickBefore) { sv.Element.MouseClick(); }

                if (sv.Mode == ESetValueMode.Clipboard) { clipboard.clear(); clipboard.paste(sv.ElementValue); }
                else if (sv.Mode == ESetValueMode.SendKeys) { sv.Element.SendKeys($"!{sv.ElementValue}"); }
                else if (sv.Mode == ESetValueMode.SendKeys) { keys.send($"{sv.ElementValue}"); }
                else { return false; }

                if (sv.CheckIfItWasSuccessful)
                {
                    sv.Element.WaitFor(Settings.LA_NegativeWait05, o => Tools.OnlyLettersAndNumbers(o.WndContainer.ControlText).Eq(Tools.OnlyLettersAndNumbers(sv.ElementValue)));
                    string currentControlValue = Tools.OnlyLettersAndNumbers(sv.Element.WndContainer.ControlText);
                    if (currentControlValue != Tools.OnlyLettersAndNumbers(sv.ElementValue)) { return false; };
                }

                if (sv.HitEnterAfter) { sv.Element.SendKeys("Enter"); }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ClickOnElement(elm element, bool doubleClick = false, bool waitForVisible = true, int wait = -10)
        {
            try
            {
                if (element is null) { return false; }

                if (waitForVisible)
                {
                    element.WaitFor(wait, x => x.IsInvisible == false);
                    if (!element.IsInvisible)
                    {
                        try
                        {
                            if (doubleClick) { element.MouseClickD(); } else { element.MouseClick(); }
                        }
                        catch (Exception) { return false; }
                    }
                    else { return false; }
                }
                else
                {
                    try
                    {
                        if (doubleClick) { element.MouseClickD(); } else { element.MouseClick(); }
                    }
                    catch (Exception) { return false; }
                }

                return true;
            }
            catch (Exception) { return false; }
        }

        public static void ClearElementContents(elm element)
        {
            try
            {
                try { element.SendKeys("Ctrl+A", "Del"); } catch (Exception) { }
                if (string.IsNullOrEmpty(element.Value)) { return; }

                try { element.SendKeys("Ctrl+A", "Back"); } catch (Exception) { }
                if (string.IsNullOrEmpty(element.Value)) { return; }

                try { element.SendKeys("Ctrl+Home", "Ctrl+Shift+End", "Del"); } catch (Exception) { }
                if (string.IsNullOrEmpty(element.Value)) { return; }

                try { element.SendKeys("Ctrl+Home", "Ctrl+Shift+End", "Back"); } catch (Exception) { }
                if (string.IsNullOrEmpty(element.Value)) { return; }

                try { element.Value = ""; } catch (Exception) { }
            }
            catch (Exception) { }
        }

        public static int SetFocusAndSelectElement(elm element)
        {
            // 0 = Error
            // 1 = Focused & Selected
            // 2 = Focused
            // 3 = Selected
            try
            {
                try { element.Focus(true); } catch (Exception) { }
                if (element.IsFocused && element.IsSelected) { return 1; }

                try { element.Focus(); } catch (Exception) { }
                if (element.IsFocused) { return 2; }

                try { element.Select(); } catch (Exception) { }
                if (element.IsSelected) { return 3; }

                return 0;
            }
            catch (Exception) { return 0; }
        }

        public static bool ApplicationIsStuckMode1(elm element)
        {
            for (int b = 0; b < 3; b++)
            {
                element.SendKeys("Ctrl+F2");
                var wTemp = wnd.find(Settings.LA_NegativeWait30, null, "TfrmManutTarefa");
                if (wTemp.Is0) { continue; }
                wTemp.Close();
                wTemp.WaitForClosed(Settings.LA_NegativeWait30);
                wTemp = wnd.find(Settings.LA_NegativeWait03, null, "TfrmManutTarefa");
                if (wTemp.Is0) { return false; }
            }
            return true;
        }

        public static void SetValueInComboBox(elm element, string elementName, string elementValue)
        {
            try
            {
                Settings.NextStep = false;
                for (int i = 0; i < 2; i++)
                {
                    if (string.IsNullOrEmpty(elementValue)) { break; }
                    if (ApplicationIsStuckMode1(element)) { continue; } // VERIFICANDO SE A APLICAÇÃO ESTÁ TRAVADA

                    if (i > 0) { wait.s(Settings.LA_PositiveWait03); }

                    if (!ElementSetTextComboboxMode1(element, elementValue))
                    {
                        Settings.LastError = $"{elementName} inválido!"; continue;
                    }

                    if (ApplicationIsStuckMode1(element)) { continue; } // VERIFICANDO SE A APLICAÇÃO ESTÁ TRAVADA

                    if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) != Tools.OnlyLettersAndNumbers(elementValue))
                    {
                        Settings.LastError = $"Falha ao definir {elementName}!"; continue;
                    }

                    Settings.NextStep = true; break;
                }
                if (!Settings.NextStep) { SendResponse.Send(new() { Success = false, Message = Settings.LastError, ErrorCode = ListOfErrorCodes.E151744 }); }
            }
            catch (Exception error)
            {
                SendResponse.Send(new() { Success = false, Message = error.Message, ErrorCode = ListOfErrorCodes.E157033 });
            }
        }

        public static void SetValueInTextBox(SetValueModel svm, int attempts = 0)
        {
            try
            {
                if (attempts == 0) { attempts = Settings.DefaultMaxAttempts; }

                Settings.NextStep = false;
                for (int i = 0; i < attempts; i++)
                {
                    if (svm.ElementValue is null) { svm.ElementValue = ""; }

                    if (i > 0) { wait.s(Settings.LA_PositiveWait10); }

                    if (!ElementSetValue(svm))
                    {
                        Settings.LastError = $"Falha ao preencher o campo {svm.ElementName}!"; continue;
                    }

                    Settings.NextStep = true; break;
                }
                if (!Settings.NextStep) { SendResponse.Send(new() { Success = false, Message = Settings.LastError, ErrorCode = ListOfErrorCodes.E158190 }); }
            }
            catch (Exception error)
            {
                SendResponse.Send(new() { Success = false, Message = error.Message, ErrorCode = ListOfErrorCodes.E157829 });
            }
        }

        public static bool ElementSetTextComboboxMode1(elm element, string elementValue)
        {
            try
            {
                elementValue = elementValue.Trim();
                //wait.s(Settings.LA_PositiveWait03);
                if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU

                #region LIMPANDO COMBOBOX
                for (int a = 0; a < 20; a++)
                {
                    element.Value = "???"; element.SendKeys("Enter");
                    wait.s(Settings.LA_PositiveWait01);
                    CloseComboBoxPopup();
                    if (element.WndContainer.ControlText != "")
                    {
                        wait.s(Settings.LA_PositiveWait03);
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                #endregion

                #region PREENCHENDO COMBOBOX
                for (int b = 0; b < 5; b++)
                {
                    element.Value = elementValue; element.SendKeys("Enter");
                    wait.s(Settings.LA_PositiveWait01);
                    CloseComboBoxPopup();

                    if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU

                    wait.s(Settings.LA_PositiveWait01);

                    while (true)
                    {
                        element.MouseClick(); element.SendKeys("Up");
                        wait.s(Settings.LA_PositiveWait01);
                        CloseComboBoxPopup();

                        if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU
                        if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == "") { break; }
                    }
                }
                #endregion

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ElementSetTextComboboxMode2(elm element, string elementValue)
        {
            try
            {
                elementValue = elementValue.Trim();
                wait.s(Settings.LA_PositiveWait03);
                if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU

                for (int i = 0; i < Settings.DefaultMaxAttempts; i++)
                {
                    #region LIMPANDO COMBOBOX
                    for (int a = 0; a < 20; a++)
                    {
                        element.Value = "???"; element.SendKeys("Enter");
                        wait.s(Settings.LA_PositiveWait01);
                        CloseComboBoxPopup();
                        if (element.WndContainer.ControlText != "")
                        {
                            wait.s(Settings.LA_PositiveWait03);
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion

                    #region PREENCHENDO COMBOBOX
                    for (int b = 0; b < 5; b++)
                    {
                        element.Value = elementValue; element.SendKeys("Enter");
                        wait.s(Settings.LA_PositiveWait01);
                        CloseComboBoxPopup();

                        if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU

                        wait.s(Settings.LA_PositiveWait01);

                        while (true)
                        {
                            element.MouseClick(); element.SendKeys("Up");
                            wait.s(Settings.LA_PositiveWait01);
                            CloseComboBoxPopup();

                            if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU
                            if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == "") { break; }
                        }

                    }
                    #endregion

                    //for (int c = 0; c < 50; c++)
                    //{
                    //    element.MouseClick(); element.SendKeys("Up");
                    //}

                    wait.s(Settings.LA_PositiveWait03);
                    var elementSplitValue = Regex.Split(elementValue, @"\s+")[0];

                    //#region LIMPANDO COMBOBOX
                    //for (int a = 0; a < 5; a++)
                    //{
                    //    element.Value = "???"; element.SendKeys("Enter");
                    //    wait.s(Settings.LA_PositiveWait01);
                    //    CloseComboBoxPopup();
                    //    if (element.WndContainer.ControlText != "")
                    //    {
                    //        wait.s(Settings.LA_PositiveWait10);
                    //        continue;
                    //    }
                    //    else
                    //    {
                    //        break;
                    //    }
                    //}
                    //#endregion

                    #region PREENCHENDO COMBOBOX
                    element.Value = elementSplitValue; element.SendKeys("Enter");
                    wait.s(Settings.LA_PositiveWait01);
                    CloseComboBoxPopup();
                    if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU

                    wait.s(Settings.LA_PositiveWait01);

                    while (true)
                    {
                        element.MouseClick(); element.SendKeys("Up");
                        wait.s(Settings.LA_PositiveWait01);
                        CloseComboBoxPopup();

                        if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU
                        if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == "") { break; }
                        if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementSplitValue)) { break; }
                    }
                    #endregion

                    string lastItem = "?Last Item?";
                    while (true)
                    {
                        wait.s(Settings.LA_PositiveWait01);
                        if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU
                        if (element.WndContainer.ControlText.Trim() == lastItem.Trim()) // CHEGOU AO FIM DO COMBOBOX
                        {
                            break;
                        }
                        else
                        {
                            lastItem = element.WndContainer.ControlText;
                        }
                        element.MouseClick(); element.SendKeys("Down");
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ElementSetTextComboboxBackup_2022_12_13(elm element, string elementValue)
        {
            try
            {
                elementValue = elementValue.Trim();
                wait.s(Settings.LA_PositiveWait03);
                if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU

                for (int i = 0; i < Settings.DefaultMaxAttempts; i++)
                {
                    #region LIMPANDO COMBOBOX
                    for (int a = 0; a < 20; a++)
                    {
                        element.Value = "???"; element.SendKeys("Enter");
                        wait.s(Settings.LA_PositiveWait01);
                        CloseComboBoxPopup();
                        if (element.WndContainer.ControlText != "")
                        {
                            wait.s(Settings.LA_PositiveWait03);
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion

                    #region PREENCHENDO COMBOBOX
                    for (int b = 0; b < 5; b++)
                    {
                        element.Value = elementValue; element.SendKeys("Enter");
                        wait.s(Settings.LA_PositiveWait01);
                        CloseComboBoxPopup();

                        if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU

                        wait.s(Settings.LA_PositiveWait01);

                        while (true)
                        {
                            element.MouseClick(); element.SendKeys("Up");
                            wait.s(Settings.LA_PositiveWait01);
                            CloseComboBoxPopup();

                            if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU
                            if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == "") { break; }
                        }

                    }
                    #endregion

                    //for (int c = 0; c < 50; c++)
                    //{
                    //    element.MouseClick(); element.SendKeys("Up");
                    //}

                    wait.s(Settings.LA_PositiveWait03);
                    var elementSplitValue = Regex.Split(elementValue, @"\s+")[0];

                    //#region LIMPANDO COMBOBOX
                    //for (int a = 0; a < 5; a++)
                    //{
                    //    element.Value = "???"; element.SendKeys("Enter");
                    //    wait.s(Settings.LA_PositiveWait01);
                    //    CloseComboBoxPopup();
                    //    if (element.WndContainer.ControlText != "")
                    //    {
                    //        wait.s(Settings.LA_PositiveWait10);
                    //        continue;
                    //    }
                    //    else
                    //    {
                    //        break;
                    //    }
                    //}
                    //#endregion

                    #region PREENCHENDO COMBOBOX
                    element.Value = elementSplitValue; element.SendKeys("Enter");
                    wait.s(Settings.LA_PositiveWait01);
                    CloseComboBoxPopup();
                    if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU

                    wait.s(Settings.LA_PositiveWait01);

                    while (true)
                    {
                        element.MouseClick(); element.SendKeys("Up");
                        wait.s(Settings.LA_PositiveWait01);
                        CloseComboBoxPopup();

                        if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU
                        if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == "") { break; }
                        if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementSplitValue)) { break; }
                    }
                    #endregion

                    string lastItem = "?Last Item?";
                    while (true)
                    {
                        wait.s(Settings.LA_PositiveWait01);
                        if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU
                        if (element.WndContainer.ControlText.Trim() == lastItem.Trim()) // CHEGOU AO FIM DO COMBOBOX
                        {
                            break;
                        }
                        else
                        {
                            lastItem = element.WndContainer.ControlText;
                        }
                        element.MouseClick(); element.SendKeys("Down");
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ElementSetTextComboboxBackup(elm element, string elementValue)
        {
            try
            {
                elementValue = elementValue.Trim();
                wait.s(Settings.LA_PositiveWait03);
                if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU

                for (int i = 0; i < Settings.DefaultMaxAttempts; i++)
                {
                    #region LIMPANDO COMBOBOX
                    for (int a = 0; a < 5; a++)
                    {
                        element.Value = "???"; element.SendKeys("Enter");
                        wait.s(Settings.LA_PositiveWait01);
                        CloseComboBoxPopup();
                        if (element.WndContainer.ControlText != "")
                        {
                            wait.s(Settings.LA_PositiveWait10);
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion

                    #region PREENCHENDO COMBOBOX
                    for (int b = 0; b < 5; b++)
                    {
                        element.Value = elementValue; element.SendKeys("Enter");
                        wait.s(Settings.LA_PositiveWait01);
                        if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU
                        CloseComboBoxPopup();

                        wait.s(Settings.LA_PositiveWait01);

                        while (true)
                        {
                            element.MouseClick(); element.SendKeys("Up");
                            wait.s(Settings.LA_PositiveWait01);
                            CloseComboBoxPopup();

                            if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU
                            if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == "") { break; }
                        }

                    }
                    #endregion

                    //for (int c = 0; c < 50; c++)
                    //{
                    //    element.MouseClick(); element.SendKeys("Up");
                    //}

                    wait.s(Settings.LA_PositiveWait03);
                    var elementSplitValue = Regex.Split(elementValue, @"\s+")[0];

                    #region LIMPANDO COMBOBOX
                    for (int a = 0; a < 5; a++)
                    {
                        element.Value = "???"; element.SendKeys("Enter");
                        wait.s(Settings.LA_PositiveWait01);
                        CloseComboBoxPopup();
                        if (element.WndContainer.ControlText != "")
                        {
                            wait.s(Settings.LA_PositiveWait10);
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion

                    #region PREENCHENDO COMBOBOX
                    for (int b = 0; b < 5; b++)
                    {
                        element.Value = elementSplitValue; element.SendKeys("Enter");
                        wait.s(Settings.LA_PositiveWait01);
                        if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU
                        CloseComboBoxPopup();

                        wait.s(Settings.LA_PositiveWait01);

                        while (true)
                        {
                            element.MouseClick(); element.SendKeys("Up");
                            wait.s(Settings.LA_PositiveWait01);
                            CloseComboBoxPopup();

                            if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU
                            if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == "") { break; }
                            if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementSplitValue)) { break; }
                        }
                    }
                    #endregion

                    string lastItem = "?Last Item?";
                    while (true)
                    {
                        wait.s(Settings.LA_PositiveWait01);
                        if (Tools.OnlyLettersAndNumbers(element.WndContainer.ControlText) == Tools.OnlyLettersAndNumbers(elementValue)) { return true; } // ACHOU
                        if (element.WndContainer.ControlText.Trim() == lastItem.Trim()) // CHEGOU AO FIM DO COMBOBOX
                        {
                            break;
                        }
                        else
                        {
                            lastItem = element.WndContainer.ControlText;
                        }
                        element.MouseClick(); element.SendKeys("Down");
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void CloseComboBoxPopup()
        {
            try
            {
                wnd cmbPopup = wnd.find(Settings.LA_NegativeWait10, "", "TcxComboBoxPopupWindow", "CProc.exe", WFlags.HiddenToo);
                if (!cmbPopup.Is0)
                {
                    cmbPopup.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        public static bool CloseWinTiposSubtiposCompromissosTarefas()
        {
            try
            {
                var win = wnd.find(Settings.LA_NegativeWait10, null, "TfrmManutTarefa");
                if (!win.Is0) // SE A JANELA EXISTIR
                {
                    win.Close();
                    win.WaitForClosed(Settings.LA_NegativeWait30);

                    var win2 = wnd.find(Settings.LA_NegativeWait05, null, "TfrmManutTarefa");
                    if (!win2.Is0) // SE A JANELA EXISTIR
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool OpenWinTiposSubtiposCompromissosTarefas(elm element)
        {
            try
            {
                element.SendKeys("Ctrl+F2");
                var win = wnd.find(Settings.LA_NegativeWait30, null, "TfrmManutTarefa");
                if (win.Is0) { return false; } // SE A JANELA NAO EXISTIR
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static (bool Success, wnd Window) FindWindow(string winName, string winClass, int winWait = 0)
        {
            if (winWait == 0) { winWait = Settings.WindowWaitTimeout; }
            if (winWait > 0) { winWait *= -1; }
            wnd w = wnd.find(winWait, winName, winClass);

            if (w.Is0)
            {
                return (false, w);
            }
            else
            {
                return (true, w);
            }
        }

        public static (bool Success, elm? Element) FindElementByNavigate(wnd window, string roleBaseElement, string nameBaseElement, string navig, int elmWait = 0)
        {
            if (elmWait == 0) { elmWait = Settings.ControlWaitTimeout; }
            if (elmWait > 0) { elmWait *= -1; }

            elm e = window.Elm[roleBaseElement, nameBaseElement, navig: navig].Find(elmWait);

            if (e is null)
            {
                return (false, e);
            }
            else
            {
                return (true, e);
            }
        }
    }
}
