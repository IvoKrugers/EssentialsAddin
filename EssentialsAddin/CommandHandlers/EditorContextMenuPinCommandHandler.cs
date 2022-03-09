using System;
using EssentialsAddin.Helpers;
using EssentialsAddin.SolutionFilter;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Pads;
using MonoDevelop.Projects;

namespace EssentialsAddin.CommandHandlers
{
    public class EditorContextMenuPinCommandHandler: FileNodeContextMenuPinCommandHandler
    {
        protected override void Update(CommandInfo info)
        {
            base.Update(info);
            info.Enabled = false;
            if (IdeApp.Workbench.ActiveDocument != null && IdeApp.Workbench.ActiveDocument.IsFile)
            {
                info.Enabled = true;
                var isPinned = EssentialProperties.IsPinned(IdeApp.Workbench.ActiveDocument);
                info.Text = isPinned ? "Pinned on Solution Tree" : "Pin on Solution Tree";
                info.Checked = isPinned;
            }
        }

        protected override void Run()
        {
            if (IdeApp.Workbench.ActiveDocument != null && IdeApp.Workbench.ActiveDocument.IsFile)
            {
                var isPinned = EssentialProperties.IsPinned(IdeApp.Workbench.ActiveDocument);
                if (isPinned)
                    EssentialProperties.RemovePinnedDocument(IdeApp.Workbench.ActiveDocument);
                else
                    EssentialProperties.AddPinnedDocument(IdeApp.Workbench.ActiveDocument);

                var pad = (SolutionPad)IdeApp.Workbench.Pads.SolutionPad.Content;
                if (pad == null)
                    return;

                var node = pad.Controller.GetSelectedNode();
                if (node != null && node.DataItem is ProjectFile projectfile)
                {
                    if (projectfile.FilePath.FullPath == IdeApp.Workbench.ActiveDocument.FilePath.FullPath)
                    {
                        pad.RefreshSelectedNode();
                    }
                }
                else
                {
                    var SolutionPad = (SolutionFilterPad)IdeApp.Workbench.Pads.Find((p) => p.Id == Constants.SolutionFilterPadId).Content;
                    if (SolutionPad != null)
                    {
                        ((SolutionFilterWidget)SolutionPad.Control).FilterSolutionPad();
                    }
                }
            }
        }
    }
}