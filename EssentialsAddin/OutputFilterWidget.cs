using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
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

        TextBuffer _sourceBuffer=null;

        private TextBuffer SourceBuffer
            //=> _sourceBuffer ?? (_sourceBuffer = GetSourceBuffer());
            => GetSourceBuffer();

        private TextBuffer DestinationBuffer => textview1.Buffer;

        public OutputFilterWidget()
        {
            this.Build();
            this.ShowAll();

            //Environment.SetEnvironmentVariable("MONODEVELOP_CONSOLE_LOG_USE_COLOUR", "true");

            //internalLogger.TextWritten += WriteConsoleLogText;
            //console = new LogViewProgressConsole (this);

            debugTag = new TextTag("debug")
            {
                Foreground = Styles.InformationForegroundColor.ToHexString(false),
            };
            DestinationBuffer.TagTable.Add(debugTag);

            endMark = DestinationBuffer.CreateMark("end-mark", DestinationBuffer.EndIter, false);

            textview1.ModifyFont(IdeApp.Preferences.CustomOutputPadFont ?? IdeServices.FontService.MonospaceFont);

            RegisterEvents();
        }

        private void RegisterEvents()
        {
            foreach (var p in IdeApp.Workbench.Pads)
            {
                if (p.Id.Contains("MonoDevelop.Ide.ApplicationOutput"))
                {
                    Debug.WriteLine($"{p}");
                    var mp = (PadContent)p.Content;
                    if (mp != null)
                    {
                        var lv = (LogView)mp.Control;
                        if (lv != null)
                        {
                            var c = (Container)lv.Children[0];
                            var tv = (LogTextView)c.Children[0];
                            //tv.Buffer.Changed += Buffer_Changed;
                        }
                    }
                }
            }
        }

        //private void Buffer_Changed(object sender, EventArgs e)
        //{
        //    Task.Run(async () =>
        //    {
        //        await Task.Delay(2000);
        //        FilterConsoleOuput();
        //    });
        //}

        private TextBuffer GetSourceBuffer()
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
                                    return tv.Buffer;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CANNOT FIND SOURCE BUFFER ({ex.Message})");
            }
            return null;
        }


       

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

        protected void FilterEntry_Changed(object sender, EventArgs e)
        {
            EssentialProperties.ConsoleFilter = filterEntry.Text;
            FilterConsoleOuput();
        }

        private void FilterConsoleOuput()
        {
            if (SourceBuffer == null)
                return;

            var filter = EssentialProperties.ConsoleFilterArray;

            var text = SourceBuffer.GetDebugTextFromBuffer();
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
            DestinationBuffer.Clear();

            TextIter it = DestinationBuffer.EndIter;
            DestinationBuffer.InsertWithTags(ref it, result.ToString(), debugTag);

            it.LineOffset = 0;
            DestinationBuffer.MoveMark(endMark, it);
            textview1.ScrollToMark(endMark, 0, false, 0, 0);
        }

        protected void FilterLabel_ButtonPress(object o, ButtonPressEventArgs args)
        {
        }

        protected void UpdateButton_Clicked(object sender, EventArgs e)
        {
            _sourceBuffer = null;
            FilterConsoleOuput();
        }
    }
}