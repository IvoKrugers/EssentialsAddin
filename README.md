# EssentialsAddin
An extension for Visual studio for Mac that adds functionality to the IDE. I use this on a daily basis and find myself shorthanded when not having it installed.

## Solution tree filtering

This Addin adds a Pad to filter the solution tree by partial file or foldername. Its real power can be found its ability to filter by multiple search terms seperated by a space. This way you can specify a subset of files/folder to work on. Oh, did I mention that it is not necessary to write file/foldernames out in full.

![Opening and filtering the solution tree.](/Art/Demo1_low_640.gif)

- Documents that are pinned in the Documents Pad are always shown in the Solution tree regardless of if they match with any search term. A refresh or change in search terms is required for this to be reflected in the solution tree.

- The search terms, pinned documents and projects to expand are stored in a json file in the solutionfolder. This way these values are per solution and switching between solutions is easier. 

- This Pad can be opened from the `View -> pads` menu and `Tools -> Essentials` menu.

## One click to open file

As a developer I got annoyed at VSMac for all of that double clicking a file on the solution tree to view it. Thus this feature. It will reduce your mouse clicking drastically during the day. Please do learn your `CMD + SHIFT + W` shortcut to close all open windows.

![Activating OneClick.](/Art/Demo2_low_640.gif)


## Projects to expand

Is your filtering still showing a lot of solutiontree entries, mayby you can filter those projects that you only care about. Like if you're only working on iOS, you only want to see the core and iOS project. Then specify: `core ios` and hit apply or change the tree filter.


<!--
## Installation

1. Download `.mpack` file from [Releases](https://github.com/IvoKrugers/EssentialsAddin/releases)

2. Go to the Extension manager via menu `Preferences -> Extensions` and use the "Install from File" button to install

3. Close Visual studio and re-open it.

The filter pad can be opened from the `Tools -> Essentials` or `View -> Pads` menu

If you have a previous version installed, uninstall it first, close Visual studio and re-open the app before installing the new version.
-->

## Updates

That filter Pad will show you a button when an update of this addin is availabe. Clicking it will take you to the release on github.

![The update button](/Art/UpdateButton.png)

This is still quite anonying to install and I was not able to create an account at [https://addins.monodevelop.com](https://addins.monodevelop.com). So I created a repo of my own. Follow the instructions in the next section to make the update process easier. 

## Installation

1. Open Visual Studio Extension Repository Manager via `Visual Studio -> Extensions... -> Gallery -> Repositories Dropdown -> Manage Repositories`.

2. Add `https://raw.githubusercontent.com/IvoKrugers/EssentialsAddin/master/mpack/main.mrep` to your repository sources.

3. Back in extension manager, you should now be able to see all the extensions available for installation.

**Note:** Installing an extension might require a restart of Visual Studio for Mac.

Any future updates to installed extensions should show up in the `Updates` tab of the Extension Manager.

<!--
## Terminal

Wanna install it from commandline? Dowload the mpack file and run the following instruction in the Terminal app.

```"/Applications/Visual Studio (Preview).app/Contents/MacOS/vstool" setup install ~/Downloads/EssentialsAddin.EssentialsAddin_1.7.13.mpack ```

Please make sure the location and filename of the dowloaded mpack file are correctly put in the instruction.
-->
## Authors

**Ivo Krugers** - Author

## Thanks
[Arthur Demanuele](https://github.com/ademanuele) for extension repo example

## License

This project is licensed under the MIT License - [full details](LICENSE).
