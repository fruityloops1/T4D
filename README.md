# TheFourthDimension-Quality Of Life
A **Super Mario 3d Land** level editor, originally made by [Exelix11](https://github.com/exelix11).

This is a continuation of Exelix' project trying to make this a better editor.


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


## TODO->Near future
- [ ] Improve camera
- [ ] Less intrusive messages
- [ ] Improve ObjectDB formatting and usage
- [ ] X,Y and Z all in the same dropdown for direction, position and scale
- [x] More Hotkeys -> Ctrl + S to save and Ctrl + O to open
- [ ] Collision importer gui
- [ ] Revamp ui (move messy stuff and remove other things(the Help dropdown makes no sense ;^; ))
- [x] Change old links (looking at you [neomariogalaxy](http://neomariogalaxy.bplaced.net/objectdb/3dl_download.php))
- [ ] Backface culling

## TODO->Distant future (lmao)
- [ ] Translation + some way to switch translations in the program (no redownload)
- [ ] Better GFX (better renderer or better model support, bcmdl, dae, fbx...)
- [ ] More... :shushing_face:


## Building

Use Visual Studio (not Code) with the .NET framework (specifically 4.5.2).
It should load fine so just select `The4Dimension.sln` and then build.
