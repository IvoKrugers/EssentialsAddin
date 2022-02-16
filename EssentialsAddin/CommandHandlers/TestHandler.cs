using System;
using EssentialsAddin.Helpers;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities.Internal;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;

namespace EssentialsAddin.CommandHandlers
{
    public class TestHandler : CommandHandler
    {
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
                var textBuffer = IdeApp.Workbench.ActiveDocument.GetContent<ITextBuffer>();
                var textView = IdeApp.Workbench.ActiveDocument.GetContent<ITextView>();
                var caretPosition = textView.Caret.Position;
                textBuffer.Insert(caretPosition.BufferPosition.Position, DateTime.Now.ToShortDateString());
            }
        }

        protected override void Update(CommandInfo info)
        {
            info.Enabled = true;

            if (IdeApp.Workbench.ActiveDocument != null)
            {
                var textBuffer = IdeApp.Workbench.ActiveDocument.GetContent<ITextBuffer>();
                if (textBuffer != null && textBuffer.AsTextContainer() is SourceTextContainer container)
                {
                    var document = container.GetTextBuffer();
                    if (document != null)
                    {
                        info.Enabled = true;
                    }
                }
            }
        }
    }
}
