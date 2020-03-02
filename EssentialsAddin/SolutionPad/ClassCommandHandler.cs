using System;
using System.Text;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Projects;

namespace EssentialsAddin.SolutionPad
{
	public enum SampleCommands
	{
		ShowProperties
	}

	class ClassCommandHandler : NodeCommandHandler
	{
		[CommandHandler(SampleCommands.ShowProperties)]
		protected void ShowProperties()
		{
			//IClass cls = (IClass)CurrentNode.DataItem;

			Project p = (Project)CurrentNode.GetParentDataItem(typeof(Project), false);
			ProjectFile file = (ProjectFile)CurrentNode.GetParentDataItem(typeof(ProjectFile), false);

			StringBuilder sb = new StringBuilder();
			//sb.AppendFormat("Class: {0}\n", cls.Name);
			sb.AppendFormat("Project: {0}\n", p.Name);
			sb.AppendFormat("File: {0}\n", file.Name);
			sb.AppendFormat("Methods:\n", file.Name);

			//foreach (IMethod met in cls.Methods)
				//sb.AppendFormat(" - {0}\n", met.Name);

			MessageService.ShowMessage(sb.ToString());
		}

		public override void ActivateItem()
		{
			//IClass cls = (IClass)CurrentNode.DataItem;
			//if (cls.Region.FileName != null)
			//{
			//	Runtime.FileService.OpenFile(cls.Region.FileName);
			//}

		}

		public override void OnItemSelected()
		{
			base.OnItemSelected();

		}

	//	protected override void Update(CommandInfo info)
	//	{
	//		info.Enabled = IdeApp.Workbench.ActiveDocument?.Editor != null;
	//	}
	}
}
