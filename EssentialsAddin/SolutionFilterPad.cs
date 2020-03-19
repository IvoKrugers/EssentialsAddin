using MonoDevelop.Components;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace EssentialsAddin
{
    public class SolutionFilterPad : PadContent
    {
        public override Control Control
            => control ?? (control = new SolutionFilterWidget());
        SolutionFilterWidget control;

        public override string Id => Constants.SolutionPadId;

        protected override void Initialize(IPadWindow window)
        {
            base.Initialize(window);
            StartListeningForWorkspaceChanges();
            //window.PadHidden += (sender, e) => control.SaveNodeLocationsForSelectedProject();

            //Debug.WriteLine($"Bundle path: {NSBundle.MainBundle.BundlePath}");
            //Debug.WriteLine($"Bundle Resource path: {NSBundle.MainBundle.ResourcePath}");

            this.Window.Title = $"Essentials Pad {Constants.Version}";
        }

        void StartListeningForWorkspaceChanges()
        {
            IdeApp.Workbench.DocumentClosed += (sender, e) => control.OnDocumentClosed();
        }
    }
}
