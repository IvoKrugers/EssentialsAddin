using System;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Gui.Pads;

namespace EssentialsAddin.Helpers
{
    public static class SolutionPadExtensions
    {
        public static ExtensibleTreeViewController GetTreeView(this SolutionPad pad)
        {
            return (ExtensibleTreeViewController)pad.Controller;
        }

        public static ITreeNavigator GetRootNode(this SolutionPad pad)
        {
            return pad.GetTreeView().GetRootNode();
        }

        public static void CollapseTree(this SolutionPad pad)
        {
            var c = (ExtensibleTreeView)pad.Control;
            c.CollapseTree();
        }

        
        public static ITreeNavigator GetRootNode(this ExtensibleTreeViewController treeview)
        {
            var pos = treeview.GetRootPosition();
            return treeview.GetNodeAtPosition(pos);
        }
    }
}
