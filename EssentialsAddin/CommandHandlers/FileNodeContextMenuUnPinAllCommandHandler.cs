using EssentialsAddin.Helpers;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Pads;

namespace EssentialsAddin.SolutionFilter
{
    public class FileNodeContextMenuUnPinAllCommandHandler : CommandHandler
    {
        protected override void Update(CommandInfo info)
        {
            base.Update(info);
            info.Enabled = true;
        }

        protected override void Run()
        {
            EssentialProperties.ClearPinnedDocuments();
            var pad = (SolutionFilterPad)IdeApp.Workbench.GetPad<SolutionFilterPad>().Content;
            if (pad == null)
                return;

            Runtime.RunInMainThread(((SolutionFilterWidget)pad.Control).FilterSolutionPad);
           ;
        }
    }
}