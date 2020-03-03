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

        private void ExpandAll(ExtensibleTreeView tree, ITreeNavigator node)
        {
            if (node == null)
                return;

            var typename = node.DataItem.GetType().Name;
            if (typename == "Solution")
            {
                node.ExpandToNode();

                Debug.WriteLine($"\t{node.DataItem.GetType().FullName}\t{node.NodeName}");

                if (node.HasChildren())
                {
                    var continueLoop = node.MoveToFirstChild();
                    while (continueLoop)
                    {
                        ExpandCSharpProject(tree, node);
                        continueLoop = node.MoveNext();
                    }
                    node.MoveToParent();
                }
            }
        }

        private void ExpandCSharpProject(ExtensibleTreeView tree, ITreeNavigator node)
        {
            if (node == null)
                return;

            var typename = node.DataItem.GetType().Name;
          
            if (typename == "CSharpProject" || typename == "ProjectFolder" || typename == "ProjectFile")
            {

                if (node.HasChildren())
                {
                    var continueLoop = node.MoveToFirstChild();
                    while (continueLoop)
                    {
                        ExpandCSharpProject(tree, node);
                        continueLoop = node.MoveNext();
                    }
                    node.MoveToParent();
                    return;
                }

                if (typename != "CSharpProject")
                {
                    Debug.WriteLine($"\tExpandToNode {node.DataItem.GetType().FullName}\t{node.NodeName}");
                    node.ExpandToNode();
                }
            }
        }
    }
}
