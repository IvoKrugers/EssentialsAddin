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


## Installation
Installation still has to be done by the "Install from File" functionality in `Preferences -> Extensions`. Releases are found [here](/Releases)
After installing from file in the extension manager, close Visual studio and re-open it.
The filter pad can be opened from the `Tools -> Essentials` or `View -> Pads` menu

## Authors

**Ivo Krugers** - Author

## License

This project is licensed under the MIT License - [full details](LICENSE).