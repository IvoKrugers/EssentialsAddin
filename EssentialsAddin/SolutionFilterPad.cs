using System.Diagnostics;
using System.Threading.Tasks;
using MonoDevelop.Components;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Documents;

namespace EssentialsAddin
{
    public class SolutionFilterPad : PadContent
    {
        public override Control Control
            => control ?? (control = new SolutionFilterWidget());
        SolutionFilterWidget control;

        public override string Id => "EssentialsAddin.SolutionFilterPad";

        public static string PROPERTY_KEY = "EssentialsAddin.SolutionFilterPad.Filter";

        protected override void Initialize(IPadWindow window)
        {
            base.Initialize(window);
            StartListeningForWorkspaceChanges();
            //window.PadHidden += (sender, e) => control.SaveNodeLocationsForSelectedProject();

            //Debug.WriteLine($"Bundle path: {NSBundle.MainBundle.BundlePath}");
            //Debug.WriteLine($"Bundle Resource path: {NSBundle.MainBundle.ResourcePath}");
        }

        void StartListeningForWorkspaceChanges()
        {
            //IdeApp.Workspace.SolutionUnloaded += (sender, e) => control.Clear();
            //IdeApp.Workspace.SolutionLoaded += (sender, e) => control.ReloadProjects();
            //IdeApp.Workspace.ItemAddedToSolution += (sender, e) => control.ReloadProjects();
            //IdeApp.Workspace.ItemRemovedFromSolution += (sender, e) => control.ReloadProjects();
            IdeApp.Workspace.LastWorkspaceItemClosed += (sender, e) => Debug.WriteLine("\t\tLASTWORKSPACEITEMCLOSED !!");
            IdeApp.Workspace.WorkspaceItemClosed += (sender, e) => Debug.WriteLine("\t\tWorkspaceItemClosed !!");
            IdeApp.Workspace.WorkspaceItemUnloaded += (sender, e) => Debug.WriteLine("\t\tWorkspaceItemUnloaded !!");
            IdeApp.Workbench.DocumentClosed += (sender, e) =>
            {
                Debug.WriteLine("DocumentClosed!!");
            };
            IdeApp.Workbench.DocumentClosing += Workbench_DocumentClosing;
        }

        private Task Workbench_DocumentClosing(object o, DocumentCloseEventArgs e)
        {
            Debug.WriteLine("DocumentClosing!!");
            return Task.CompletedTask;
        }
    }
}
