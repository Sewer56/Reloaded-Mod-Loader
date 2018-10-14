using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Reloaded;
using Reloaded.Assembler;
using Reloaded.Memory;
using Reloaded.Memory.Pointers;
using Reloaded.Memory.Sources;
using Reloaded.Process;
using Reloaded.Process.Buffers;
using Reloaded.Process.Memory;
using Reloaded_Mod_Template.Adventure;
using Reloaded_Mod_Template.Structs;

namespace Reloaded_Mod_Template
{
    public static class Program
    {
        #region Reloaded Mod Template Stuff
        /// <summary>
        /// Holds the game process for us to manipulate.
        /// Allows you to read/write memory, perform pattern scans, etc.
        /// See libReloaded/GameProcess (folder)
        /// </summary>
        public static ReloadedProcess GameProcess;

        /// <summary>
        /// Stores the absolute executable location of the currently executing game or process.
        /// </summary>
        public static string ExecutingGameLocation;

        /// <summary>
        /// Specifies the full directory location that the current mod 
        /// is contained in.
        /// </summary>
        public static string ModDirectory;
        #endregion Reloaded Mod Template Stuff

        /// <summary>
        /// Your own user code starts here.
        /// If this is your first time, do consider reading the notice above.
        /// It contains some very useful information.
        /// </summary>
        public static void Init()
        {
            /*
                Reloaded Mod Loader Sample: Memory Manipulation
                Architectures supported: X86, X64

                An example of memory manipulation in Reloaded, showing how easily it is
                possible to write or read structures to and from memory.

                This is a mini-tutorial for Reloaded.Memory package.
            */

            // This line will trigger a debugger dialog once the mod is executed.
            #if DEBUG
            Debugger.Launch(); 
            #endif

            // Reloaded 3.X+ exposes the <Memory> class that can be used to write to the memory of the current class.
            // You can find it in Reloaded.Memory.Sources;

            // Recommended access pattern:
            var memory = Memory.Current;                // Static/Preinitialized access to current process' memory.

            // Tutorial 0: Allocate/Free Memory
            var memoryLocation = memory.Allocate(65535); // Did you think it would be harder? Here's 65535 bytes at memoryLocation.
                                                         // And you would free it with memory.Free(memoryLocation);

            // Tutorial 1: Basic Reading/Writing Primitives
            PrimitivesExample(memory, memoryLocation);

            // Tutorial 2: Writing Structs
            WriteStructsExample(memory, memoryLocation);

            // Tutorial 3: Memory Sources [Other Processes etc.]
            MemorySourceExample(memory, memoryLocation);

            // Tutorial 4: Struct Arrays
            StructArrayExample(memory, memoryLocation);

            // Tutorial 5: Marshalling
            MarshallingExample(memory, memoryLocation);

            // Tutorial 6: Struct & StructArray Utility Classes
            StructUtilityExample(memory, memoryLocation);

            // Tutorial 7: Free Memory
            memory.Free(memoryLocation);
        }

        /// <summary>
        /// A simple method example which demonstrates writing a simple primitives to memory.
        /// </summary>
        /// <param name="memory">This object is used to perform memory read/write/free/allocate operations.</param>
        /// <param name="memoryLocation">Arbitrary location in memory where this tutorial will be held.</param>
        private static void PrimitivesExample(Memory memory, IntPtr memoryLocation)
        {
            // You can use the Memory Option to write any arbitrary generic primitive to memory.
            // Here is an example:
            memory.Write(memoryLocation, 1337);         // Implicitly memory.Write<int>(memoryLocation, 1337);

            // No <> brackets?
            // C# has this feature called "Type Inference"; it guesses the generic type from object supplied.
            // You typed in a number; it automatically assumed, as with each number, it was an int.

            // Nothing changes with a variable.
            int leet = 1337;
            memory.Write(memoryLocation, leet);

            // But what if you instead wanted to write a short?
            memory.Write<short>(memoryLocation, 1337);  // Implicit cast happens here.
            memory.Write(memoryLocation, (short)1337);  // Explicit cast; will also write a short.
            memory.Write(memoryLocation, (short)leet);  // Explicit cast with variable.

            // This is possible with any generic type that can be represented in the unmanaged C/C++/D/other language world.
            // Floats, Doubles, no problem.

            // And of course; reading is just as obvious.
            // No type inference on reading unfortunately; must explicitly set type inside <>.
            int anotherLeet = memory.Read<short>(memoryLocation);

            if (leet == anotherLeet)
                Bindings.PrintText("[Memory Demo] Writing Primitives: Success");
        }

