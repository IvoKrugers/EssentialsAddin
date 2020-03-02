using System;
using System.Diagnostics;
using Foundation;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using Xwt;

namespace EssentialsAddin
{
	public class NewInstanceVSHandler: CommandHandler
	{
		[CommandHandler(AddinCommands.NewInstanceVS)]
		protected override void Run()
		{
			string cmd = ";open -n -a \"Visual Studio\"";


			var startInfo = new ProcessStartInfo()
			{
				FileName = cmd,
				Arguments = null,
				UseShellExecute = false,
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				RedirectStandardInput = true,
				UserName = System.Environment.UserName
			};

			//Process.Start(startInfo);

			//if (DesktopService.CanOpenTerminal)
			//{
			//	DesktopService.OpenTerminal("~");
			//}


			//var output = Run("~", cmd);

			
			
			
			//using (Process process = Process.Start(startInfo))
			//{ // Monitor for exit}
			//	process.WaitForExit();
			//	using (var output = process.StandardOutput)
			//	{
			//		Console.Write("Results: {0}", output.ReadLine());
			//	}
			//}
			//Desktop.OpenFile("");
			
		}


		string Run(string launchPath, params string[] launchArgs)
		{
			var r = string.Empty;

			try
			{
				var pipeOut = new NSPipe();

				var t = new NSTask();
				t.LaunchPath = launchPath;
				t.Arguments = launchArgs;
				t.StandardOutput = pipeOut;

				t.Launch();
				//t.WaitUntilExit();
				//t.Release();

				//r = pipeOut.ReadHandle.ReadDataToEndOfFile().ToString();
			}
			catch (Exception ex)
			{
				Console.WriteLine("launchctl failed: " + ex);
			}
			return r;
		}
		
		protected override void Update(CommandInfo info)
		{
			info.Enabled = true;
		}
	}
}
