using System;
using System.Diagnostics;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;

namespace EssentialsAddin
{
	public class ViewFileHandler: CommandHandler
	{
		
		protected override void Run(object dataItem)
		{
			base.Run(dataItem);
		
			//var editor = IdeApp.Workbench.ActiveDocument.Editor;
			//var date = $"//{DateTime.Now.ToShortDateString()}";
			//editor.InsertAtCaret(date);
			
			//IdeApp.Workbench.ActiveDocumentChanged += OnWindowChanged;
			//IdeApp.ProjectOperations.CurrentSelectedSolutionChanged += OnSolutionChanged;
		}

        void OnSolutionChanged(object sender, MonoDevelop.Projects.SolutionEventArgs e) 
		{
			Debug.Print($"CurrentSelectedSolutionChanged {IdeApp.ProjectOperations.CurrentSelectedItem.ToString()}");
		}

        void OnWindowChanged(object ob, EventArgs args)
		{
			Debug.Print($"OnWindowChanged {IdeApp.Workbench.ActiveDocument.FileName}");
		}

        protected override void Update(CommandInfo info)
		{
			info.Enabled = true;
			//info.Enabled = IdeApp.Workbench.ActiveDocument?.Editor != null;
		}
	}
}