        /// <summary>
        /// A simple method example which demonstrates writing structs.
        /// </summary>
        /// <param name="memory">This object is used to perform memory read/write/free/allocate operations.</param>
        /// <param name="memoryLocation">Arbitrary location in memory where this tutorial will be held.</param>
        private static void WriteStructsExample(Memory memory, IntPtr memoryLocation)
        {
            // Writing structs is no different to writing primitives; at all.
            Vector3 xyzPosition = new Vector3(1F, 2F, 3F);
            memory.Write(memoryLocation, xyzPosition);

            // Now to confirm our struct writing; let's read it back and check.
            var xyzPositionCopy = memory.Read<Vector3>(memoryLocation);

            if (xyzPosition == xyzPositionCopy)
                Bindings.PrintText("[Memory Demo] Writing Struct Primitives: Success");
        }

        /// <summary>
        /// A simple method example which demonstrates the <see cref="IMemory"/> interface.
        /// </summary>
        /// <param name="memory">This object is used to perform memory read/write/free/allocate operations.</param>
        /// <param name="memoryLocation">Arbitrary location in memory where this tutorial will be held.</param>
        private static void MemorySourceExample(IMemory memory, IntPtr memoryLocation)
        {
            // Do you see the subtle difference in the parameters of this function?

            // Earlier in the program; you would have seen that we have been writing generics to the program's memory
            // using this 'Memory' class. Well... the truth was that you were actually interacting with an interface called IMemory.

            // If you look around; you will see that you still have the same identical parameters available for this parameter.
            // IMemory is an interface in Reloaded 3.X+ that provides memory read/write from an arbitrary memory source.
            // The implementation of `Memory` is just one of them; that lets you read/write inside the current process.

            // But what if you want to read from a DIFFERENT process?
            // Well; let's look at another IMemory implementation.

            IMemory anotherProcessMemory = new ExternalMemory(Process.GetCurrentProcess());

            // ExternalMemory is yet another implementation of IMemory; allowing you to read from another process.
            // In this case we have pointed it at the current process - now let's show this working.

            // Write "1337" to memory address in external process.
            int leet = 1337;
            anotherProcessMemory.Write(memoryLocation, leet);

            // Read "1337" written by "another process" using our IMemory implementation (Memory) that reads from current process.
            int anotherLeet = memory.Read<int>(memoryLocation);

            // And they are equal.
            if (leet == anotherLeet)
                Bindings.PrintText("[Memory Demo] Other Sources: Success");

            // Implementing the IMemory interface is quite easy; especially with the many tools in the <Struct> class (more on that later).
            // You may think for now that the interface isn't much useful; but the power of it will become very clear.

            // Extra note: Overloads for IMemory are implemented as Extension Methods
            // Make sure to add `using Reloaded.Memory.Sources;` in your own projects.
        }

        /// <summary>
        /// A simple method example which demonstrates struct Array operations.
        /// </summary>
        /// <param name="memory">This object is used to perform memory read/write/free/allocate operations.</param>
        /// <param name="memoryLocation">Arbitrary location in memory where this tutorial will be held.</param>
        private static void StructArrayExample(IMemory memory, IntPtr memoryLocation)
        {
            // Let's load a binary file from the disk and write it to memory.
            byte[] physicsData = File.ReadAllBytes($"{ModDirectory}\\Structs\\phys.bin");
            memory.WriteRaw(memoryLocation, physicsData);

            // Number of items in structure (known).
            const int itemCount = 40;

            // Array Read from Memory
            AdventurePhysics[] sonicAdventurePhysicsData = memory.Read<AdventurePhysics>(memoryLocation, itemCount);

            // Array Write to Memory
            memory.Write(memoryLocation, sonicAdventurePhysicsData);

            // Pointer to array in memory. Provides enhanced functionality over a standard.
            var adventurePhysics = new ArrayPtr<AdventurePhysics>((ulong)memoryLocation);
            float speedCap = adventurePhysics[0].HorizontalSpeedCap;    // And of course read/writes work like with regular pointers.

            // Pointer to array in memory with known length. Provides even extra functionality.
            var adventurePhysicsFixed = new FixedArrayPtr<AdventurePhysics>((ulong)memoryLocation, itemCount);
            float averageAirAcceleration = adventurePhysicsFixed.Average(physics => physics.AirAcceleration);   // LINQ over Arbitrary Memory: Of course...

            // Did I mention that all of these support `IMemory` fully? What does this mean?
            // Let's have a small "experiment" with "another process" (made from current process).

            IMemory anotherProcessMemory = new ExternalMemory(Process.GetCurrentProcess());
            var physicsFixedOtherProcess = new FixedArrayPtr<AdventurePhysics>((ulong)memoryLocation, itemCount, anotherProcessMemory);
            float averageAirAcceleration2 = physicsFixedOtherProcess.Average(physics => physics.AirAcceleration);

            // What you just witnessed was... LINQ over arbitrary structs inside memory of another process.
            // This is really the true power of IMemory

            // Foreach loop over structs in other processes? Of course.
            float greatestInitialJump = float.MinValue;
            float smallestInitialJump = float.MaxValue;
            foreach (var physics in physicsFixedOtherProcess)
            {
                if (physics.InitialJumpSpeed > greatestInitialJump)
                    greatestInitialJump = physics.InitialJumpSpeed;

                if (physics.InitialJumpSpeed < smallestInitialJump)
                    smallestInitialJump = physics.InitialJumpSpeed;
            }

            Bindings.PrintText($"[Memory Demo] LINQ Over Arbitrary Memory: {averageAirAcceleration} (Average air Acceleration in Sonic Adventure-Heroes)");
            Bindings.PrintText($"[Memory Demo] LINQ Over Memory in Another Process: {greatestInitialJump - smallestInitialJump} (Sonic Adventure-Heroes Physics delta between jump speeds)");
        }

