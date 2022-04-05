using EssentialsAddin.Helpers;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Gui.Pads;
using MonoDevelop.Projects;

namespace EssentialsAddin.SolutionFilter
{
    public class FileNodeContextMenuPinCommandHandler : CommandHandler
    {
        protected override void Update(CommandInfo info)
        {
            base.Update(info);
            info.Enabled = false;
            if (IdeApp.ProjectOperations.CurrentSelectedItem is ProjectFile projectFile)
            {
                info.Enabled = true;
                var isPinned = EssentialProperties.IsPinned(projectFile);
                info.Text = isPinned ? "Pinned on Solution Tree" : "Pin on Solution Tree";
                info.Checked = isPinned;
            }
        }

        protected override void Run()
        {
            if (IdeApp.ProjectOperations.CurrentSelectedItem is ProjectFile projectFile
                && projectFile.Subtype == Subtype.Code)
            {

                var isPinned = EssentialProperties.IsPinned(projectFile);
                if (isPinned)
                    EssentialProperties.RemovePinnedDocument(projectFile);
                else
                    EssentialProperties.AddPinnedDocument(projectFile);


                var pad = (SolutionPad)IdeApp.Workbench.Pads.SolutionPad.Content;
                if (pad == null)
                    return;

                pad.RefreshSelectedNode();
            }
        }
    }
}
