
<div align="center">
	<h1>Reloaded's Pseudo-Launcher</h1>
	<img src="https://i.imgur.com/TbCUc05.png" align="center" />
	<br/> <br/>
</div>

## Page Information

🕒 Reading Time: < 5 Minutes

💯 Difficulty Level: 0/5.


## Table of Contents
- [Introduction](#introduction)
  - [How it works.](#how-it-works)
  - [Installing the launcher.](#installing-the-launcher)
      - [A: Executable as a launcher replacement](#a-executable-as-a-launcher-replacement)
      - [B: As a Steam Executable replacement](#b-as-a-steam-executable-replacement)
      - [For both Option A and Option B:](#for-both-option-a-and-option-b)
  - [Benefits](#benefits)
  - [Limitations](#limitations)
  - [Extra Notes](#extra-notes)


## Introduction

Sometimes, there games that require to be launched through a launcher. Typically all these games do is simply check whether the launcher is alive to confirm whether the game has been launched through the launcher and shut the launcher down on boot, else throw an error message.

Starting with version 2.00, Reloaded gains a new ability. You can now drop a pseudo-launcher that will automatically load another executable inside the same folder through Reloaded, providing a Reloaded profile is created for the executable.

Very often, this simple trick allows you to skip the launcher entirely and load a game with a forced tacked on launcher.

### How it works.
Under the hood, the pseudo-launcher reads all Reloaded game configurations and filters out the ones that are either in the same folder, subfolders or... for brevity **parent folders at any level**.

If only one config is found, it is automatically launched - else the user is asked in a simple menu in terms of which game to launch.

![Exhibit B](https://i.imgur.com/nkWIexT.png)

*When there are multiple found games, it looks like this.*

### Installing the launcher.

In order to install the launcher, the you are required to press the "Generate Launcher" button inside Reloaded's GUI.

Doing this should copy the pseudo-launcher to a folder in the user's `TEMP` directory and open the folder - complete with the executable to drop into the game folder and a file named `Instructions.txt`.

![Instructions](https://i.imgur.com/6DqXJVL.png)
![Instructions 2](https://i.imgur.com/juvYZAl.png)

And now, we have two options.

##### A: Executable as a launcher replacement
In the case of a game like Shenmue, we would like to use the pseudo-launcher, as a launcher replacement. In this case we simply copy `Shenmue.exe` over Shenmue's own launcher, `SteamLauncher.exe`.

##### B: As a Steam Executable replacement
As an alternative to [Reloaded and Steam Games "Reattaching"](https://github.com/sewer56lol/Reloaded-Mod-Loader/blob/master/Documents/Reloaded%20and%20Steam%20Games%20Reattaching.md), you can simply rename the game's original executable and put the pseudo-launcher with the name of the old execuable.

##### For both Option A and Option B:
![Instructions](https://i.imgur.com/6DqXJVL.png)

The individual game profiles should not point their executables at the launcher but instead at the executables of the actual game/s.

### Benefits

- The launcher can fool a bunch of games tricking them into believing they were launched from a launcher - saving you reverse engineering effort to patch that code.

- The pseudo-launcher can be used as an alternative to the `steam_appid.txt` method described in [Reloaded and Steam Games "Reattaching"](https://github.com/sewer56lol/Reloaded-Mod-Loader/blob/master/Documents/Reloaded%20and%20Steam%20Games%20Reattaching.md) to allow booting Steam games directly without having to reattach as the game restarts itself via Steam.

### Limitations

- [Option B specific] Performing a Steam game update or Steam file verification will likely wipe/remove the pseudo-launcher in which case the user would have to reapply it **else the game will not load with mods at all**.

### Extra Notes

The pseudo-launcher install process is not automated because the creator of Reloaded wishes to have the user aware of what is happening.
