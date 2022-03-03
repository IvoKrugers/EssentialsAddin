using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EssentialsAddin.Helpers;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities.Internal;
using Mono.Addins;
using Mono.Addins.Description;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;

namespace EssentialsAddin.CommandHandlers
{
    public class TestHandler : CommandHandler
    {
        protected override void Update(CommandInfo info)
        {
            info.Enabled = true;
        }

        protected override void Run()
        {
            System.Diagnostics.Debug.WriteLine(GitHub.GitHelper.GetCurrentBranch());
            System.Diagnostics.Debug.WriteLine(GitHub.GitHelper.GetLocalBranches().Join(", "));

            //EssentialProperties.PurgeProperties()

            System.Diagnostics.Debug.WriteLine(PropertyService.Instance.GetAllKeys().Join(", "));

            var pad = IdeApp.Workbench.GetPad<SolutionFilterPad>();
            if (pad != null)
            {
                ((SolutionFilterPad)pad.Content).Initialize();
            }

            if (IdeApp.Workbench.ActiveDocument != null)
            {
                //var textBuffer = IdeApp.Workbench.ActiveDocument.GetContent<ITextBuffer>();
                //var textEditor = textBuffer.GetTextEditor();
                var textView = IdeApp.Workbench.ActiveDocument.GetContent<ITextView>();
                var caretPosition = textView.Caret.Position;
                //textBuffer.Insert(caretPosition.BufferPosition.Position, DateTime.Now.ToShortDateString());
                var ln = caretPosition.BufferPosition.GetContainingLine().LineNumber;
                
                //var offset = textEditor.Length;

                //var ln = textBuffer.GetTextEditor().OffsetToLineNumber(offset);
            }

            ExtensionPoints();
        }

        public void ExtensionPoints()
        {
            var addins = AddinManager.Registry.GetAddins();

            var points = GatherExtensionPoints(addins);

            Debug.WriteLine($"ExtensionPoint Count: {points.Count}");

            foreach (var point in points)
            {
                Debug.WriteLine($"\tpoint: {point}");
            }
        }

        private List<string> GatherExtensionPoints(Addin[] addins)
        {
            var points = new List<string>();

            foreach (var addin in addins)
            {
                foreach (ExtensionPoint extensionPoint in addin.Description.ExtensionPoints)
                {
                    points.Add(extensionPoint.Path);
                }
            }
            return points.OrderBy(p => p).ToList();
        }

        
    }
}
