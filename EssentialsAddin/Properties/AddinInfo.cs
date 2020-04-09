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
[assembly: AddinDescription("Essentials Addin provides:\n"+
    " - Solution tree filtering (Pad)\n" +
    " - One click to open file functionality. \n" +
    " - Filter project to Expand\n"+
    " - Filter Application Output in a new Pad\n" +
    "\n" +
    "The pads can be opened from the Tools -> Essentials")]
[assembly: AddinAuthor("Ivo Krugers")]

[assembly: AddinDependency("::MonoDevelop.Core", MonoDevelop.BuildInfo.Version)]
[assembly: AddinDependency("::MonoDevelop.Ide", MonoDevelop.BuildInfo.Version)]