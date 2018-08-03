## Page Information

🕒 Reading Time: 10 Minutes

💯 Difficulty Level: 4/5

### Can I use Reloaded Mod Loader alongside [X] Mod Loader [Technical Edition]
If you want just the simple answer, see the user guide.
tl;dr 99.9% of the cases everything will work perfectly with you having to do 0% work.

For a more complete and technical answer: Although Reloaded was built from the ground up to be able to work with other loaders, there are some things that can never be perfect, the one and only main potential problem of achieving perfect interoperability would be function hooks.

Basically, perfect interoperability depends on the mod loader and that individual loader's hooking mechanism. 
For the most part, you can break it all up into two individual cases:

####  1. Default Case (X Loader Hooks Reloaded)
Most of the time other mod loaders will be hooking onto Reloaded Hooks.

This is because of Reloaded's unique approach, starting and injecting into a suspended process allowing for mods to initialise and execute your own init code before the game even starts running any instructions.

Unfortunately, here we are at the mercy of the other mod loader in question.
If the other mod loader in question is any good, it (probably) includes some disassembler to auto calculate the amount of bytes to steal for the hook, if it does, we are all good, 100% of the time.

If the other mod loader in question does not have such feature implemented, there exists a *very rare* potential case of failure if the user specified hook length is lesser than 6 bytes (normal minimal hook lengths are 5 bytes, very rare).

*By design, in the name of interoperability, Reloaded makes use of absolute rather than relative jumps behind the scenes. This is why other Mod Loaders can hook Reloaded hooked functions without a care in the world, when executing original bytes, it'll safely end up executing Reloaded function executing original bytes. Likewise Reloaded can hook other Reloaded Hooks - Stack Overflow's the limit!*

####  2. Alternate Case (Reloaded Hooks X Loader's Hooks)
As you may know, Reloaded also includes an alternative to launching the game with Reloaded, attaching to an already running live process.

This exists both as a workaround for mod loaders lacking auto-hook length detection, games which force a launcher requirement and for testing scenarios.

In this case, libReloaded's magic is also happy to kick in.
libReloaded is able to directly patch other mod loaders' and hooks' jump addresses back to the original function, allowing Reloaded to stack itself ontop of other mod loaders' hooks happily. The two most common formats of relative jump followed by nop and nop followed by relative jump are supported.

When a hook is performed, libReloaded will automatically examine the bytes to be stolen from the target function and search for any relative jumps (which would generally belong to another mod loader's function hook).
If a relative jump is found, libReloaded will then follow the jump target and look for return addresses to patch (to point to new address of end of hook/original function), depending on the target location. 

If the target location is part of module code (e.g. SADXMod.dll), libReloaded will scan the entire module looking for jump back addresses to patch.

If the target location is dynamically allocated memory instead, libReloaded will scan the entire page the target is located in, looking for return addresses.

This is why for example Reloaded can hook DirectX11's Present function with the Steam Overlay running while most other loader couldn't, without any hardcoding at all.

*Note: libReloaded suspends the process when a function hook is about to take place. You do not need to fear program execution being in the middle of another hook during Reloaded's hooking procedure.
To preserve interoperability, no unhooking mechanism is provided. If you want to hook no more, just have your hook call the original function.*

#### Extra Super-Rare Case of Failure by Bad Design
*Hook unhooks itself to call the original function before re-hooking at the end of it (see Rivatuner Statistics Server).
Please, in the name of good lord, if you are writing a mod loader, don't do this, I've seen it even break some engines, let alone mod loaders.*
