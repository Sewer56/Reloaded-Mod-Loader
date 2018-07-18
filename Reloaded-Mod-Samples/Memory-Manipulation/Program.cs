using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Reloaded;
using Reloaded.Assembler;
using Reloaded.Process;
using Reloaded.Process.Buffers;
using Reloaded.Process.Helpers.Arrays;
using Reloaded.Process.Memory;

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
            */

            // Want to see this in with a debugger? Uncomment this line.
            // Debugger.Launch();

            /// //////////////////////////////////
            /// Example #1: Reading/Writing Memory
            /// //////////////////////////////////

            // First let's allocate some memory to write our data to.
            // See footnote at 1*, you should really instead use the MemoryBufferManager class for this.
            IntPtr addressOfAllocation = GameProcess.AllocateMemory(2048);
            Bindings.PrintInfo($"Memory allocated at {addressOfAllocation.ToString("X")}");

            // Now let's write some memory to the given address.
            int oneThreeThreeSeven = 1337;
            GameProcess.WriteMemory(addressOfAllocation, ref oneThreeThreeSeven);
            Bindings.PrintInfo($"Written {1337} to address {addressOfAllocation.ToString("X")}");

            // Wait what!? That simple??
            // Well, of course, under the hood we use some generics and fancy C# to automatically convert a type
            // into an array of bytes to write into memory. Feel free to verify the result with Cheat Engine.

            // Let's read this address back.
            int valueAtAddress = GameProcess.ReadMemory<int>(addressOfAllocation);
            Bindings.PrintInfo($"Read {valueAtAddress} from address {addressOfAllocation.ToString("X")}");

            /// //////////////////////////////////////
            /// Example #2: Reading/Writing Structures
            /// //////////////////////////////////////

            // Let's try something a bit more complex now.
            addressOfAllocation += sizeof(int);     // Keep our previously written integer for later so you can verify it's there.
            Bindings.PrintInfo($"Demo #2, Read/Write address now at {addressOfAllocation.ToString("X")}");

            // Let's create and write our own custom structure.
            PlayerCoordinates playerCoordinates = new PlayerCoordinates()
            {
                xPosition = 10,
                yPosition = 1368.62F,
                zPosition = -5324.677F
            };

            // Now let's write it to memory.
            GameProcess.WriteMemory(addressOfAllocation, ref playerCoordinates);
            Bindings.PrintInfo($"Written arbitrary player coordinates {playerCoordinates.xPosition} {playerCoordinates.yPosition} {playerCoordinates.zPosition} to address {addressOfAllocation.ToString("X")}");

            // Wait... Nothing changed?
            // Of course I did say something about generics and fancy C# didn't I?
            // Let's read it back.
            PlayerCoordinates newPlayerCoordinates = GameProcess.ReadMemory<PlayerCoordinates>(addressOfAllocation);
            Bindings.PrintInfo($"Read player coordinates back from {addressOfAllocation.ToString("X")}");

            // Check if we are equal.
            if (newPlayerCoordinates.xPosition == playerCoordinates.xPosition &&
                newPlayerCoordinates.yPosition == playerCoordinates.yPosition &&
                newPlayerCoordinates.zPosition == playerCoordinates.zPosition)
            {
                Bindings.PrintInfo($"Success: See? It's incredibly easy!");
            }
            else { Bindings.PrintInfo($"Failure: Read back player coordinates are not equal."); }

            /// /////////////////////////////////////////
            /// Example #3: Reading/Writing Struct Arrays
            /// /////////////////////////////////////////

            // But what about arrays? 
            // Well, libReloaded has an utility for even that.
            // First let's read some arbitrary array from a file and write it to memory.
            Bindings.PrintInfo($"Demo #3, Array Read/Write Test Begin! (This one you should see in a debugger)");
            
            // First write our arbitrary physics data into memory.
            byte[] physicsData = File.ReadAllBytes($"{ModDirectory}\\phys.bin"); // ModDirectory is from Reloaded Template 
            IntPtr arrayLocation = MemoryBufferManager.Add(physicsData);         // A good example of using MemoryBufferManager (see note below), 
                                                                                 // normally no guarantee our data will fit into our allocated memory, 
                                                                                 // MemoryBufferManager also handles that case without extra code.
            Bindings.PrintInfo($"Character physics data written to {arrayLocation.ToString("X")}");

            // Now let's read back the memory.
            // Length of array for this sample is known as 40.
            FixedArrayPtr<AdventurePhysics> adventurePhysicsArray = new FixedArrayPtr<AdventurePhysics>(arrayLocation, 40);

            // Let's try to read one entry back from memory.
            AdventurePhysics firstEntry = adventurePhysicsArray[0];                     // Wow! It's almost like a native array... isn't it?

            // But we want to know everything! Give me it all!
            AdventurePhysics[] adventurePhysicses = adventurePhysicsArray.ToArray();    // Huh? That's it. Yup.

            // How about writing to one of the entries?
            firstEntry.HangTime = 1337;
            adventurePhysicsArray[0] = firstEntry;                                      // You've gotta be kidding me...
            Bindings.PrintInfo($"Physics at {arrayLocation.ToString("X")} changed.");
            Bindings.PrintInfo($"Demo end.");
        }

        /// <summary>
        /// Defines a simple structure defining player coordinates.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct PlayerCoordinates
        {
            public float xPosition;
            public float yPosition;
            public float zPosition;
        }

        /*
            1.

            Now we just gave some extra memory for the process to use that's all for us to write to,
            this memory may be used to write our own assembly functions to redirect the program to or
            some other arbitrary user preferred reasons.

            It is recommended HOWEVER that you use MemoryBufferManager class in the Assembler namespace if writing some
            small size information into unallocated memory is all you want.

            The minimum granularity (smallest step amount) of allocation is the size of a page (4096bytes),
            repeatedly allocating 4096bytes of memory like this for small size 4-8 byte commonly used writes 
            is unnecessarily wasteful on memory.

            To put it in context, although I specified 2048 bytes of memory, 4096 bytes will be allocated anyway.
        */
    }
}
