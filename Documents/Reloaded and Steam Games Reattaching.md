
<div align="center">
	<h1>Reloaded and Steam Games "Reattaching"</h1>
	<img src="https://i.imgur.com/TbCUc05.png" align="center" />
	<br/> <br/>
</div>


## Page Information

🕒 Reading Time: < 5 Minutes

💯 Difficulty Level: 0/5.

# Table of Contents

- [Introduction](#introduction)
- [Why this happens? (Simplified)](#why-this-happens-simplified)
- [Solution: Explicitly Setting Steam Application ID](#solution-explicitly-setting-steam-application-id)
  - [Step by Step Tutorial](#step-by-step-tutorial)
- [Alternative Solution: Using the pseudo launcher.](#alternative-solution-using-the-pseudo-launcher)

## Introduction

You may have noticed that running a large amount of Steam games directly led to the game restarting itself, and Reloaded's loader having to reattach to it.

*When this event occurs - the loader console might resemble this:*

![Exhibit A](https://i.imgur.com/l6oadNt.png)

*The text may vary in earlier versions of Reloaded*.

In general this behaviour is not harmful and neither a problem; but it causes Reloaded to lose one key feature that it had over alternative mod loaders; **loading and initializing all mods before the game ran any code**.

## Why this happens? (Simplified)

Many games on the PC platform make use of the Steamworks API, which allows them to perform things such as pausing the game when the Steam Overlay, is open inviting friends to play, allowing players to unlock Steam Achievements and various other activities.

There is however a caveat; in that some of the functions of Steamworks require a running Steam client which *believes it is running the game*. To specifically quote:

`A running Steam client is required to provide implementations of the various Steamworks interfaces.`

To ensure that these functions are available, games often make use of a Steam API function named [SteamAPI_RestartAppIfNecessary](https://partner.steamgames.com/doc/sdk/api#SteamAPI_RestartAppIfNecessary), which restarts the application through Steam if the local Steam client doesn't believe it is running the game.

## Solution: Explicitly Setting Steam Application ID

One solution around this problem, originally intended for developers is to manually specify the Steam AppID for the application - which the Steam API will read from the game directory instead of kindly asking Steam.

### Step by Step Tutorial
1. Right click the game in your Steam Library and press the `Create Desktop Shortcut` button.
![](https://i.imgur.com/Ju1yYWC.png)
2. Right click the shortcut and in the properties window, extract the AppID from the URL.
![](https://i.imgur.com/ZKidaJb.png)
3. In the same folder as the game's executable; create a file named *steam_appid.txt*, inside the file, save the number you extracted earlier. 


![](https://i.imgur.com/eK3lovo.png)
![](https://i.imgur.com/Fjgrq3Y.png)

You are done. In addition, you can bypass some game launchers this way entirely - allowing you to make direct shortcuts to certain games.

## Alternative Solution: Using the pseudo launcher.
See [Reloaded's Pseudo Launcher](https://github.com/sewer56lol/Reloaded-Mod-Loader/blob/master/Documents/Reloaded%20Pseudo%20Launcher.md).
