
<div align="center">
	<h1>Modifications: Getting Started</h1>
	<img src="https://i.imgur.com/OnausdF.png" align="center" />
	<br/> <br/>
</div>

## Page Information

🕒 Reading Time: 5 Minutes

💯 Difficulty Level: 1/5

## Table of Contents
- [Modification Structure](#modification-structure)
  - [Config.json](#configjson)
  - [Banner.png](#bannerpng)
- [File Redirection Modifications](#file-redirection-modifications)
  - [File Redirection Modifications: Extra Info](#file-redirection-modifications-extra-info)
- [Building Code/Script based Modifications](#building-codescript-based-modifications)
- [Distributing Modifications](#distributing-modifications)
  - [Third Parties](#third-parties)
  - [Third Parties: User's End](#third-parties-users-end)
  - [Distributing Updates (Default Plugins)](#distributing-updates-default-plugins)
      - [Github](#github)
      - [GameBanana](#gamebanana)
  - [Third Party Specifics](#third-party-specifics)
      - [GameBanana](#gamebanana)

## Modification Structure

At the core, Reloaded Modifications consist of three components:

```
Config.json: Defines the modification properties such as name, description and version.
Banner.png: Preview image for the Reloaded-Launcher.
main32/main64.dll: Used for code mods only. Contains custom C# user code.
```

### Config.json
The typical configuration file is defined in a JSON file as follows, you simply should fill the following fields.
```json
{
	"ModId": "reloaded.global.fileredirector",
	"ModName": "[X86/X64] Reloaded File Redirector",
	"ModDescription": "Redirects loading of game files from your game directory to files inside Plugins/Redirector/ in enabled mod folders.",
	"ModVersion": "1.0.0",
	"ModAuthor": "Sewer56",
	"ModSite": "https://github.com/sewer56lol/Reloaded-Mod-Loader",
	"ModSource": "https://github.com/sewer56lol/Reloaded-Mod-Loader",
	"ConfigurationFile": "",
	"Dependencies": []
}
```

- ModId: Can be any arbitrary piece of text or value, just make it unique. Once set, it should not be changed.

- Dependencies: A list of other mods' Mod IDs that require to be loaded for your mod to function. When set, any dependencies of a mod will always be loaded before the mod itself.

- ConfigurationFile: A name of a file to be opened inside the mod directory when the user selects "Configuration" in Reloaded-Launcher. Can be an executable, ini or any other file kind.

### Banner.png
The size of the image is 271x271 pixels, as displayed in Reloaded-Launcher.

![Sample Image](https://i.imgur.com/Yg960UU.png)

## File Redirection Modifications
Due to Reloaded's quest to modularity, all features that actually do the tampering with games are enabled inside mods rather than at the core with the loader. As such, Reloaded's file redirection capabilities are implemented in a mod itself (which ships with all Reloaded copies) as seen below:

<center>
	<img src="https://i.imgur.com/0oeTNXr.png" align="center" width="500" />
</center>

As such, for file redirection based mods you should specify a dependency on the mod above:
```json
"Dependencies": [ "reloaded.global.fileredirector.new" ]
```
(Originally `reloaded.global.fileredirector`, now replaced with a new implementation)

For files to be loaded by the game with the mod enabled, create the folders`Plugins/Redirector` in your mod directory; i.e. you create the folder `Plugins`, and inside that folder `Redirector` and place your files there.

To be strictly precise, the folder in `Plugins/Redirector` maps to individual game profile's `Game Directory` as seen in the launcher below:

<center>
	<img src="https://i.imgur.com/a4BjzRP.png" align="center" width="500" />
</center>

That's all you need to know.

[Here's an example.](https://gamebanana.com/skins/162715)

### File Redirection Modifications: Extra Info

Files are not mapped by filename but their location relative to the game folder, thus in this scenario, to replace a music file at `E:/Projects/Sonic Hacking Stuff/SonicHeroes/Game/dvdroot/bgm/SNG_STG26.adx` your mod should place the file at `Plugins/Redirector/dvdroot/bgm/SNG_STG26.adx`.

The reason game directory in Reloaded is a separate field to the executable, path is due to the occasional games that store their executables in subfolders to the game's root directory.

For example Rocket League stores the executable in `Binaries/Win32` relative to the game directory while the files are at `TAGame/CookedPCConsole`. 

It is your job as communities to agree on the folder to use, although in most cases it will be the same folder that contains the executable (auto-filled when adding an executable path in Reloaded-Launcher).

## Building Code/Script based Modifications

See Reloaded for Developers.


## Distributing Modifications
In order to distribute your modifications, you simply make an archive with the directory containing the modification inside, that's all that's required.

Your archive file should look as such:

<center>
	<img src="https://i.imgur.com/6W1O2nq.png" align="center" width="500" />
</center>

*The empty RELOADED file at the archive is not necessary and is ignored by the loader, it is third party specific [GameBanana] (see below).*

### Third Parties
Reloaded-Launcher has a custom URI handler and can download archived mods from the web if they begin with `reloaded:`.

Here is an example (not a real link): `reloaded:https://gamebanana.com/mmdl/example123456`

Formats supported include 7z, RAR and ZIP, among a few others.

### Third Parties: User's End
 
Due to Reloaded's nature of being a universal loader, there is no specific detection of the game title in question. The user is required to themselves explicitly specify the game the mod should be installed to.

<center>
	<img src="https://i.imgur.com/C9cY3cc.png" align="center" width="300" />
</center>

### Distributing Updates (Default Plugins)

##### Github 

The plugin looks for updates in Github repositories linked in the ModSource member of each mod's config.json.

```csharp
"ModSource": "https://github.com/sewer56lol/Heroes-Graphics-Essentials",
```

Once that is setup, pushing new updates is easy. In order to push a new update, simply create a new `Release` on Github.

The plugin will look for a file that starts with `Update-` and download it when the publish time is newer than the last time Config.json was edited.

![](https://i.imgur.com/8H9WHAU.png)

##### GameBanana

You don't need to do anything. Literally.
Just add a new update to the submission on the site and the plugin will show a new update.
![](https://i.imgur.com/Y0QMf9S.png)

*Note: This only functions with mods downloaded through 1 click links at the moment.*
*Note 2: The functionality is currently be inactive until a stable release of Reloaded 2.00. Coming to you very soon!*

### Third Party Specifics

##### GameBanana

In order to be successfully picked up as a Reloaded mod on the site and gain a 1-click install, include an empty file with no extension named RELOADED at the root of archive.

If a 1 click link does not appear and/or you are the first person to submit a Reloaded mod for a specific game, let either myself or a member of GameBanana site staff know.

Currently due to implementation, for performance reasons validation against mod loaders is done on a game to game basis and individual games need to be enabled for individual mod loaders.

