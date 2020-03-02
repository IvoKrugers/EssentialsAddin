using MonoDevelop.Components;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace Diagram
{
	public class DiagramPad : PadContent
	{
		public override Control Control => control ?? (control = new DiagramPadWidget());
		DiagramPadWidget control;

		public override string Id => "Diagram.DiagramPad";

		protected override void Initialize(IPadWindow window)
		{
			base.Initialize(window);
			StartListeningForWorkspaceChanges();
			window.PadHidden += (sender, e) => control.SaveNodeLocationsForSelectedProject();

			//Debug.WriteLine($"Bundle path: {NSBundle.MainBundle.BundlePath}");
			//Debug.WriteLine($"Bundle Resource path: {NSBundle.MainBundle.ResourcePath}");
		}

		void StartListeningForWorkspaceChanges()
		{
			IdeApp.Workspace.SolutionUnloaded += (sender, e) => control.Clear();
			IdeApp.Workspace.SolutionLoaded += (sender, e) => control.ReloadProjects();
			IdeApp.Workspace.ItemAddedToSolution += (sender, e) => control.ReloadProjects();
			IdeApp.Workspace.ItemRemovedFromSolution += (sender, e) => control.ReloadProjects();
		}
	}
}
