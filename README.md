# TheFourthDimension-Quality Of Life
A **Super Mario 3d Land** level editor, originally made by [Exelix11](https://github.com/exelix11).

This is a continuation of Exelix' [project](https://github.com/exelix11/TheFourthDimension) trying to make this a better editor.


## Features
- Edits every 3d Land level and every file under romfs/StageData/
  - Stage collision editor
  - Camera editor
  - Rail editor
  - Area editor
  - Music changer (assign a song to a level)
- 3d Previews *(very bad ones rn)* of most assets
- byml <-> xml 


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
- [ ] Create Rotation and Position GUI
- [ ] Dark theme
- [ ] Translation (at least a base for it)

### Up to v 1.0.0.0
- [ ] Fix rendering issues
- [ ] Minor stuff?
- [ ] Bugfixing


## TODO->Near future
- [ ] Improve camera (?)
- [x] Less intrusive messages
- [x] X,Y and Z all in the same dropdown for direction, position and scale
  - [x] Scale
  - [x] Direction
  - [X] Position
- [x] More Hotkeys -> Ctrl + S to save and Ctrl + O to open
- [ ] Collision importer gui
- [ ] Revamp ui (move messy stuff and remove other things)
  - [ ] Position, Rotation and Scale visual editors (similar to the one found in other editors like Quad64)
  - [x] Make options it's own window and not a weird blue box
  - [ ] Dark theme
  - [ ] Remove deprecated programs
  - [ ] Improve ObjectDB formatting and usage 
- [x] Change old links (looking at you [neomariogalaxy](http://neomariogalaxy.bplaced.net/objectdb/3dl_download.php))
- [ ] Change the model renderer or improve current one to fix clipping and rendering issues
- [ ] Better documentation for everything
  - [ ] Finish documenting every object
  - [ ] Camera functions
  - [ ] Rail types
  - [ ] Rail arguments
  - [ ] Layer usage
  - [ ] Area arguments


## TODO->Long term
- [ ] Translation + some way to switch translations in the program (no redownload)
- [ ] Better GFX (better renderer or better model support, bcmdl, dae, fbx...)
- [ ] More (?)


## Building

Use Visual Studio (not Code) with the .NET framework (specifically 4.5.2).
It should load fine so just select `The4Dimension.sln` and then build.


## Join Us
If you need help with Super Mario 3D Land Hacking, you can join the Tanooki Tree  <a href="https://discord.gg/CXJgeUk"><img src="https://img.shields.io/discord/308323056592486420.svg?color=7289da&logo=discord&logoColor=white" alt="The Cat Chat" /></a> <br/>(*Disclaimer: We cannot help you get the 3D Land files*)

