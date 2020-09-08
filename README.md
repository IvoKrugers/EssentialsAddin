# EssentialsAddin
An extension for Visual studio for Mac.



## Solution tree filtering

The addin adds an pad to the `View -> pads` menu and `Tools -> Essentials` menu. Once opened it allows the developer to filter the solution tree by filename or foldername, like the xcode filter, and allows multiple keywords. These keywords can be of course parcial. It is not necessary to write them out in full.

![Opening and filtering the solution tree.](/Art/Demo1_low_640.gif)


## One click to open file

As a developer I got annoyed at VS4Mac for all of that double clicking a file on the solution tree to view it. Thus this feature.

![Activating OneClick.](/Art/Demo2_low_640.gif)


## Projects to expand

Is your filtering still showing a lot of solutiontree entries, mayby you can filter those projects that you only care about. Like if you're only working on iOS, you only want to see the core and iOS projects. Then specify: `core ios` and hit apply or change the tree filter.


<!--
## Installation

1. Download `.mpack` file from [Releases](https://github.com/IvoKrugers/EssentialsAddin/releases)

2. Go to the Extension manager via menu `Preferences -> Extensions` and use the "Install from File" button to install

3. Close Visual studio and re-open it.

The filter pad can be opened from the `Tools -> Essentials` or `View -> Pads` menu

If you have a previous version installed, uninstall it first, close Visual studio and re-open the app before installing the new version.
-->

## Installation

1. Open Visual Studio Extension Repository Manager via `Visual Studio -> Extensions... -> Gallery -> Repositories Dropdown -> Manage Repositories`.

2. Add `https://raw.githubusercontent.com/IvoKrugers/EssentialsAddin/master/mpack/main.mrep` to your repository sources.

3. Back in extension manager, you should now be able to see all the extensions available for installation.

**Note:** Installing an extension might require a restart of Visual Studio for Mac.

Any future updates to installed extensions should show up in the `Updates` tab of the Extension Manager.

## Authors

**Ivo Krugers** - Author

## Thanks
[Arthur Demanuele](https://github.com/ademanuele) for extension repo example

## License

This project is licensed under the MIT License - [full details](LICENSE).