        /// <summary>
        /// A simple method example which demonstrates marshalling at work.
        /// </summary>
        /// <param name="memory">This object is used to perform memory read/write/free/allocate operations.</param>
        /// <param name="memoryLocation">Arbitrary location in memory where this tutorial will be held.</param>
        private static void MarshallingExample(IMemory memory, IntPtr memoryLocation)
        {
            // Marshalling is yet another feature that is supported when reading and writing from ANY IMemory source.
            // Consequently; this also means that classes based on IMemory - such as ArrayPtr or FixedArrayPtr support it under the hood.
            // This example will read simple binary struct with an inline fixed length array of strings and.

            // Let's load a binary file from the disk and write it to memory.
            byte[] characterData = File.ReadAllBytes($"{ModDirectory}\\Structs\\CustomFileHeader.bin");
            memory.WriteRaw(memoryLocation, characterData);

            // Now let's parse it back. (The marshalling rules are defined in CustomFileHeader class). 
            var customHeader = memory.Read<CustomFileHeader>(memoryLocation, true); // Setting to "true" enables the marshaller.

            Bindings.PrintText($"[Memory Demo] Marshal Test (Struct fixed length char array as string): " +
                               $"Name = {customHeader.Name} Offset = {customHeader.Offset} Length = {customHeader.Length}");
        }

        /// <summary>
        /// Shows some functionality of the <see cref="Struct"/> and <see cref="StructArray"/> utility classes.
        /// </summary>
        /// <param name="memory">This object is used to perform memory read/write/free/allocate operations.</param>
        /// <param name="memoryLocation">Arbitrary location in memory where this tutorial will be held.</param>
        private static void StructUtilityExample(IMemory memory, IntPtr memoryLocation)
        {
            // Under the hood; the IMemory implementations may use a certain struct utility classes known as Struct
            // and StructArray which provide various methods for struct conversions and general work with structs.

            // Like earlier; let's load the adventure binary file.
            byte[] physicsData = File.ReadAllBytes($"{ModDirectory}\\Structs\\phys.bin");

            // But this time; do a direct conversion rather than reading from memory.
            // Note that you don't even need to specify item count this time around.
            AdventurePhysics[] adventurePhysics = StructArray.FromArray<AdventurePhysics>(physicsData);
            
            // Calculate total array size (in bytes).
            int arraySize = StructArray.GetSize<AdventurePhysics>(adventurePhysics.Length);

            // Get raw bytes for the struct.
            byte[] physicsDataBack = StructArray.GetBytes(adventurePhysics);

            // You can also read/write structures; as a shorthand to Memory class.
            StructArray.ToPtr(memoryLocation, adventurePhysics);
            AdventurePhysics[] adventurePhysicsCopy = StructArray.FromPtr<AdventurePhysics>(memoryLocation);

            // Beware of the double sided blade however.
            // A. Struct class allows you to change the source read/write source for FromPtr and ToPtr.
            // B. It affects both Struct and StructArray.

            // Note: There are also explicit overloads for FromPtr and ToPtr that let you use a source without modifying current source.
            Struct.Source = memory; // And of course the source is an implementation of IMemory.

            // Print details.
            if (physicsDataBack.SequenceEqual(physicsDataBack))
                Bindings.PrintText($"[Memory Demo] Success: Original Physics Data and StructArray.GetBytes() are Equal");

            Bindings.PrintText($"[Memory Demo] Struct Array Size: {arraySize}");

        }
    }
}
