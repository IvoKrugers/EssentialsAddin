using System;
using Mono.Addins;
using Mono.Addins.Description;

[assembly: Addin(
	"EssentialsAddin",
	Namespace = "EssentialsAddin",
	Version = EssentialsAddin.Constants.Version
)]

[assembly: AddinName("EssentialsAddin")]
[assembly: AddinCategory("IDE extensions")]
[assembly: AddinDescription("EssentialsAddin provides:\n"+
    " - Solution tree filtering\n" +
    " - One click to open file functionality. \n" +
    " - Expand project filter when closing all projects\n"+
    "\n"+
    "The filter pad can be opened from the Tools->Essentials->")]
[assembly: AddinAuthor("Ivo Krugers")]

[assembly: AddinDependency("::MonoDevelop.Core", MonoDevelop.BuildInfo.Version)]
[assembly: AddinDependency("::MonoDevelop.Ide", MonoDevelop.BuildInfo.Version)]