using System;
using System.Diagnostics;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Components;
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
            pad.TreeView.CollapseTree();

            var root = pad.TreeView.GetRootNode();
            if (root != null)
            {
                root.Expanded = true;
                var option = root.Options[FileNodeBuilderExtension.OneClickShowFileOption];
                root.Options[FileNodeBuilderExtension.OneClickShowFileOption] = false;
                pad.TreeView.RefreshNode(root);

                ExpandAll(pad.TreeView, root);

                root.Options[FileNodeBuilderExtension.OneClickShowFileOption] = option;
            }
            filterEntry.GrabFocus();
        }

        private void ExpandAll(ExtensibleTreeView tree,  ITreeNavigator node)
        {
            if (node == null)
                return;

            node.ExpandToNode();

            Debug.WriteLine($"\t{node.DataItem.GetType().FullName}\t{node.NodeName}");

            if (node.HasChildren())
            {
                var continueLoop = node.MoveToFirstChild();
                while (continueLoop)
                {
                    ExpandAll(tree, node);
                    continueLoop = node.MoveNext();
                }
                node.MoveToParent();
                Debug.WriteLine($"\t--{node.DataItem.GetType().FullName}\t{node.NodeName}");
                return;
            }
           
        }
    }
}
