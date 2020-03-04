using System;
using Mono.Addins;
using Mono.Addins.Description;

[assembly: Addin(
	"EssentialsAddin",
	Namespace = "EssentialsAddin",
	Version = "1.4"
)]

[assembly: AddinName("EssentialsAddin")]
[assembly: AddinCategory("IDE extensions")]
[assembly: AddinDescription("EssentialsAddin provides one click to open functionality. "+
    "This can be deactivated in the solution tree's root entry. Open it's context menu and goto Display Option")]
[assembly: AddinAuthor("Ivo Krugers")]
