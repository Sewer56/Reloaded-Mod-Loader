
<div align="center">
	<h1>Modifications: Getting Started</h1>
	<img src="https://i.imgur.com/OnausdF.png" align="center" />
	<br/> <br/>
</div>

## Modification Structure

At the core, Reloaded Modifications consist of three components:

```
Config.json: Defines the modification properties such as name, description and version.
Banner.png: Preview image for the Reloaded-Launcher.
main32/main64.dll: Used for code mods only. Contains custom C# user code.
```

### Config.json
The typical configuration file is defined as follows, you simply require to fill the following fields.
```json
{
  "ModName": "Reloaded Template Mod",
  "ModDescription": "Reloaded Template which prints \"Hello World\" to the console output on launch.",
  "ModVersion": "1.0.0",
  "ModAuthor": "Sewer56lol",
  "ThemeSite": "https://github.com/sewer56lol/Reloaded-Mod-Loader",
  "ThemeGithub": "https://github.com/sewer56lol/Reloaded-Mod-Loader",
  "ModConfigExe": "N/A"
}
```

### Banner.png
The size of the image is 271x271 pixels, as displayed in Reloaded-Launcher.

![Sample Image](https://i.imgur.com/Yg960UU.png)

## File Redirection Modifications
Due to Reloaded's quest to modularity, all features that actually do the tampering with games are enabled inside mods rather than at the core with the loader explicitly implementing them, as such, Reloaded's file redirection capabilities are implemented in a mod itself that must be enabled, you should make sure that the user knows about this:

<center>
	<img src="https://i.imgur.com/0oeTNXr.png" align="center" width="500" />
</center>

Nonetheless, packing file redirection mods is quite easy, all you are required to do is include and complete `Config.json` and `Banner.png` as aforementioned.

For files to be loaded by the game with the mod enabled, you create the folders `Plugins` and inside that `Redirector`, game files to be replaced as the game executable loads them are placed there.

The folder `Plugins/Redirector` will map to is specifically the game profile specified `Game Directory` as seen in the launcher:

<center>
	<img src="https://i.imgur.com/a4BjzRP.png" align="center" width="500" />
</center>

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


### Third Party Specifics:
**GameBanana**

In order to be successfully picked up as a Reloaded mod on the site and gain a 1-click install, include an empty file with no extension named RELOADED at the root of archive.

Should a 1-click install link be still missing after submission, then congratulations! You pioneer are the first to submit a Reloaded mod for a specific game on the site.

Consider contacting a member of the site staff or myself to add the game to Reloaded's game list on the site to enable 1-click Reloaded links for the game. Due to performance reasons, on the site's end, 1-click cannot be added on a site basis and needs to be added on a per-game basis instead as-is.

