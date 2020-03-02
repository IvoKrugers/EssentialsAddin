using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diagram.ClassGroups;
using Diagram.Drawing;
using Diagram.Storage;
using Gtk;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace Diagram
{
	public partial class DiagramPadWidget : Bin
	{
		IEnumerable<Project> projects;
		Project previouslySelectedProject, selectedProject;
		StoredDiagramDetails diagramDetails;

		ListStore projectDropdownStore;
		ClassDrawingWebView drawingArea;
		DrawingProgressBar progressBar;
		ClassGroupsTreeView groupsTree;

		public DiagramPadWidget()
		{
			//Build();
			//SetupProgressBar();
			//SetupGroupsTree();

			//SetupProjectDropdown();
			//SetupDrawingArea();
			//ReloadProjects();

			ShowAll();
		}

		void SetupProgressBar()
		{
			//progressBar = new DrawingProgressBar();
			//statusBarHBox.PackStart(progressBar, true, true, 0);
		}

		void SetupGroupsTree()
		{
			groupsTree = new ClassGroupsTreeView();
			classGroupsVBox.PackEnd(groupsTree, true, true, 0);
		}

		void SetupProjectDropdown()
		{
			projectDropdownStore = new ListStore(typeof(string));
			projectDropdown.Model = projectDropdownStore;
			projectDropdown.Changed += HanldeProjectDropdownChanged;
		}

		async void HanldeProjectDropdownChanged(object sender, EventArgs e)
		{
			DisableUi();
			SaveNodeLocationsForSelectedProject();
			UpdateSelectedProjects();
			await RedrawAsync();
		}

		void UpdateSelectedProjects()
		{
			previouslySelectedProject = selectedProject;
			selectedProject = projects.ElementAt(projectDropdown.Active);
		}

		async Task RedrawAsync()
		{
			var progress = new Progress<TreeBuilderProgress>();
			progressBar.Progress = progress;

			var classes = await ClassTreeBuilder.Instance.ProcessAsync(selectedProject, progress);
			diagramDetails = DiagramStorage.Instance.GetProjectDetails(selectedProject);
			drawingArea.Draw(classes, diagramDetails);
			groupsTree.Model = new ClassGroupsTreeStore(classes, diagramDetails);
			EnableUi();
		}

		void SetupDrawingArea()
		{
			drawingArea = new ClassDrawingWebView();
			drawingArea.NodeDragged += (e) => OnDrawingAreaNodeDragged(e.nodeId, e.x, e.y);
			drawingArea.NodeContextClicked += (e) => OnDrawingAreaNodeContextClicked(e.nodeId, e.x, e.y);
			var drawingAreaWidget = drawingArea.GtkWidget;
			drawingAreaWidget.SetSizeRequest(200, 200);
			mainHPaned.Pack2(drawingAreaWidget, true, true);
		}

		void OnDrawingAreaNodeDragged(string nodeId, int x, int y)
		{
			StoredNode node;
			if (diagramDetails.nodes.ContainsKey(nodeId))
				node = diagramDetails.nodes[nodeId];
			else
			{
				node = new StoredNode();
				diagramDetails.nodes.Add(nodeId, node);
			}
			node.x = x;
			node.y = y;
		}

		void OnDrawingAreaNodeContextClicked(string nodeId, int x, int y)
		{
			diagramDetails.nodes.TryGetValue(nodeId, out StoredNode node);
			var menu = new ClassGroupsContextMenu(diagramDetails.classGroups, node);
			menu.GroupSelected += (groupId) =>
			{
				if (node == null)
					diagramDetails.nodes.Add(nodeId, new StoredNode { classGroups = new List<string> { groupId } });
				else if (node.classGroups == null)
					node.classGroups = new List<string> { groupId };
				else
					node.classGroups.Add(groupId);

				// TODO: REFRESH TREE
				groupsTree.Model = groupsTree.Model;
			};
			menu.GroupDeselected += (groupId) => node.classGroups.Remove(groupId);

			menu.Popup();
		}

		public void ReloadProjects()
		{
			selectedProject = null;
			previouslySelectedProject = null;
			projectDropdownStore.Clear();
			projects = IdeApp.Workspace.GetAllProjects();
			foreach (var project in projects)
				projectDropdownStore.AppendValues(project.Name);
		}

		public void Clear()
		{
			DisableUi();
			selectedProject = null;
			previouslySelectedProject = null;
			projectDropdownStore.Clear();
			drawingArea.Clear();
			groupsTree.Model = null;
			progressBar.Clear();
		}

		void EnableUi()
		{
			projectDropdown.Sensitive = true;
			newClassGroupButton.Sensitive = true;
			drawGroupsCheckbox.Sensitive = true;
		}

		void DisableUi()
		{
			projectDropdown.Sensitive = false;
			newClassGroupButton.Sensitive = false;
			drawGroupsCheckbox.Sensitive = false;
		}

		public void SaveNodeLocationsForSelectedProject()
		{
			if (selectedProject == null) return;
			DiagramStorage.Instance.SaveProjectDetails(selectedProject, diagramDetails);
		}

		protected void HandleGroupsCheckboxToggled(object sender, EventArgs e)
		{
			drawingArea.DrawClassGroups = (sender as CheckButton).Active;
		}

		protected void HandleNewGroupClicked(object sender, EventArgs e)
		{
			using (var dialog = new NewClassGroupDialog(diagramDetails.classGroups))
			{
				if (dialog.Run() != (int)ResponseType.Ok) return;

				diagramDetails.classGroups.Add(dialog.NewGroupId, dialog.NewGroup);
				(groupsTree.Model as ClassGroupsTreeStore).AppendClassGroup(dialog.NewGroupId, dialog.NewGroup);
			}
		}
	}
}
