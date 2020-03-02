using Gtk;
using System.Collections.Generic;
using Diagram.Drawing.Data;
using Diagram.ClassGroups;
using Diagram.Storage;

namespace Diagram
{
	public delegate void ClassGroupContextMenuSelectionDelegate(string groupId);

	public class ClassGroupsContextMenu : Menu
	{
		public event ClassGroupContextMenuSelectionDelegate GroupSelected;
		public event ClassGroupContextMenuSelectionDelegate GroupDeselected;

		Dictionary<string, ClassGroupContextMenuItem> menuItems;

		public ClassGroupsContextMenu(Dictionary<string, ClassGroup> groups, StoredNode node = null)
		{
			PopulateMenu(groups, node);
			ShowAll();
		}

		void PopulateMenu(Dictionary<string, ClassGroup> groups, StoredNode node)
		{
			if (groups.Count == 0)
			{
				Add(NoLabelsMenuItem());
			}
			else
			{
				PopulateMenuItems(groups);
				SetMenuItemValues(node);
			}
		}

		void PopulateMenuItems(Dictionary<string, ClassGroup> groups)
		{
			menuItems = new Dictionary<string, ClassGroupContextMenuItem>();
			foreach (var g in groups)
			{
				var menuItem = new ClassGroupContextMenuItem(this, g.Key, g.Value);
				menuItems.Add(g.Key, menuItem);
				Add(menuItem);
			}
		}

		void SetMenuItemValues(StoredNode node)
		{
			if (node != null && node.classGroups != null)
			{
				foreach (var g in node.classGroups)
				{
					menuItems.TryGetValue(g, out ClassGroupContextMenuItem menuItem);
					if (menuItem != null) menuItem.Checked = true;
				}
			}
		}

		MenuItem NoLabelsMenuItem()
		{
			var item = new MenuItem();
			item.Add(new Label { Text = "No Groups" });
			item.Sensitive = false;
			return item;
		}

		protected void OnItemActivated(MenuItem item)
		{
			var menuItem = (Active as ClassGroupContextMenuItem);
			if (menuItem.Checked)
				GroupDeselected?.Invoke(menuItem.GroupId);
			else
				GroupSelected?.Invoke(menuItem.GroupId);
		}

		class ClassGroupContextMenuItem : MenuItem
		{
			public string GroupId { get; }
			public bool Checked
			{
				get => checkbox.Active;
				set => checkbox.Active = value;
			}

			ClassGroupsContextMenu parentMenu;
			CheckButton checkbox;

			public ClassGroupContextMenuItem(ClassGroupsContextMenu parentMenu, string groupId, ClassGroup g)
			{
				this.parentMenu = parentMenu;
				GroupId = groupId;
				InitUI(g);
			}

			void InitUI(ClassGroup g)
			{
				var hbox = new HBox();
				checkbox = new CheckButton();
				hbox.PackStart(checkbox);
				var color = new ClassGroupColorWidget { Color = g.color };
				hbox.PackStart(color);
				var label = new Label { Text = g.label };
				hbox.PackStart(label, true, true, 0);
				Add(hbox);
			}

			protected override void OnActivate()
			{
				base.OnActivate();
				parentMenu.OnItemActivated(this);
			}
		}
	}
}
