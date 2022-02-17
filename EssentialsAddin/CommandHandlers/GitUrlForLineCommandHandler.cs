using System.Diagnostics;
using EssentialsAddin.GitHub;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;

namespace EssentialsAddin.CommandHandlers
{
    public class GitUrlForLineCommandHandler : CommandHandler
    {
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

        protected override void Run()
        {
            if (IdeApp.Workbench.ActiveDocument != null)
            {
                var url = GitHelper.GetUrl();
                Debug.WriteLine(url);
                if (url != null)
                {
                    // DesktopService.ShowUrl(url);

                    Xwt.Clipboard.SetText(url);
                    IdeApp.Workbench.StatusBar.ShowMessage("Git Url successfully copied to clipboard");
                }
            }
        }
    }
}
