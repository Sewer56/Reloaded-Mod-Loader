<div align="center">
	<h1>Writing Reloaded Code Mods: Quick Tour</h1>
	<img src="https://i.imgur.com/HsLAGlQ.png" align="center" />
	<br/> <br/>
</div>

## Page Information

🕒 Reading Time: 15+ Minutes

💯 Difficulty Level: 3/5.

## Introduction

The following is a small, quick, non-exhaustive resource to help some of you fellow programmers getting started with developing some simple modifications - providing an introduction to developing mods with Reloaded. This serves as a guide to help you get going, covering the basics and essentials.

This guide is intended to be read after [Reloaded for Programmers](https://github.com/sewer56lol/Reloaded-Mod-Loader/blob/master/Documents/Getting%20Started%20for%20Developers.md), it also assumes you have at least a small amount of experience with hacking games already. 

## Table of Contents
- [1. Reading/Writing to/from Memory](#1-readingwriting-tofrom-memory)
	- [Examples](#examples)
	- [Memory Read/Write Operations](#memory-readwrite-operations)
	- [Helper Functions](#helper-functions)
		- [ArrayPtr & FixedArrayPtr](#arrayptr--fixedarrayptr)
		- [Pointer](#pointer)
	- [Memory R/W: Remarks](#memory-rw-remarks)
- [2. Allocating/Freeing/Managing Native Memory](#2-allocatingfreeingmanaging-native-memory)
	- [Reusable Memory](#reusable-memory)
	- [Permanent/Long Term Storage](#permanentlong-term-storage)
- [3. Writing Assembly Code](#3-writing-assembly-code)
- [4. Native Functions](#4-native-functions)
	- [Defining Reloaded-Compatible Delegates](#defining-reloaded-compatible-delegates)
		- [Examples](#examples-1)
	- [Calling Functions](#calling-functions)
	- [Hooking Functions](#hooking-functions)
		- [Example](#example)
		- [Hooking Functions: Additional Remarks](#hooking-functions-additional-remarks)
	- [Function Pointers](#function-pointers)
- [5. Further Exploration](#5-further-exploration)
	- [Personal Recommendations](#personal-recommendations)
	- [Additional Remarks](#additional-remarks)

## 1. Reading/Writing to/from Memory

Reading or writing memory with Reloaded is extremely easy; Unlike some more traditional methods of reading and writing memory you may have seen, Reloaded's memory reading and writing implementations are extensive and fully support the use of generics/generic constructs.

Support of reading and writing memory is provided in the main libReloaded library and can be found exactly inside the `Reloaded.Process.Memory` namespace, `MemoryReadWrite` static class (add `using Reloaded.Process.Memory` if you are not seeing the overloads).

That said, the class mainly provides extension methods for another class `ReloadedProcess`, which is what you should use for reading/writing memory.

Read/Write operations are supported both by reference and by value in order to improve performance.

### Examples

Writing a primitive to memory:
```csharp
// GameProcess is an instance of ReloadedProcess
int oneThreeThreeSeven = 1337;
GameProcess.WriteMemory(addressOfAllocation, oneThreeThreeSeven);
```

Writing a struct to memory:
```csharp
// Let's create and write our own custom structure.
PlayerCoordinates playerCoordinates = new PlayerCoordinates()
{
    xPosition = 10,
    yPosition = 1368.62F,
    zPosition = -5324.677F
};
GameProcess.WriteMemory(addressOfAllocation, ref playerCoordinates);
```

*As you can see, WriteMemory() is actually a generic. The language can automatically infer the type of parameter*.

Writing a **class** to memory:
```csharp
[StructLayout(LayoutKind.Sequential)] // Absolutely necessary!!
class RandomClass
{
    int a;
    int b;
    int c;
};
GameProcess.WriteMemory(addressOfAllocation, ref playerCoordinates);
```
Yes, you can read classes in and out of memory too but you **must** explicitly define the structure layout of the class using the `StructLayout` attribute.

### Memory Read/Write Operations
*Describes the overloads available to use from `ReloadedProcess`*.
*Ranked in terms of speed, where the first is the fastest, last is the slowest.*

| Method              |                                Details                                | Example            |
|---------------------|:---------------------------------------------------------------------:|--------------------|
| Raw Pointer         | Only available from a mod DLL. Treat game variables as your own.      | C/C++              |
| Unsafe              | Does not marshal managed data. Inlined IL read from unmanaged memory. | ReadMemoryUnsafe   |
| Fast                | Doesn't set page permissions (see below*)                             | ReadMemoryFast     |
| Regular (No suffix) |                                                                       | ReadMemory         |
| External            | Use if `Bindings.ReloadedProcess` points to another process.          | ReadMemoryExternal |

The names of these overloads act like flags, and they can be combined, e.g. `ReadMemoryUnsafeFast` (fastest overload) will simply read memory inlined using pure IL without marshalling but `ReadMemoryUnsafe` will set the page permissions and then after read the memory.

*Methods 1-3 can fail if appropriate permissions not set for a page - e.g. sometimes if you are trying to modify the executable's assembly code.

### Helper Functions
Aside from simple management of memory; Reloaded provides various different utility function that may be used for reading and writing of unmanaged memory; here's highlight of some of these utilities in `Reloaded.Process.Helpers`.

#### ArrayPtr & FixedArrayPtr 
Found in `(Reloaded.Process.Helpers.Arrays)`.
Convenient abstraction that allows us for the use of arrays in memory as if they were our own without
using any pointers or performance penalties.

Use `FixedArrayPtr` if you know the length, else `ArrayPtr`.

```csharp
// arrayLocation is an arbitrary memory address.
var adventurePhysicsArray = new FixedArrayPtr<AdventurePhysics>(arrayLocation, 40);

// Get, modify and set element from array (from unmanaged memory).
AdventurePhysics sanicPhysics = adventurePhysicsArray[0];
sanicPhysics.HangTime = 1337;
adventurePhysicsArray[0] = sanicPhysics;

// Use LINQ (bet you didn't expect this one did you?)
var fastCharacters = adventurePhysicsArray.Where(x => x.HorizontalSpeedCap > 16);
```

#### Pointer
Err... uh, it's basically a managed abstraction for a simple pointer letting you easily
read/write to a specific memory address similar to `ArrayPtr`. Mainly intended for those
not experienced in the world of native oldschool unmanaged languages.

```csharp
var pointer = new Pointer<int>(0x123456);
pointer.Value = 1337;
pointer.Address = (void*)0x234567;
```

### Memory R/W: Remarks
**A.** By default, on all operations, libReloaded assumes that you are working on the current process (i.e. you are an injected DLL). If you wish to use Reloaded as a library to manage the memory of another application - you need to explicitly assign `Bindings.TargetProcess` of `libReloaded` with your own `ReloadedProcess`. This should be fairly easy as `ReloadedProcess` offers a wide range of constructor overloads.

**B.** Although rare with games, when working with certain modern executables - sometimes you may find that certain static memory addresses (green - Cheat Engine) change between application restarts. If that happens, chances are that you are dealing with ASLR (Address Space Layout Randomization).

If this term is one you are unaware with - you should research it. Nonetheless, Reloaded does have a means of obtaining the base address of an executable through the use of `ReloadedProcess.GetBaseAddress()`, however this is only available if your program is an injected DLL/a mod (i.e. within the same address space as the game).

## 2. Allocating/Freeing/Managing Native Memory
Should you ever need some of your own unmanaged arbitrary memory to work with things, one can easily allocate their own memory for their use with Reloaded. 

### Reusable Memory

Should you need reusable memory then the class `MemoryAllocator` inside `Reloaded.Process.Memory` provides you with a set of extension methods for `ReloadedProcess` to allocate and free memory.

**Example**:
```csharp
GameProcess.AllocateMemory(2048); // GameProcess is a ReloadedProcess instance
```

Otherwise and/or additionally, if you are writing a mod and are used to your `Marshal` methods, you'd be happy to know that you are perfectly safe to use your `Marshal.AllocHGlobal` and `Marshal.FreeHGlobal` for allocating/deallocating memory.

### Permanent/Long Term Storage

Should you be writing memory that is permanent and which you do not intend to ever free, Reloaded actually provides a special class for you called `MemoryBufferManager` (`Reloaded.Process.Buffers`) for memory efficient (RAM saving) storage of permanent variables.

The use of this class is fairly simple and does not warrant explaining; everything is managed by Reloaded behind the scenes. To use this class, simply call `MemoryBuffer.Add(T value)` to write an arbitrary generic value/structure to a Reloaded managed buffer in memory. You will receive back the address of where your data has been written to.

## 3. Writing Assembly Code

One of the cool features of Reloaded is that it allows you to assemble both X86 and X64 code on the fly at any point during runtime; the purpose of this is to work around the lack of an inline assembler you would normally see in C++.

To assemble some X86 or X64 code at runtime, simply use `Reloaded.Assembler` as such:

```csharp
// Sample assembler.
string[] x64asm = new[]
{
    "use64",                // Specifies this is a X64 piece of ASM.
    "mov rbx, rax",     
    "mov rax, 0x123456",
    "jmp qword [0x123456]"  // This is FASM, YOU MUST SPECIFY OPERAND SIZE EXPLICITLY (`qword` in this case)
};
byte[] result64 = Assembler.Assemble(x64asm);
```

Bear in mind, that because the solution for assembly is FASM under the hood - this cannot be *natively* used within an x64 process. If your mod is running inside an x64 application, libReloaded will automatically unpack its own remote assembler (Reloaded-Assembler) and communicate with it over the local system instead.

You are also advised to test whether your assembly compiles with FASM manually before adding it into your mod - Reloaded does not warn you if the assembler failed to compile and simply returns a nop (0x90). 

*Should you want to execute your own arbitrary assembly code, feel free to read onward onto the `Native Functions` section. That said, you'll probably never practically end up requiring to do that realistically.*

## 4. Native Functions

Calling, hooking and performing other operations with native functions in Reloaded mods is performed through the use of delegate declarations. Reloaded's main library is able to create individual delegate instances from supplied function addresses - allowing you to use game functions as if they were your own.

### Defining Reloaded-Compatible Delegates

Defining delegates to call game functions under Reloaded is performed just like defining regular delegates - with the exception of two key things.

- Foremost, you must inform Reloaded of the kind of function you are going to call with the use of Reloaded's own `ReloadedFunctionAttribute` (and in the case of X64 applications, `X64ReloadedFunctionAttribute`).

- Secondmost, you *must* also set the regular `UnmanagedFunctionAtrribute` with a calling convention of CDECL **regardless of the actual calling convention used**. Aside from that, you can use `UnmanagedFunctionAtrribute` to control marshalling and some other options as usual.

#### Examples:

**Native CDECL function:**
```csharp
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[ReloadedFunction(CallingConventions.Cdecl)]
public delegate void RwCameraSetViewWindow(RWCamera* RwCamera, RWView* view);
```

**Function with registers as parameters:**
```csharp
/// <summary>
/// Reads a ANM/HAnim stream from a .ONE archive. Returns address of a decompressed ANM file.
/// </summary>
/// <param name="fileIndex">[EAX] The index of the file inside the .ONE archive (starting with 2)</param>
/// <param name="addressToDecompressTo">[ECX] The address to which the file inside the ONE archive will be decompressed to.</param>
/// <param name="thisPointer">"This" pointer for the ONEFILE class instance.</param>
/// <returns>The address containing the read in ANM (H Anim - Character Animation) stream.</returns>
[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
[ReloadedFunction(new[] { Register.eax, Register.ecx }, Register.eax, StackCleanup.Callee)]
public delegate int OneFileLoadHAnimation(int fileIndex, void* addressToDecompressTo, ONEFILE* thisPointer);
```

*This was an example of a custom function with the following properties:*

- Two parameters (left to right) in registers `EAX` and `ECX`. 
- Return register of `EAX`. 
- "Callee" stack cleanup, i.e. the stack pointer is reset at the end of the function either via `ret X` or `add esp, X`. 

Those with some experience in reverse engineering might know that IDA would identify such function as `userpurge`.

For custom functions, when necessary, Reloaded creates CDECL => Custom convention wrappers with its own JIT Assembler - which is what will be called behind the scenes as you call your game functions.

### Calling Functions

In order to create an instance of your own custom delegate from a supplied function pointer, the factory classes `FunctionWrapper` (*Reloaded.Process.Functions.X86Functions*) and `X64FunctionWrapper` (*Reloaded.Process.Functions.X64Functions*) are used respectively. These will return you an instance of your requested function at address which you can call like a native function.

```csharp
// Based on the delegate above.
RwCameraSetViewWindow rwCameraSetViewWindow = FunctionWrapper.CreateWrapperFunction<RwCameraSetViewWindow>(0x0064AC80);

// You may now call the delegate instance/game fuction like if it was your own.
rwCameraSetViewWindow(RwCamera, view);
```

Regarding the other, more complex function seen above that has been optimized out by the compiler - nothing changes. The process is exactly the same and saves you having to write what would otherwise be custom inline assembly in the C++ world.

### Hooking Functions

Just like Reloaded makes the calling of any regular or custom arbitrary functions involving register parameters easy, the hooking of arbitrary functions in Reloaded can also be considered a simple walk in the park through the use of the `FunctionHook` factory class.

The usage is fairly simple and mostly self-explanatory, below is a simple example, stripped down and simplified from the Reloaded Mod Sample pack:

#### Example:

```csharp
/* Fields */

// Create the hook, then activate it.
// Note: createFileAPointer is the address of WinAPI function CreateFileA.
//       CreateFileAImpl is your C# function the game code execution will redirect to when it calls CreateFileA.
FunctionHook<CreateFileA> _createFileAHook = FunctionHook<CreateFileA>.Create((long)createFileAPointer, CreateFileAImpl).Activate();

/* Hook Function */

/// <summary>
/// Contains the implementation of the CreateFileA hook.
/// Simply prints the file name to the console and calls + returns the original function.
/// </summary>
private static IntPtr CreateFileAImpl(string filename, FileAccess access, FileShare share, IntPtr securityAttributes, FileMode creationDisposition, FileAttributes flagsAndAttributes, IntPtr templateFile)
{
    // If statement filters out non-files such as HID devices;
    if (!filename.StartsWith(@"\\?\"))
        Bindings.PrintInfo($"[CFA] Loading File {filename}");

    // Calls the original function that we hooked and returns its return address back.
    return _createFileAHook.OriginalFunction(filename, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile);
}
```

There is nothing extra you need to do such as writing your own custom inline assembler by hand for hooking functions like you may be used to doing in C++. The complicated stuff is already handled by Reloaded for you.

Reloaded's hooking system is very, very, very powerful under the hood while also at the same time being fairly complex due to that, with aspects such as its own "mini-JIT" built in for the generation of `CDECL -> Calling Convention` and `Calling Convention -> CDECL` wrappers - thankfully everything complex is abstracted from you, the end user.

Other notable fairly unique features of Reloaded's hooking mechanism include being able to automatically patch other, non-Reloaded hooks such as the Steam Overlay (including return addresses) and stack hooks ontop of one another successfully.

#### Hooking Functions: Additional Remarks

**A.** You may have noticed that nowhere in that text, or the  paragraph, the length of the hook was mentioned. Well, Reloaded is able to happily and easily calculate the hook length for you without requiring anything from you in return. That said, if you have any specific need to set the length manually, feel free to do so - just note that the minimum hook length is 6 bytes, not 5. 

**B.** Just like function calling in Reloaded supports marshalling, the same is likewise true about function hooks. You can happily use some of the standard C# marshals alongside your custom marshalling (if you have any) for your hooks. Here is an example.

```csharp
/*
    Within native code this individual function would be expressed as "int PlayADX(char* fileName)",
    here thanks to marshalling we are able to simply specify it as a string.
    
    In this specific case, additionally, CharSet = CharSet.Ansi must be specified as the game from
    which this function originates from did not use the Unicode encoding that is default in C#. 
*/

[ReloadedFunction(CallingConventions.Cdecl)]
[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)
public delegate int PlayADX(string fileName);
```

### Function Pointers

Should you ever find yourself needing to call functions that are pointed to by a pointer whose value constantly changes, Reloaded provides you with a simple utility class to help you alleviate the pain of constantly changing function addresses.

The utility class is simply named `FunctionPtr` and can be found at `Reloaded.Process.Helpers.Functions` respectively - the usage is simple. Here is a small sample example:

```csharp
// 0x123456 is the address of a pointer which points to a function of type MyCustomDelegate
// MyCustomDelegate is a delegate marked with ReloadedFunctionAttribute and UnmanagedFunctionPointerAttribute.
FunctionPtr<MyCustomDelegate> functionPtr = new FunctionPtr<MyCustomDelegate>(0x123456);

// Gets the address of our function.
var functionAddress = functionPtr.FunctionAddress;

// Gets the address of the pointer to our function.
var functionPointer = functionPtr.FunctionPointer;

// Gets the delegate to use for calling the native game function and calls the function. 
var myCustomFunction = functionPtr.GetDelegate();
myCustomFunction(1000);
```

Regarding the topic of performance - the class automatically caches function wrappers using a Dictionary under the hood as the pointer points to each new address. This means that for those custom functions whose pointers will alternate between a set number of values, most, if not at some point all of the time the class will already have a pre-prepared function wrapper ready to call, thus reducing overhead.

## 5. Further Exploration

Well, this was it, the basic aspects of writing Reloaded mods explained.

Of course this isn't the full story, Reloaded has much more to offer such as easy to use internal DirectX overlays, external overlays, its own custom controller input code - thread management, working with memory pages and much more.

Should you be in pursuit of other features, you are encouraged to look at the samples and take a few looks through the other libraries that Reloaded offers in its collection, both in the repository and NuGet.

For convenience, here's a quick breakdown of the more useful libraries:

| Library             |                       Things of Potential Interest                      |
|---------------------|:-----------------------------------------------------------------------:|
| libReloaded-IO      | Contains the code that drives Reloaded's config files & configurations. |
| libReloaded-Input   | Reloaded's own controller input library. Useful for input mods.         |
| libReloaded-Native  | Helpers for some things such as working with windows. Rarely used.      |
| libReloaded-Overlay | Very easy to use internal and external DirectX, D2D, WPF overlays.      |

### Personal Recommendations

**A.** Look at some of the basic samples available for the mod loader (these are also prebuilt with every loader copy). You can find those under the `Reloaded-Mod-Samples` directory in Visual Studio's solution explorer once you clone the repository, submodules and open the solution in VS.

**B.** Feel free to clone the mod loader repository and add the individual components of libReloaded that you need as a project dependency instead of using the NuGet packages. Being able to see the source code of libReloaded will provide you with the discrete advantage of becoming much more familliar with the library.

**C.** Included because some people did this: Spare yourself the manual copying and simply change the build directory of the mod template under project properties to your mod directory in AppData - you'll save yourself a lot of hassle.

### Additional Remarks

- Something exclusive to Reloaded is that under normal use, your modifications run before the game even starts executing. This actually means that you can patch even the entry point/very beginning of the process.

- Almost all of Reloaded's non-GUI/Mod Manager code is presently spread across the various libReloaded libraries for easy reusability. This leaves options for you - if you want to, load other Reloaded mods inside your Reloaded mod via DLL injection.
