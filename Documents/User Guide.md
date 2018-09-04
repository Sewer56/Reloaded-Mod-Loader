<div align="center">
	<h1>Reloaded For Users</h1>
	<img src="https://i.imgur.com/gEeMBq2.png" align="center" />
	<br/> <br/>
</div>

## Page Information

🕒 Reading Time: 05-15 Minutes

💯 Difficulty Level: 0/5

## Table of Contents
- [Prerequisites](#prerequisites)
- [Downloading](#downloading)
- [Navigation Tips](#navigation-tips)
- [Adding Games](#adding-games)
- [Enabling Mods](#enabling-mods)
  - [Setting Mod Order](#setting-mod-order)
  - [Long Mod Lists](#long-mod-lists)
- [Configuring Controller Inputs](#configuring-controller-inputs)
  - [Preview & Controller Ports](#preview-&-controller-ports)
  - [Axis](#axis)
- [Alternate Launch Dialog](#alternate-launch-dialog)
- [Installing Plugins](#installing-plugins)
- [Troubleshooting](#troubleshooting)

## Prerequisites
* Windows 7, 8.1, 10 or newer.
* .NET Framework 4.7.2+

## Downloading
- Download Setup.exe from the Releases section on Github (or wherever found).
- Now you're done (* ^ ω ^), that's it.

## Navigation Tips
- You can switch tabs using the mouse back and forward buttons.
- For lists, such as the game list in main menu, you can use the scroll wheel while hovering over the list to change current selection.
- For lists which support drag and drop reordering (mods menu mod list), you can hold CTRL while using the scroll wheel to rearrange current selection.

## Adding Games
As Reloaded is a universal loader, the first thing you need to do for your game is to manually add it to
your games list for the game to launch later, to do this, enter the Manage Menu.

<center>
	<img src="https://i.imgur.com/AkZgZcK.png" width="500" />
</center>

From there you should add your game to the list of games, by pressing the new game button, when prompted to select a directory, you should right click and create a new directory for your game, then select the directory:

<center>
	<img src="https://i.imgur.com/pBaIRGs.png" width="500" />
</center>

You will then land with a new profile in the drop down menu, your game profile to complete:

<center>
	<img src="https://i.imgur.com/7rYTTMj.png" width="500"/>
</center>

From there on, you simply fill the empty fields from the top to the bottom. The first two fields are optional while the remaining three fields in the bottom must be filled. Click on the gear icons to bring up prompts to select a file, unless you want to type the paths manually.

For Mod Directory, create a new directory just like after hitting `New Game`. You can also choose an existing directory to combine mods with another game, or a subdirectory in a game directory, your choice.

The `EXE RELATIVE PATH` is the relative path of your executable to the field below which specifies `Game Directory`, to best provide an example, here's an already existing pre-filled profile:

<center>
	<img src="https://i.imgur.com/slwN1aO.png" width="500"/>
</center>

The folder of the game is specified under `Game Directory` and the executable is a file inside it named `sonic2app.exe`, the actual executable location (in my case) is `F:\Games\Steam\Windoze\steamapps\common\Sonic Adventure 2\sonic2app.exe`.

You may also add a banner to your game profile by clicking the empty box that would otherwise contain the banner image, the image should be a PNG file and its resolution is 273x85.

## Enabling Mods

In order to enable mods, you first need to go back to the main tab and highlight the game to enable mods for in the main menu.

Then simply at the top of the launcher, hit the `Mods` category to enter the mods menu:

<center>
	<img src="https://i.imgur.com/uIEUGsm.png" width="500"/>
</center>

To enable mods, simply click the `[-]` button of a mod entry, which will set it to an enabled state `[+]`.

### Setting Mod Order

The order of mods to be loaded with Reloaded in the mods menu can be changed by dragging a mod entry and dropping it over in the place of another entry (in the desired place), alternatively, you may hold the CTRL key and use the scroll wheel to perform the same operation.

Reloaded loads mods in the bottom to top order as seen in the Mods menu, with the mods at the bottom being loaded first and the mods at the bottom being loaded last.

The order is as such because in the case of multiple code mods, this means that mods which apply "function hooks" to modify game functions perform this last and get priority over other hooks.

### Long Mod Lists
If your mod list extends down beyond the box (you have a lot of mods), you can access it by scrolling down the list using the mouse wheel while hovering over it, or using the arrow keys to navigate the lists.

The lists are cyclic, if you press up at the first entry, it will take you to the last entry, if you press down at the last entry, it will take you to the first entry.

## Configuring Controller Inputs

Reloaded also hosts its own input system, allowing for mods to more easily receive controller inputs rather than having for modifications to read it from a game's memory (or in some cases not supporting controllers at all).

To configure controllers to be used, select the `Input` tab in Reloaded-Launcher, you will be greeted with this screen:

<center>
	<img src="https://i.imgur.com/gZ5Tx79.png" width="500"/>
</center>

To add a button mapping for a controller, double click the button/axis mappings list entry and press a button on the currently selected controller. If you would want to discard the button or axis, double click and wait for the timeout to complete (or alternatively select it and press the middle mouse button).

Lists can once again be scrolled using the arrow keys or mouse wheel.

### Preview & Controller Ports
With Reloaded, it should be noted that the mod loader takes a special and somewhat uncommon approach to controllers when it comes to port allocation. 

With Reloaded, you are allowed to assign multiple input devices such as controllers to a specific port and all of the devices will be seen by the individual mods as one controller from the point of view of the mod. The controller port for the current device may be changed by changing the number on the top right of the screen beside the controller name.

The controls on the top right of the screen show the current preview for all controllers attached to the specific controller port as it would be seen by the mods, this may include other controllers attached to the same port.

### Axis
Axis assignments and customization is a bit unconventional but very, very powerful.

Internally, Reloaded behaves and assumes a controller layout of a 360 controller, but that controller can be assigned in many, many different ways as already experienced with button assignments.

An axis consists of a `Source` and a `Destination`, the source declares where to get the value for the axis from the controller and the destination declares to which virtual Reloaded controller the axis should be applied.

To set an axis, doubleclick the Axis Source column and move an axis such as a trigger or stick, then assign the destination axis stating to which virtual axis the value should map to.

As an example, to set the left stick for a DirectInput controller, I double click the Axis Source entry for "Left Stick X" and move my stick to the right, then double click the destination axis box and set the value to LeftStickX, now immediately, the preview on the top right of the screen is able to see the left and right movements of my stick - repeat for remaining axis.

## Alternate Launch Dialog
This is included more for debugging purposes and finding issues but can be nonetheless useful.
In the main menu screen, if you hold the left CTRL key while pressing the "Launch" button, you will be greeted with the following hidden dialog:

<center>
	<img src="https://i.imgur.com/dIWXyAp.png" width="400"/>
</center>

From here, you can either load Reloaded into an already running game by pressing the `Attach` button or launch Reloaded while also dumping a copy of the console window when the game is launched by ticking the Log File Location checkbox.

## Installing Plugins

Starting with version 2.00, Reloaded provides basic plugin support for those who wish to extend the functionality of the loader and/or launcher. The idea of plugin support in Reloaded is mostly "experimental" and plugin support currently available will be improved and/or extended should plugins find an increased demand or popularity.

### How Plugins Work
Plugins function mostly the same as mods, except without support for plugin order, updates or dependencies. They can be installed by dropping directories directly into `AppData\Roaming\Reloaded-Mod-Loader\Reloaded-Config\Plugins` folder. 

### Limitations & Troubleshooting Plugins
There is no list of enabled/disabled plugins, at the current moment, each plugin states whether it should be enabled or disabled via its own `Config.json` configuration file.

In the case of a rogue plugin that crashes and prevents you from operating the launcher properly; you are required to disable it explicitly through the plugin's config file.

## Troubleshooting

#### Failed to start ReloadedProcess Undefined\Undefined ...

The path to the individual game has not been set in the Manage tab, thus defaults to "Undefined".
In most cases, chances are you just forgot to hit the `Save Game Settings` button in the user interface.

In extremely, near impossible cases - you may be on some super locked down system and not have write access to `AppData\Roaming\Reloaded-Mod-Loader`, in which case either Run As Administrator (if you can), or seek the nearest administrator.

Reloaded is incapable (nothing is) of automatically scanning contents of all connected storage devices and magically determining
the correct executables for your arbitrarily added games.

#### Failed to start ReloadedProcess (Valid Executable Path)

Try running Reloaded-Launcher (the Reloaded Mod Loader shortcut on your desktop) as an Administrator.

In some cases you may find that your user may just simply not have the correct priviledges to access a file where your game is installed available to other programs, as may be the case with `Program Files` for some people.

#### Alternate Launch Dialog - Attach Fails

Attaching and loading mods into an already running process may sometimes fail in the cases that you do not have equal or greater
priviledge level as the process/user which started the game/application you are attaching to.

i.e. If you run your game as administrator and want to attach to it, you must start Reloaded as administrator.

#### Reloaded fails to launch for an unexplicable reason.

Check your Antivirus software. Due to the nature of how Reloaded operates (Dll Injection, packing Reloaded-Assembler into libReloaded.dll), it is relatively easy for antivirus software to mistaken Reloaded for malware or other questionably moral software.

I generally, when I can, send tickets to AV Companies whenever it is easy to do so/possible when I hear of false detections. I encourage you to do the same should you ever receive a false positive (please link back in false detection details/descriptions to the Github source code page).

At the moment, the only two official sources of Reloaded's distribution are on this Github page (https://github.com/sewer56lol/Reloaded-Mod-Loader) and GameBanana (https://gamebanana.com/tools/6424).
