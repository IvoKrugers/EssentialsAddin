using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using EssentialsAddin.Helpers;
using Microsoft.VisualStudio.Text.Editor.Commanding.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;
using MonoDevelop.Projects;

namespace EssentialsAddin.SolutionFilter
{
    public class FileNodeBuilderExtension : NodeBuilderExtension
    {
        private readonly string OneClickChar
#if DEBUG
         = "-->";
#else
         = "=>";
#endif
        public static string OneClickShowFileOption = "OneClickShowFile";

        private FilteredProjectCache _fileCache = new FilteredProjectCache();

        public override bool CanBuildNode(Type dataType)
        {
            var canBuild =
                typeof(ProjectFolder).IsAssignableFrom(dataType) ||
                typeof(ProjectFile).IsAssignableFrom(dataType);
            //Debug.WriteLine($"[CanBuildNode] {dataType}, canBuild: {canBuild}");
            return canBuild;
        }

        public override void PrepareChildNodes(object dataObject)
        {
            //Debug.WriteLine($"PrepareChildNodes {dataObject}");
            base.PrepareChildNodes(dataObject);
        }

        public override void GetNodeAttributes(ITreeNavigator parentNode, object dataObject, ref NodeAttributes attributes)
        {
            base.GetNodeAttributes(parentNode, dataObject, ref attributes);

            var filter = EssentialProperties.SolutionFilterArray;
            if (dataObject is ProjectFolder pf && filter.Length > 0)
            {
                if (!HasChildNodesInFilter((ITreeBuilder)parentNode, pf))
                {
                    attributes = NodeAttributes.Hidden;
                }
                var txt = attributes == NodeAttributes.Hidden ? "HIDE" : "SHOW";
                Debug.WriteLine($"{txt} \tProjectFolder {pf.Path} ");
            }

            if (dataObject is ProjectFile file && filter.Length > 0)
            {
                var hide = true;
                foreach (var key in filter)
                {
                    if (file.ProjectVirtualPath.ToString().ToLower().Contains(key))
                    {
                        hide = false;
                        break;
                    }
                }
                if (hide)
                    attributes = NodeAttributes.Hidden;
            }
        }
       

        public bool HasChildNodesInFilter(ITreeBuilder builder, ProjectFolder dataObject)
        {
            Project project = builder.GetParentDataItem(typeof(Project), true) as Project;
            if (project == null)
                return false;

            _fileCache.ScanProjectForFiles(project);
            return _fileCache.IsProjectFolderVisible(dataObject);

            // For big projects, a real HasChildNodes value is too slow to get
            //if (project.Files.Count > 500)
            //    return true;

            //var folder = dataObject.Path;

            //foreach (var file in project.Files)
            //{
            //    FilePath path;

            //    if (!file.Visible || file.Flags.HasFlag(ProjectItemFlags.Hidden) || file.Subtype == Subtype.Directory)
            //        continue;

            //    if (file.Subtype != Subtype.Directory)
            //        path = file.IsLink ? project.BaseDirectory.Combine(file.ProjectVirtualPath) : file.FilePath;
            //    //else
            //    //    path = file.FilePath;

            //    if (path.IsChildPathOf(folder))
            //    {
            //        foreach (var key in _filter)
            //        {
            //            if (file.ProjectVirtualPath.FileNameWithoutExtension.ToLower().Contains(key))
            //            {
            //                return true;
            //            }
            //        }
            //    }
            //}
            //return false;
        }

        public override void BuildNode(ITreeBuilder treeBuilder, object dataObject, NodeInfo nodeInfo)
        {
            if (!(dataObject is ProjectFile))
                return;

            ProjectFile file = (ProjectFile)dataObject;

            var ext = Path.GetExtension(file.FilePath);

            if (EssentialProperties.OneClickShowFile &&  EssentialProperties.ExcludedExtensionsFromOneClick.FindIndex((s) => s == ext) == -1)
            {
                nodeInfo.Label = string.Format("{0} {1}", Path.GetFileName(file.FilePath), OneClickChar);
            }
        }

        public override Type CommandHandlerType
        {
            get
            {
                //if (Context.GetTreeBuilder().Options[OneClickShowFileOption])
                if (EssentialProperties.OneClickShowFile)
                    return typeof(FileNodeCommandHandler);
                else
                    return base.CommandHandlerType;
            }
        }
    }
}
