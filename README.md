
<div align="center">
	<h1>Project Reloaded</h1>
	<img src="https://i.imgur.com/BjPn7rU.png" width="150" align="center" />
	<br/> <br/>
	<strong>All your mods are belong to us.</strong>
	<p>Experimental, C# universal mod loader framework compatible with arbitrary processes.</p>
</div>


# Introduction
**[Reloaded]** is an actively developed DLL Injection based Mod Loader, Mod Management System, Optional Mod SDK *(libReloaded)* among various other utilities. 

<div align="center">
	<img src="https://i.imgur.com/aG6rXm9.png" width="550" align="center" />
	<br/><br/>
</div>

It is a completely free and open source public rewrite of **Heroes Mod Loader**, my original first attempt at a mod management/loader system all the way from the ground up using the C# programming language.

At the time of writing of this readme, Reloaded has currently been in development for 5 months, plus the extra time spent on the predecessing mod loader, this included me learning many new aspects of hacking and reverse engineering from the ground up.

# Table of Contents
- [Features](https://github.com/sewer56lol/Reloaded-Mod-Loader/blob/master/Documents/Features.md)
- [User Guide](https://github.com/sewer56lol/Reloaded-Mod-Loader/blob/master/Documents/User%20Guide.md)
- [Getting Started: Modifications](https://github.com/sewer56lol/Reloaded-Mod-Loader/blob/master/Documents/Getting%20Started%20Modifications.md)
- [Getting Started: Programmers](https://github.com/sewer56lol/Reloaded-Mod-Loader/blob/master/Documents/Getting%20Started%20for%20Developers.md)
- [Project Structure](https://github.com/sewer56lol/Reloaded-Mod-Loader/blob/master/Documents/Project%20Structure.md)

# On Cheating in Multiplayer Games

I've had one or two people ask me whether they can use the library safely in online game X/Y.

The only answer I may give to this is be cautious and tread wisely.

Reloaded does not make any kinds of attempts to disguise itself among other applications and uses standard, common methods of DLL Injection. As a result of this, there is a good chance that Reloaded is fairly easily detectable by function signatures (and also throw false positives by AV Scanners). 

In addition, I do not personally support or commit to cheating in online, competitive games. If necessary, or things go out of hand - I may even report my project/modules to get it blacklisted from anti-cheat solutions and vendors. As universal as Reloaded is, I do not wish for it to belong in the competitive multiplayer space.

While I cannot stop you, I would advise that If you *really* want to cheat, or write cheats for online games, you should write your own private solutions without using the Reloaded libraries and not share them with the rest of the world.

Needless to say, you risk getting banned. It's not worth it.
