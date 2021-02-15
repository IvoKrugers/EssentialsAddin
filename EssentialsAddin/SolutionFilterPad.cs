using EssentialsAddin.Helpers;
using MonoDevelop.Components;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace EssentialsAddin
{
    public class SolutionFilterPad : PadContent
    {
        private SolutionFilterWidget control;
        public override Control Control => control ?? (control = new SolutionFilterWidget());
        public override string Id => Constants.SolutionFilterPadId;

        protected override void Initialize(IPadWindow window)
        {
            base.Initialize(window);
            StartListeningForWorkspaceChanges();
            //window.PadHidden += (sender, e) => control.SaveNodeLocationsForSelectedProject();

            //Debug.WriteLine($"Bundle path: {NSBundle.MainBundle.BundlePath}");
            //Debug.WriteLine($"Bundle Resource path: {NSBundle.MainBundle.ResourcePath}");

            //this.Window.Title = $"Solution Filter ({Constants.Version})";
            Initialize();
        }

        void StartListeningForWorkspaceChanges()
        {
            IdeApp.Workbench.DocumentClosed += (sender, e) => control.FilterSolutionTreeDelayed();
            IdeApp.Workbench.DocumentOpened += (sender, e) => control.FilterSolutionTreeDelayed();

            //IdeApp.Workspace.SolutionUnloaded += (sender, e) => control.Clear();
            IdeApp.Workspace.SolutionLoaded += (sender, e) => Initialize();

            IdeApp.Workspace.ItemAddedToSolution += (sender, e) => control.FilterSolutionTreeDelayed();
            IdeApp.Workspace.ItemRemovedFromSolution += (sender, e) => control.FilterSolutionTreeDelayed();

            IdeApp.Workspace.CurrentSelectedSolutionChanged += (sender, e) => Initialize();
        }

        private void Initialize()
        {
            PropertyService.Instance.Init(IdeApp.Workspace.CurrentSelectedSolution);
            ((SolutionFilterWidget)Control).LoadProperties();
        }
    }
}
