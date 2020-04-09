using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using EssentialsAddin.Helpers;
using EssentialsAddin.Lib;
using GLib;
using Gtk;
using MonoDevelop.Core;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.TypeSystem;
using static MonoDevelop.Ide.Gui.Components.LogView;

namespace EssentialsAddin
{

    [System.ComponentModel.ToolboxItem(true)]
    public partial class OutputFilterWidget : Gtk.Bin
    {
        private TextTag debugTag;
        private TextMark endMark;

        LogTextWriter internalLogger = new LogTextWriter();


        //	NotSupportedTextReader inputReader = new NotSupportedTextReader ();
        //	OperationConsole console;

        private TextBuffer buffer => textview1.Buffer;



        public OutputFilterWidget()
        {
            this.Build();
            this.ShowAll();

            Environment.SetEnvironmentVariable("MONODEVELOP_CONSOLE_LOG_USE_COLOUR", "true");

            //internalLogger.TextWritten += WriteConsoleLogText;
            //console = new LogViewProgressConsole (this);

            debugTag = new TextTag("debug")
            {
                Foreground = Styles.InformationForegroundColor.ToHexString(false),
                //LeftMargin= 10
            };
            buffer.TagTable.Add(debugTag);

            endMark = buffer.CreateMark("end-mark", buffer.EndIter, false);
        }

        //void WriteConsoleLogText(string text)
        //{
        //    TextIter it = buffer.EndIter;
        //    buffer.InsertWithTags(ref it, text, debugTag);

        //    it.LineOffset = 0;
        //    buffer.MoveMark(endMark, it);
        //    textview1.ScrollToMark(endMark, 0, false, 0, 0);
        //}

        protected void ClearButton_Clicked(object sender, EventArgs e)
        {
            //var outprogmon = IdeServices.ProgressMonitorManager.GetOutputProgressMonitor("MonoDevelop.Ide.ApplicationOutput", GettextCatalog.GetString("Application Output"), MonoDevelop.Ide.Gui.Stock.MessageLog, false, false);

            // var text = outprogmon.Console.Debug(.ReadToEnd();
            //Debug.WriteLine(text);

            filterEntry.Text = string.Empty;
            return;



            //foreach (var p in IdeApp.Workbench.Pads)
            //{
            //    if (p.Id.Contains("MonoDevelop.Ide.ApplicationOutput"))
            //    {
            //        Debug.WriteLine($"{p}");
            //        var mp = (PadContent)p.Content;
            //        if (mp != null)
            //        {
            //            //{MonoDevelop.Ide.Gui.Components.LogView.LogTextView}
            //            Debug.WriteLine($"{mp}");
            //            var lv = (LogView)mp.Control;
            //            if (lv != null)
            //            {
            //                Debug.WriteLine($"{lv}");

            //                var c = (Container)lv.Children[0];
            //                var tv = (LogTextView)c.Children[0];
            //                var text = tv.Buffer.Text;
            //                textview1.Buffer.Text = text;

            //                //https://stackoverflow.com/questions/27547425/how-do-i-cut-copy-paste-and-select-all-in-a-textview-control

            //                //var start = tv.Buffer.GetIterAtOffset(0);
            //                //var end = tv.Buffer.GetIterAtOffset(0);
            //                //end.ForwardToEnd();
            //                //var clipboard = tv.GetClipboard(Gdk.Selection.Clipboard);
            //                //tv.Buffer.SelectRange(start, end);
            //                //tv.Buffer.CopyClipboard(clipboard);


            //                var monitor = (LogViewProgressMonitor)lv.GetProgressMonitor();
            //                //monitor.ErrorLog.
            //                //monitor.Console.In.
            //                //MonoDevelop.Core.Execution.
            //                //monitor.
            //                //MonoDevelop.Core.LoggingService.

            //            }


            //        }
            //    }
            //}
        }


        private TextIter _lastEndIter;
        private string _text;

        private string CopyConsoleOutput()
        {
            try
            {
                foreach (var p in IdeApp.Workbench.Pads)
                {
                    if (p.Id.Contains("MonoDevelop.Ide.ApplicationOutput"))
                    {
                        Debug.WriteLine($"{p}");
                        if (p.Content is PadContent pc)
                        {
                            if (pc != null)
                            {
                                var lv = (LogView)pc.Control;
                                if (lv != null)
                                {
                                    var c = (Container)lv.Children[0];
                                    var tv = (LogTextView)c.Children[0];

                                    //https://stackoverflow.com/questions/27547425/how-do-i-cut-copy-paste-and-select-all-in-a-textview-control

                                    var sb = tv.Buffer.GetDebugTextFromBuffer();
                                    _text = sb.ToString();
                                    _lastEndIter = tv.Buffer.EndIter;

                                    return sb.ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return string.Empty;
        }

        //private StringBuilder GetDebugTextFromBuffer(TextBuffer buffer)
        //{
        //    TextTag debugTag = new TextTag("TheDebugTagToBe");
        //    buffer.TagTable.Foreach(t => { if (t.Name == "debug") { debugTag = t; } });

        //    var sb = new StringBuilder();
        //    var tagIter = buffer.StartIter;
        //    while (true)
        //    {
        //        if (!tagIter.IsStart || !tagIter.BeginsTag(debugTag))
        //        {
        //            if (!tagIter.ForwardToTagToggle(debugTag))
        //                break;
        //        }

        //        var startIter = tagIter;
        //        var stopIter = buffer.EndIter;
        //        if (tagIter.ForwardToTagToggle(debugTag))
        //            stopIter = tagIter;

        //        sb.AppendLine(buffer.GetText(startIter, stopIter, false));

        //        if (tagIter.IsEnd)
        //            break;
        //    }
        //    return sb;
        //}

        protected void FilterEntry_Changed(object sender, EventArgs e)
        {
            EssentialProperties.ConsoleFilter = filterEntry.Text;
            FilterConsoleOuput();
        }

        private void FilterConsoleOuput()
        {
            var filter = EssentialProperties.ConsoleFilterArray;

            var text = CopyConsoleOutput();
            var lines = text.Split("\n".ToCharArray());
            var result = new StringBuilder();

            if (string.IsNullOrEmpty(EssentialProperties.ConsoleFilter))
            {
                result.Append(text);
            }
            else
            {
                foreach (string line in lines)
                {
                    var searchLine = line.ToLowerInvariant();
                    foreach (var key in filter)
                    {
                        if (searchLine.Contains(key.ToLowerInvariant()))
                        {
                            result.AppendLine(line);
                            break;
                        }
                    }
                }
            }
            textview1.Buffer.Clear();

            textview1.ModifyFont(IdeApp.Preferences.CustomOutputPadFont ?? IdeServices.FontService.MonospaceFont);

            TextIter it = buffer.EndIter;
            buffer.InsertWithTags(ref it, result.ToString(), debugTag);

            it.LineOffset = 0;
            buffer.MoveMark(endMark, it);
            textview1.ScrollToMark(endMark, 0, false, 0, 0);
        }

        protected void FilterLabel_ButtonPress(object o, ButtonPressEventArgs args)
        {
        }

        protected void UpdateButton_Clicked(object sender, EventArgs e)
        {
            FilterConsoleOuput();
        }
    }
}