# Reloaded-Assembler

Reloaded Assembler is a part of libReloaded, the main Reloaded-Mod-Loader development library.

This X86 compiled program runs a local loopback TCP server based on C# native websockets, allowing external programs and utilities to connect to it through WebSocket based Interprocess Communication.

## Benefits

This allows you to make use of the FASM.NET wrapper outside of 32-bit runtime/development environments, by simply connecting to an external already running 32-bit process - allowing libReloaded based mod loader mods to also make use of the live-assembler inside 64-bit games.

## Contents

This repository contains the relevant server code for the thin networked wrapper as well as the relevant copies of libReloaded networking/assembler class code.

The code copies are provided in the case you wish to use this simple networked implementation standalone outside of libReloaded (no dependency on libReloaded).

### ReloadedAssemblerService
Program which runs in the background without a GUI and provides X86/X64 assembly to connected WebSocket clients on demand.

### ReloadedAssemblerTest
A dummy program to test integration into an external library, or for you to copy code from to use the Reloaded Assembler without a dependency on libReloaded.