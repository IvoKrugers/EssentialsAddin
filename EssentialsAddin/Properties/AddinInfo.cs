using System;
using Mono.Addins;
using Mono.Addins.Description;

[assembly: Addin(
	"EssentialsAddin",
	Namespace = "EssentialsAddin",
	Version = EssentialsAddin.Constants.Version
)]

[assembly: AddinName("Essentials Addin")]
[assembly: AddinCategory("IDE extensions")]
[assembly: AddinDescription("Essentials Addin provides several pads and other functionalities. \n\n"+
"Funtionality summary: \n"+
"- Solution Tree filtering (Pad)\n" +
"	- Filter project to Expand.\n"+
"	- Take into account pinned tabs when filtering solution tree.\n" +
"	- Pin files in Solution Tree via Context Menu\n" +
"	- Store filter and pinned files per solution and git branch\n" +
"\n" +
"- One click to open file functionality. \n" +
"\n" +
"- Menu: Copy Git Url of selected files. This works for GitHub and Azure DevOps.\n" +
"\n" +
"- Filter Application Output in a new Pad\n" +
"\n" +
"The pads can be opened from the View-Pads or Tools menu.\n\nby Ivo Krugers")]
[assembly: AddinAuthor("Ivo Krugers")]
[assembly: AddinUrl("https://github.com/IvoKrugers/EssentialsAddin")]

//[assembly: AddinDependency("::MonoDevelop.Core", MonoDevelop.BuildInfo.Version)]
//[assembly: AddinDependency("::MonoDevelop.Ide", MonoDevelop.BuildInfo.Version)]
[assembly: AddinDependency("::MonoDevelop.Core", "8.0")]
[assembly: AddinDependency("::MonoDevelop.Ide", "8.0")]
[assembly: AddinDependency("::MonoDevelop.TextEditor", "8.0")]