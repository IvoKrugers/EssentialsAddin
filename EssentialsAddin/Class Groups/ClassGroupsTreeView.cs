using System.Collections.Generic;
using System.Linq;
using Diagram.ClassGroups;
using Diagram.Drawing.Data;
using Diagram.Storage;
using Gtk;

namespace Diagram
{
	[System.ComponentModel.ToolboxItem(typeof(ClassGroupsTreeView))]
	public class ClassGroupsTreeView : TreeView
	{
		public ClassGroupsTreeView()
		{
			SetupColumns();
			ShowAll();
		}

		void SetupColumns()
		{
			var mainColumn = new TreeViewColumn();

			var groupIconCell = new ClassGroupCellRenderer();
			mainColumn.PackStart(groupIconCell, false);
			mainColumn.SetCellDataFunc(groupIconCell, HandleClassGroupIconRenndererData);

			var nameCell = new CellRendererText();
			mainColumn.PackStart(nameCell, true);
			mainColumn.AddAttribute(nameCell, "text", 0);

			AppendColumn(mainColumn);
		}

		void HandleClassGroupIconRenndererData(TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter)
		{
			var store = tree_model as ClassGroupsTreeStore;
			(cell as ClassGroupCellRenderer).ClassGroup = store.ClassGroupForIter(iter);
		}
	}

	public class ClassGroupsTreeStore : TreeStore
	{
		Dictionary<string, TreeIter> ClassGroups { get; set; }
		StoredDiagramDetails storedDetails;
		TreeIter noGroupIter;

		public ClassGroupsTreeStore(List<ClassDefinition> classes, StoredDiagramDetails storedDetails) : base(typeof(string), typeof(string))
		{
			this.storedDetails = storedDetails;
			AddClassGroups();

			foreach (var c in classes)
			{
				StoredNode node;
				if (storedDetails.nodes.TryGetValue(c.Id, out node))
				{
					if (node.classGroups == null || node.classGroups.Count == 0)
					{
						AppendValues(noGroupIter, c.DisplayName);
						continue;
					}

					foreach (var g in node.classGroups)
					{
						TreeIter groupNode;
						if (ClassGroups.TryGetValue(g, out groupNode))
							AppendValues(groupNode, c.DisplayName);
					}
				}
				else
					AppendValues(noGroupIter, c.DisplayName);
			}
		}

		void AddClassGroups()
		{
			ClassGroups = new Dictionary<string, TreeIter>();

			foreach (var g in storedDetails.classGroups)
				ClassGroups.Add(g.Key, AppendValues(g.Value.label));

			noGroupIter = AppendValues("No Group");
		}

		public ClassGroup ClassGroupForIter(TreeIter iter)
		{
			var groupId = ClassGroups.FirstOrDefault(x => x.Value.Equals(iter)).Key;
			if (groupId == null) return null;
			return storedDetails.classGroups[groupId];
		}

		public void AppendClassGroup(string key, ClassGroup group)
		{
			var newIter = InsertNodeBefore(noGroupIter);
			SetValues(newIter, group.label);
			ClassGroups[key] = newIter;
		}
	}
}
