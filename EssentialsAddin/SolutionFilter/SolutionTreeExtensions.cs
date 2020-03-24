using System.Diagnostics;
using EssentialsAddin.Helpers;
using MonoDevelop.Ide.Gui.Components;
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
            }

            if (node.HasChildren())
            {
                var continueLoop = node.MoveToFirstChild();
                while (continueLoop)
                {
                    var wso = node.DataItem as WorkspaceObject;

                    if (node.DataItem is Project)
                    {
                        if (!string.IsNullOrWhiteSpace(EssentialProperties.ExpandFilter))
                        {
                            foreach (var item in EssentialProperties.ExpandFilterArray)
                            {
                                if (wso.Name.ToLower().Contains(item))
                                {
                                    ExpandProjectFiles(node);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ExpandProjectFiles(node);
                        }
                    }
                    else
                    {
                        ExpandAll(node);
                    }

                    continueLoop = node.MoveNext();
                }
                node.MoveToParent();
            }
        }

        private static void ExpandProjectFiles(ITreeNavigator node)
        {
            if (node == null)
                return;
#if DEBUG
            if (node.DataItem is ProjectFile f)
            {
                if (f.Name.ToLower().Contains("appstart.cs"))
                {
                    Debug.WriteLine("BINGO !!");
                }
            }

            if (node.DataItem is ProjectFolder folder)
            {
                if (folder.Name.ToLower().Contains("views"))
                {
                    Debug.WriteLine("BINGO !!");
                }
            }
#endif
            if (FilteredProjectCache.IsProjectItemExpanded(node.DataItem))
                node.ExpandToNode();

            if (node.HasChildren())
            {
                var continueLoop = node.MoveToFirstChild();
                while (continueLoop)
                {
                    if (!(node.DataItem is ProjectFile pf) || string.IsNullOrEmpty(pf.DependsOn))
                        ExpandProjectFiles(node);
                    continueLoop = node.MoveNext();
                }
                node.MoveToParent();

                return;
            }
        }

        public static void ExpandOnlyCSharpProjects(ITreeNavigator node)
        {
            if (node == null)
                return;

            var typename = node.DataItem.GetType().Name;
            if (typename == "Solution")
            {
                node.ExpandToNode();
            }

            if (node.HasChildren())
            {
                var continueLoop = node.MoveToFirstChild();
                while (continueLoop)
                {
                    if (node.DataItem is Project proj)
                    {
                        var filter = EssentialProperties.ExpandFilterArray;
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
                    else
                    {
                        ExpandOnlyCSharpProjects(node);
                    }
                    continueLoop = node.MoveNext();
                }
                node.MoveToParent();
            }
        }
    }
}