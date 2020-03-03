using System;
using System.Diagnostics;
using Gtk;

namespace Diagram
{
    public partial class DiagramPadWidget : Bin
	{
		public DiagramPadWidget()
		{
			ShowAll();
		}

        public void ReloadProjects()
        {
        }

		public void Clear()
		{
			DisableUi();
			filterEntry.Text = string.Empty;
		}

		void EnableUi()
		{
			filterEntry.Sensitive = true;
		}

		void DisableUi()
		{
			filterEntry.Sensitive = false;
		}

        protected void OnFilterEntryChanged(object sender, EventArgs e)
        {
			Debug.WriteLine("[OnFilterEntryChanged] TEST!!!!");
        }
    }
}
