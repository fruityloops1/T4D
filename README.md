# TheFourthDimension-Quality Of Life
A **Super Mario 3d Land** level editor, originally made by [Exelix11](https://github.com/exelix11).

This is a continuation of Exelix' [project](https://github.com/exelix11/TheFourthDimension) trying to improve it as much as possible.


## Features
- Edits every 3d Land level and every file under romfs/StageData/
  - Stage collision editor
  - Camera editor
  - Rail editor
  - Fog editor
  - Worldmap editor
  - Area editor
  - Music changer (assign a song to a level)
- 3d Previews of most assets
- byml <-> xml 
# New features
- Improvements to general user experience
  - Scale, position and rotation show all the axis together
  - Added GUI for these 3 properties
  - Now you can switch between comma and dot for decimals (unless you're editing an xml/byml)
  - Shortcuts for various operations
- The camera editor now has more options
- Added more model importer options
- Added translation base so the program can be easily translated

## Usage
Extract your Super Mario 3d Land romfs folder using citra or godmode9 and load it following the program's steps, then open your level and edit as you like!

## Roadmap
### v 0.9.5.0
- [X] Improve ui for settings, menus etc; make everything use the t4d icon instead of generic window icon
- [x] Add more bgms to bgmselector
- [x] Add more options for the model importer (object creator)
- [x] Add more options for the cameras (Rail, fixed, etc)
- [x] Add GenerateChildren C0list option button in the add property/it's own button so it's less tedious
- [x] Change Romfs folder selection to use file selection style instead of the old window
- [x] Force the apps to use . or , depending on the settings

### v 0.9.6.0
- [x] Create Rotation and Position GUI
- [x] Translation (at least a base for it)

### Up to v 1.0.0.0
- [x] Fix rendering issues
- [ ] General improvements like worldmap editor and better object database
- [ ] Dark theme
- [ ] Bugfixing


## TODO->Near future
- [x] Translation + some way to switch translations in the program (no redownload)
- [ ] Improve camera (?)
- [x] Less intrusive messages
- [x] X,Y and Z all in the same dropdown for direction, position and scale
  - [x] Scale
  - [x] Direction
  - [X] Position
- [x] More Hotkeys -> Ctrl + S to save and Ctrl + O to open
- [ ] Collision importer gui
- [ ] Revamp ui (move messy stuff and remove other things)
  - [x] Position, Rotation and Scale visual editors (similar to the one found in other editors like Quad64)
  - [x] Make options it's own window and not a weird blue box
  - [ ] Dark theme
  - [ ] Remove deprecated programs
  - [ ] Improve ObjectDB formatting and usage 
- [ ] Change old links 
- [x] Change the model renderer or improve current one to fix clipping and rendering issues


## TODO->Long term
- [ ] Better model support (bcmdl and fbx)
- [ ] Better documentation for everything
  - [x] Finish documenting every object
  - [x] Camera functions
  - [x] Rail types
  - [ ] Rail arguments
  - [x] Layer usage
  - [x] Area usage
  - [ ] Area arguments


## Building

Use Visual Studio (not Code) with the .NET framework (specifically 4.5.2).
It should load fine so just select `The4Dimension.sln` and then build.


## Join Us
If you need help with Super Mario 3D Land Hacking, you can join the Tanooki Tree  <a href="https://discord.gg/CXJgeUk"><img src="https://img.shields.io/discord/308323056592486420.svg?color=7289da&logo=discord&logoColor=white" alt="The Cat Chat" /></a> <br/>(*Disclaimer: We cannot help you get the 3D Land files*)

