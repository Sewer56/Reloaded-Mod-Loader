

<div align="center">
	<h1>Reloaded For Programmers</h1>
	<img src="https://i.imgur.com/HsLAGlQ.png" align="center" />
	<br/> <br/>
</div>

## Prerequisites
* Visual Studio 2017+
* .NET Framework 4.7.1+
* NuGet Package Manager (Fetching Dependencies)
* Git for Windows (with Git.exe available in PATH)

## Compiling Reloaded (As a whole)
 - Clone this repository and the submodules.
 ```bash
git clone https://github.com/sewer56lol/Reloaded-Mod-Loader
cd Reloaded-Mod-Loader
git submodule update --init --recursive
```
- Open 'Reloaded-Launcher.sln' in Visual Studio.
- Right-click solution in Solution Explorer and restore NuGet packages.
- Now you're done (* ^ ω ^), go play around!

## Mod Examples: Getting Started

Multiple samples related to mod development with Reloaded Mod Loader can be found under the [Reloaded-Mod-Samples](https://github.com/sewer56lol/Reloaded-Mod-Loader/tree/master/Reloaded-Mod-Samples) directory after you clone the Reloaded Loader.

The recommended inspection order for the samples is as follows:
1. Memory Manipulation `(Memory, Arrays, Allocation/Deallocation)`
2. Assembler Demo `(Real time "JIT" X86 and X64 assembly demo)`
3. Input Stack Demo `(Reading Reloaded Assigned Controllers/Devices)`
4. File Monitor `(Reloaded Hooking Demo)`

Extra Samples with libReloaded:
  - File Redirection: [Sample](https://github.com/sewer56lol/Reloaded-Mod-Loader/tree/master/Reloaded-Mod-Samples/File-Redirector)
  - Universal Borderless Windowed: [Sample](https://github.com/sewer56lol/Reloaded-Mod-Loader/tree/master/Reloaded-Mod-Samples/Universal-Borderless)
  - DX9 Internal Overlay: [Sample](https://github.com/sewer56lol/Reloaded-Mod-Loader/tree/master/Reloaded-Mod-Samples/DX9-Drawing)
  - WPF Overlay: [Sample](https://github.com/sewer56lol/Reloaded-Mod-Loader/tree/master/Reloaded-Mod-Samples/WPF-Test)
  - D2D + WPF Hybrid Extermal Overlay: [Sample](https://github.com/sewer56lol/Reloaded-Mod-Loader/tree/master/Reloaded-Mod-Samples/D2D-Drawing)

These should be enough to get you started.

To debug, insert a `Debugger.Launch()` line into your program, this will automatically provide you with a prompt to start or use running version of Visual Studio.

I would recommend having a source copy of libReloaded to allow you to become more familliar with the library, the examples only scratch the surface of each of the individual feature categories present in the library.

Reloaded is intended to be easy to pick up and learn, while at the same time being very powerful :).

## Starting Reloaded C# Projects
 - Clone the Reloaded Mod Template sources to a new folder.
 ```bash
git clone https://github.com/sewer56lol/Reloaded-Mod-Template
```
- Open .csproj in Visual Studio and restore NuGet packages.

Now you're done (* ^ ω ^), go play around!

Note: If this is your first time, I would first recommend looking at mod samples beforehand to get a jist of working with Reloaded, they can be found in Reloaded's main Mod Loader repository.

PS. Remember to change the output path.
Loader Mod Directory: `%AppData%/Roaming/Reloaded-Mod-Loader/Reloaded-Mods`

## Troubleshooting

### Cannot see local variables when debugging Reloaded Mods:
Although rare, this can happen when you are trying to debug `async` methods, try to debug those non-asynchronously or use the printing to console functionality provided to you in the template.

Alternatively try enabling `Native Code Debugging` under Project => Properties => Debug for your current project as well as `Use Managed Compatibility Mode` under Tools => Options => Debugging.

This applies regardless of whether the debugger has been manually attached at runtime by the user or via `Debugger.Launch()`.

### "Main.il" is in use.
See [DLLExport Issue #73](https://github.com/3F/DllExport/issues/73)
tl;dr: Compile just the mod or disable Visual Studio's parallel project builds.

## FAQ

### Can I use another programming language with Reloaded-Loader?
Absolutely. While there are no bindings for the loader's rich main development library or features of libReloaded. You should remember that at heart, Reloaded is a very heavily glorified DLL Injector.

This means that as long as your modification contains a Config.json configuration file and main32/main64.dll (depending on the architecture), Reloaded will execute your DLL.

You are only required to export 1 function named `main` which takes 1 pointer to a list of arguments (only Mod Loader Server port at time of writing).

Reloaded will wait for your DLL is expected to return. If you want to do work in the background, start a new thread.

### Address Space Layout Randomization (ASLR)?
Your absolute address of a global static variable is changing on every game/program launch?
Looking for a way around this? Fear not, easy peasy!

Use libReloaded's `MemoryReadWrite.GetBaseAddress()` or `Reloaded.Process.Native.Native.GetModuleHandle(null)`.

There's your base address back to add to a variable or function offset, enjoy your day :).

### Can I use Reloaded Mod Loader alongside [X] Mod Loader
If you want just the simple answer, see the user guide.

If you want the technical explanation, see [Hooks Interop.md](https://github.com/sewer56lol/Reloaded-Mod-Loader/blob/master/Documents/Hooks%20Interop.md)

tl;dr 99.9% of the cases everything will work perfectly with you having to do 0% work.

