using Gtk;
using Diagram.Drawing.Data;
using System.Collections.Generic;
using MonoDevelop.Components;

namespace Diagram.ClassGroups
{
	public partial class AddClassGroupDialog : Gtk.Dialog
	{
		//public ClassGroup NewGroup { get; set; }
		//public string NewGroupId { get; set; }

		//Dictionary<string, ClassGroup> groups;

		//public AddClassGroupDialog(Dictionary<string, ClassGroup> groups)
		//{
		//	Build();
		//	SetupErrorMessage();
		//	groupNameEntry.ModifyFont(Pango.FontDescription.FromString("monospace Ultra-Bold 24"));
		//	this.groups = groups;
		//}

		//void SetupErrorMessage()
		//{
		//	Gdk.Color col = new Gdk.Color(255, 50, 50);
		//	errorLabel.ModifyFg(StateType.Normal, col);
		//}

		//protected void HandleGroupNameEdited(object sender, System.EventArgs e)
		//{
		//	var text = (sender as Entry).Text;
		//	if (string.IsNullOrEmpty(text))
		//	{
		//		buttonOk.Sensitive = false;
		//	}
		//	else if (groups.ContainsKey(GroupIdForName(text)))
		//	{
		//		buttonOk.Sensitive = false;
		//		errorLabel.Visible = true;
		//	}
		//	else
		//	{
		//		buttonOk.Sensitive = true;
		//		errorLabel.Visible = false;
		//	}
		//}

		//void HandleOkClicked()
		//{
		//	NewGroupId = GroupIdForName(groupNameEntry.Text);
		//	NewGroup = new ClassGroup
		//	{
		//		label = groupNameEntry.Text,
		//		color = colorPicker.Color.ToXwtColor().ToHexString()
		//	};
		//}

		//string GroupIdForName(string name)
		//{
		//	return name.Trim().ToLower().Replace(" ", string.Empty);
		//}

		//protected override void OnResponse(ResponseType response_id)
		//{
		//	base.OnResponse(response_id);

		//	switch (response_id)
		//	{
		//		case ResponseType.Cancel:
		//			Hide();
		//			break;
		//		case ResponseType.Ok:
		//			HandleOkClicked();
		//			Hide();
		//			break;
		//	}
		//}
	}
}
