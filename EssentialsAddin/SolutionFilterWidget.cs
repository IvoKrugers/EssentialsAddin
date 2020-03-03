using System;
using System.Diagnostics;
using Cairo;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Pads;

namespace EssentialsAddin
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class SolutionFilterWidget : Gtk.Bin
    {
        public SolutionFilterWidget()
        {
            this.Build();
            ShowAll();
        }

        protected void OnFilterEntryChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("[OnFilterEntryChanged] TEST!!!!");
            PropertyService.Set(SolutionFilterPad.PROPERTY_KEY, filterEntry.Text);

            var pad = (SolutionPad)IdeApp.Workbench.Pads.SolutionPad.Content;
            if (pad == null)
                return;

            pad.TreeView.GrabFocus();
            //pad.TreeView.CollapseTree();
            //pad.TreeView.Clear();
            //pad.TreeView.RefreshTree();
            var root = pad.TreeView.GetRootNode();
            if (root != null)
            {
                root.Expanded = true;
                pad.TreeView.RefreshNode(root);
            }
            filterEntry.GrabFocus();
        }
    }
}
