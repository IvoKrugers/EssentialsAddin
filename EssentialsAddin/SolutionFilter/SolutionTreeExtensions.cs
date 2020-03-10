using System;
using System.Diagnostics;
using Cairo;
using EssentialsAddin.Helpers;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Gui.Pads;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;
using MonoDevelop.Projects;

namespace EssentialsAddin.SolutionFilter
{
    public static class SolutionTreeExtensions
    {
        public static void ExpandAll(ITreeNavigator node)
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
                        var wso = node.DataItem as WorkspaceObject;
                        Debug.WriteLine($"{wso.Name} {wso}");

                        if (!string.IsNullOrWhiteSpace(EssentialProperties.ExpandFilter))
                        {
                            foreach (var item in EssentialProperties.ExpandFilterArray)
                            {
                                if (wso.Name.ToLower().Contains(item))
                                {
                                    ExpandCSharpProjectFiles(node);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ExpandCSharpProjectFiles(node);
                        }
                        continueLoop = node.MoveNext();
                    }
                    node.MoveToParent();
                }
            }
        }

        private static void ExpandCSharpProjectFiles(ITreeNavigator node)
        {
            if (node == null)
                return;

            var typename = node.DataItem.GetType().Name;

            if (typename == "CSharpProject" || typename == "ProjectFolder" || typename == "ProjectFile")
            {

                if (FilteredProjectCache.IsProjectItemExpanded(node.DataItem))
                    node.ExpandToNode();

                if (node.HasChildren())
                {
                    var continueLoop = node.MoveToFirstChild();
                    //var hasDependingChildren = false;
                    while (continueLoop)
                    {
                        if (!(node.DataItem is ProjectFile pf) || string.IsNullOrEmpty(pf.DependsOn))
                            ExpandCSharpProjectFiles(node);
                        continueLoop = node.MoveNext();
                    }
                    node.MoveToParent();

                    //if (hasDependingChildren)
                    //    node.ExpandToNode();

                    return;
                }

                //if (typename == "ProjectFolder" && FilteredProjectCache.IsProjectFolderExpanded(node.DataItem as ProjectFolder))
                //    node.ExpandToNode();
            }
        }

        public static void ExpandOnlyCSharpProjects(string[] filter)
        {
            var pad = (SolutionPad)IdeApp.Workbench.Pads.SolutionPad.Content;
            if (pad == null)
                return;

            pad.TreeView.GrabFocus();
            pad.TreeView.CollapseTree();

            var node = pad.TreeView.GetRootNode();
            if (node != null)
            {
                //node.Expanded = true;
                pad.TreeView.RefreshNode(node);

                var typename = node.DataItem.GetType().Name;
                if (typename == "Solution")
                {
                    if (node.HasChildren())
                    {
                        var continueLoop = node.MoveToFirstChild();
                        while (continueLoop)
                        {
                            if (node.DataItem is Project proj)
                            {
                                Debug.WriteLine($"{proj.Name} {proj}");
                                if (filter.Length == 0)
                                {
                                    node.MoveToFirstChild();
                                    node.ExpandToNode();
                                    node.MoveToParent();
                                }
                                else
                                {
                                    foreach (var item in filter)
                                    {
                                        if (proj.Name.ToLower().Contains(item))
                                        {
                                            node.MoveToFirstChild();
                                            node.ExpandToNode();
                                            node.MoveToParent();
                                            break;
                                        }
                                    }
                                }
                            }
                            continueLoop = node.MoveNext();
                        }
                        node.MoveToParent();
                    }
                }
            }
        }
    }
}
