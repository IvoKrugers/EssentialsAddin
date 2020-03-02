using System;
using Gtk;
using MonoDevelop.Components;
using MonoDevelop.Components.Docking;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Pads;

namespace EssentialsAddin.SolutionFilter
{
	public class SolutionFilterPad: PadContent
	{
		
		Widget control;

		//toolbar
		DockItemToolbar toolbar;
		TextView filterTextView;
		SearchEntry filterSearchEntry;
		

		//content view
		ScrolledWindow sw;

		public static string PAD_ID = "EssentialsAddin.Pads.SolutionFilterPad";
		public static string PROPERTY_KEY = "EssentialsAddin.SolutionFilterPad.Filter";

		public SolutionFilterPad()
		{
			Id = PAD_ID;

			VBox vbox = new VBox();
			//vbox.Accessible.SetShouldIgnore(true);

			filterTextView = new TextView();
			filterTextView.Buffer.Text = (string)PropertyService.Get(PROPERTY_KEY, "");

			filterSearchEntry = new SearchEntry();
			filterSearchEntry.FilterChanged += FilterSearchEntry_FilterChanged;
			sw = new MonoDevelop.Components.CompactScrolledWindow();
			sw.ShadowType = ShadowType.None;

			vbox.Add(sw);

			control = vbox;
		}

		public override Control Control => control;

		protected override void Initialize(IPadWindow window)
		{
			toolbar = window.GetToolbar(DockPositionType.Top);
			toolbar.Add(filterTextView );
			toolbar.Add(filterSearchEntry);
			toolbar.ShowAll();
			filterTextView.Buffer.Changed += Buffer_Changed;
			Buffer_Changed(null, null);
			
			window.Visible = true;
			window.AutoHide = false;
			window.Sticky = true;
			//window.Activate(false);

			control.ShowAll();
		}

		void FilterSearchEntry_FilterChanged(object sender, EventArgs e)
		{
			PropertyService.Set(PROPERTY_KEY, filterSearchEntry.Entry.Text);
			var pad = (SolutionPad)IdeApp.Workbench.Pads.SolutionPad.Content;
			if (pad == null)
				return;

			//pad.TreeView.Clear();
			pad.TreeView.RefreshTree();
		}

		void Buffer_Changed(object sender, EventArgs e)
		{
			PropertyService.Set(PROPERTY_KEY, filterTextView.Buffer.Text);
			
			var pad = (SolutionPad)IdeApp.Workbench.Pads.SolutionPad.Content;
			if (pad == null)
				return;

			//pad.TreeView.Clear();
			//pad.TreeView.RefreshTree();
			pad.TreeView.RefreshNode(pad.TreeView.GetRootNode());
			//pad.TreeView.GetRootNode().Selected = true;
			
			//pad.TreeView.RefreshTree();
			//control.ShowAll();
		}
	}
}
