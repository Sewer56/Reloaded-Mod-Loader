using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

/// <summary>
/// This namespace stores all code manipulation, code redirection, jumping among other methods responsible for manipulating the direct flow of program execution.
/// </summary>
namespace SonicHeroes.Hooking
{
    /// <summary>
    /// This class allows you to create a hook for the game, allowing you to replace an ingame piece of code with a call, or quite simply a statement to go off execute your own function/code. A hook requires at least 5 bytes and some code which you would be happy to scrap in order to instead run your hook. You must set a length of 5 + any stray/leftover instruction bytes AND make sure you are not in a register dependent assembly code as this solution does not backup registers.
    /// </summary>
    class Hook : IDisposable
    {
        /// <summary>
        /// An optional user defined number of bytes to replace while performing a hook, you may use this to ensure that no stray bytes are left from another instruction.
        /// </summary>
        int CustomNumberOfBytes = 0;

        /// <summary>
        /// Present for better code readibility, this is the length of the jump instruction itself.
        /// </summary>
        const int JumpInstructionLength = 5;

        /// <summary>
        /// This is the address which we will be hooking, the address where a call jmp is placed to redirect our program flow to our own function.
        /// </summary>
        IntPtr HookAddress;
        /// <summary>
        /// This will store the old original memory protection which we will restore along with the original bytes should we wish to fully dispose of the hook.
        /// </summary>
        Protection OriginalMemoryProtection;
        /// <summary>
        /// The original source array of bytes which we will be hooking/placing a call jump to our own code from.
        /// </summary>
        byte[] OriginalBytes;
        /// <summary>
        /// The new bytes which we will place to make a call jump to our own code.
        /// </summary>
        byte[] NewBytes;

        /// <summary>
        /// This will effectively call the other FunctionHook, but with an IntPtr to the destination of the method we will want to use, as this one will retrieve an address to the destination method using the delegate signature. 
        /// </summary>
        /// <param name="SourceAddressPointer">The address at which we will start our hook process.</param>
        /// <param name="DestinationAddressPointer">Delegate to the method we will want to run. (DelegateName)Method</param>
        /// <param name="HookLength">The amount of bytes the hook lasts, all stray bytes will be replaced with NOP/No Operation.</param>
        public Hook
            (IntPtr SourceAddressPointer, Delegate DestinationAddressPointer, int HookLength)
            : this(SourceAddressPointer, Marshal.GetFunctionPointerForDelegate(DestinationAddressPointer), HookLength) { }

        // Creates a hook.
        // Hook length defines the amount of bytes which will be replaced by the hook.
        // Hook length must be at least 5 bytes.
        // When creating a hook, you must ensure to not leave any stray bytes open in the wild.
        public Hook(IntPtr SourceAddressPointer, IntPtr DestinationAddressPointer, int HookLength)
        {
            // Generate the variables :)
            CustomNumberOfBytes = HookLength;
            OriginalBytes = new byte[CustomNumberOfBytes];

            // Remove protection from the old set of bytes such that we may remove the 
            VirtualProtect(SourceAddressPointer, (uint)CustomNumberOfBytes, Protection.PAGE_EXECUTE_READWRITE, out OriginalMemoryProtection);
            // Copy original bytes from the source address to the byte array.
            Marshal.Copy(SourceAddressPointer, OriginalBytes, 0, CustomNumberOfBytes);
            // This is the amount of bytes/length as an array which we will jump/insert into the jump instruction.
            byte[] JumpLength = BitConverter.GetBytes((int)DestinationAddressPointer - (int)SourceAddressPointer - JumpInstructionLength);
            
            // Write the new bytes to make a call to our own code.
            NewBytes = new byte[CustomNumberOfBytes];
            NewBytes[0] = 0xE8; // Call JMP (ASM).
            NewBytes[1] = JumpLength[0];
            NewBytes[2] = JumpLength[1];
            NewBytes[3] = JumpLength[2];
            NewBytes[4] = JumpLength[3];
            // Write NOP for the remainder of the NewBytes array.
            for (int x = 5; x < CustomNumberOfBytes; x++) { NewBytes[x] = 0x90; }

            // Set the address which we will start hooking in the future.
            HookAddress = SourceAddressPointer;
        }

        // This will activate the hook
        /// <summary>
        /// Activates the hook via the use of manual hooking. Doing this is technically faster on performance by a negligible amount, it is an option, but please for the sake of interoperability with other mods, consider 'Subscribing' this address to the Mod Loader [Currently Not Implemented].
        /// </summary>
        public void Activate() { Marshal.Copy(NewBytes, 0, HookAddress, CustomNumberOfBytes); }
        /// <summary>
        /// Deactivates the hook via the use of manual hooking. Doing this is technically faster on performance by a negligible amount, it is an option, but please for the sake of interoperability with other mods, consider 'Subscribing & Unsubscribing' this address to the Mod Loader [Currently Not Implemented].
        /// </summary>
        public void Deactivate() { Marshal.Copy(OriginalBytes, 0, HookAddress, CustomNumberOfBytes); }

        /// <summary>
        /// When we no longer need the hook, we can get rid of it and all of its traces entirely.
        /// </summary>
        public void Dispose()
        {
            Deactivate();
            Protection Dummy;
            VirtualProtect(HookAddress, (uint)CustomNumberOfBytes, OriginalMemoryProtection, out Dummy);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize,
                Protection flNewProtect, out Protection lpflOldProtect);

        public enum Protection
        {
            PAGE_NOACCESS = 0x01,
            PAGE_READONLY = 0x02,
            PAGE_READWRITE = 0x04,
            PAGE_WRITECOPY = 0x08,
            PAGE_EXECUTE = 0x10,
            PAGE_EXECUTE_READ = 0x20,
            PAGE_EXECUTE_READWRITE = 0x40,
            PAGE_EXECUTE_WRITECOPY = 0x80,
            PAGE_GUARD = 0x100,
            PAGE_NOCACHE = 0x200,
            PAGE_WRITECOMBINE = 0x400
        }
    }

    /// <summary>
    /// This class is an alternative for the Hook class, this generates a call to your code, however with the use of a clever jump following, still executes the original code after your code has finished executing.
    /// </summary>
    class Injection : IDisposable
    {
        /// <summary>
        /// An optional user defined number of bytes to replace while performing a hook, you may use this to ensure that no stray bytes are left from another instruction.
        /// </summary>
        int CustomNumberOfBytes = 0;

        /// <summary>
        /// Present for better code readibility, this is the length of the jump instruction itself.
        /// </summary>
        const int JumpInstructionLength = 5;

        /// <summary>
        /// The amount of registers we will back up before calling own code, we will restore them right after the call!
        /// </summary>
        const int RegistersToBackup = 8;

        /// <summary>
        /// This is the address which we will be hooking, the address where a call jmp is placed to redirect our program flow to our own function.
        /// </summary>
        IntPtr HookAddress;
        /// <summary>
        /// This will store the old original memory protection which we will restore along with the original bytes should we wish to fully dispose of the hook.
        /// </summary>
        Protection OriginalMemoryProtection;
        /// <summary>
        /// The original source array of bytes which we will be hooking/placing a call jump to our own code from.
        /// </summary>
        byte[] OriginalBytes;
        /// <summary>
        /// The new bytes which we will place to make a call jump to our own code.
        /// </summary>
        byte[] NewBytes;
        /// <summary>
        /// This byte array will store the PUSH instructions which will be used to place the registers on the stack before our own method execution.
        /// </summary>
        byte[] ASM_PushRegistersBytes = new byte[RegistersToBackup] 
        {
            0x50, // PUSH EAX
            0x51, // PUSH ECX
            0x52, // PUSH EDX
            0x53, // PUSH EBX
            0x54, // PUSH ESP
            0x55, // PUSH EBP
            0x56, // PUSH ESI
            0x57 // PUSH EDI
        };
        /// <summary>
        /// This byte array will store the POP instructions which will be used to restore the register state once the call to our code has been performed.
        /// </summary>
        byte[] ASM_PopRegistersBytes = new byte[RegistersToBackup]
        {
            // These are in reverse order due to last in first out stack order, like a set of stacked books.
            0x5F, // POP EDI
            0x5E, // POP ESI
            0x5D, // POP EBP
            0x5C, // POP ESP
            0x5B, // POP EBX
            0x5A, // POP EDX
            0x59, // POP ECX
            0x58 // POP EAX
        };
        /// <summary>
        /// This will point to where the backing up of the registers will occur, the method call for the dll and the restoration of the registers, running of the original code and jumping back will occur.
        /// </summary>
        IntPtr NewInstructionAddress;

        /// <summary>
        /// These are the bytes which will be stored at the new instruction address which correspond to assembly instructions. Here in this memory region, ASM to backup the registers will be written, a call to our own method, will be performed, registers will be restored and a jump will be made back.
        /// </summary>
        byte[] NewInstructionBytes;

        /// <summary>
        /// This class is an alternative for the Hook class, this generates a call to your code, however with the use of a clever jump following, still executes the original code after your code has finished executing.
        /// </summary>
        /// <param name="SourceAddressPointer">The address at which we will start our hook process.</param>
        /// <param name="DestinationAddressPointer">Delegate to the method we will want to run. (DelegateName)Method</param>
        /// <param name="HookLength">The amount of bytes the hook lasts, all stray bytes will be replaced with NOP/No Operation.</param>
        public Injection
            (IntPtr SourceAddressPointer, Delegate DestinationAddressPointer, int HookLength)
            : this(SourceAddressPointer, Marshal.GetFunctionPointerForDelegate(DestinationAddressPointer), HookLength) { }

        // Hook Length Must be 5 bytes + any stray bytes!
        public Injection(IntPtr SourceAddressPointer, IntPtr DestinationAddressPointer, int HookLength)
        {
            ///
            /// The Setup
            /// Getting Ready for Hooking!
            CustomNumberOfBytes = HookLength; // Set hook length
            OriginalBytes = new byte[CustomNumberOfBytes]; // Initialize storage of original bytes.
            NewBytes = new byte[CustomNumberOfBytes]; // Initialize storage of new bytes.
            HookAddress = SourceAddressPointer;       // Set the address which we will start hooking in the future.

            ///
            /// Backup Original Bytes
            /// | Remove protection from the old set of bytes such that we may alter the bytes & copy original bytes from the source address to the byte array.

            VirtualProtect(SourceAddressPointer, (uint)CustomNumberOfBytes, Protection.PAGE_EXECUTE_READWRITE, out OriginalMemoryProtection);
            Marshal.Copy(SourceAddressPointer, OriginalBytes, 0, CustomNumberOfBytes);

            ///
            /// Allocate New Memory
            /// Here in this memory region, ASM to backup the registers will be written, a call to our own method, will be performed, registers will be restored and a jump will be made back.

            // Total Bytes: Register Backup (6) + JMP CALL To Own Method (5) + Register Restore (6) + Custom Bytes (Old Opcodes) + JMP Back (5);
            NewInstructionAddress = AllocateMemory(Process.GetCurrentProcess(), RegistersToBackup + JumpInstructionLength + CustomNumberOfBytes + RegistersToBackup + JumpInstructionLength); // Allocate memory to write old bytes in Sonic Heroes.
            byte[] NewInstructionJumpLength = BitConverter.GetBytes((int)NewInstructionAddress - (int)SourceAddressPointer - JumpInstructionLength); // The jump length to get our code execution to move to 
            NewBytes = new byte[CustomNumberOfBytes]; // Allocate byte array of required length.
            NewInstructionBytes = new byte[RegistersToBackup + JumpInstructionLength + CustomNumberOfBytes + RegistersToBackup + JumpInstructionLength]; // Allocate byte array of required length.

            // Write the new bytes to jump to this new address where everything will happen.
            NewBytes[0] = 0xE9; // JMP (ASM).
            NewBytes[1] = NewInstructionJumpLength[0];
            NewBytes[2] = NewInstructionJumpLength[1];
            NewBytes[3] = NewInstructionJumpLength[2];
            NewBytes[4] = NewInstructionJumpLength[3];
            for (int x = 5; x < CustomNumberOfBytes; x++) { NewBytes[x] = 0x90; } // If necessary, replace any stray bytes with NOP.

            ///
            /// The Payload
            /// This is the bytes that we will write at the new Instruction Address to perform various actions.

            /// Insert the backup of registers into the payload.
            Array.Copy(ASM_PushRegistersBytes, 0, NewInstructionBytes, 0, ASM_PushRegistersBytes.Length);

            /// Insert the Jump Call to our own code into the payload.
            byte[] JumpCall = Get_OwnCodeJumpBytes(DestinationAddressPointer);
            Array.Copy(JumpCall, 0, NewInstructionBytes, ASM_PushRegistersBytes.Length, JumpCall.Length);

            /// Insert the restore of registers into the payload.
            Array.Copy(ASM_PopRegistersBytes, 0, NewInstructionBytes, ASM_PushRegistersBytes.Length + JumpCall.Length, ASM_PushRegistersBytes.Length);

            /// Insert the original bytes to be executed!
            Array.Copy(OriginalBytes, 0, NewInstructionBytes, ASM_PushRegistersBytes.Length + JumpCall.Length + ASM_PushRegistersBytes.Length, OriginalBytes.Length);

            /// Get and write the jump back address to new array.
            byte[] Jump = Get_JumpBackBytes(SourceAddressPointer);
            Array.Copy(Jump, 0, NewInstructionBytes, ASM_PushRegistersBytes.Length + JumpCall.Length + ASM_PopRegistersBytes.Length + CustomNumberOfBytes, Jump.Length);

            /// Write our payload which will be redirected to using activate and deactivate hook!
            Marshal.Copy(NewInstructionBytes, 0, NewInstructionAddress, NewInstructionBytes.Length);
        }

        public byte[] Get_JumpBackBytes(IntPtr SourceAddressPointer)
        {
            /// Calculate the jump length to our own code from here.
            byte[] JumpCall = new byte[5]; // Initialize new space for making the jump call.

            // - JumpInstructionLength - RegistersToBackup is an offset from the beginning to NewInstructionAddress to end of the jump call.
            // There is NOT a second JumpInstructionLength as we are not jumping to where we originally made our jump, but the opcode right after.
            byte[] JumpLength = BitConverter.GetBytes((int)SourceAddressPointer - ( (int)NewInstructionAddress + RegistersToBackup + JumpInstructionLength + RegistersToBackup + CustomNumberOfBytes) );

            // Set up the jump call bytes!
            JumpCall[0] = 0xE9; // JMP (ASM)
            JumpCall[1] = JumpLength[0];
            JumpCall[2] = JumpLength[1];
            JumpCall[3] = JumpLength[2];
            JumpCall[4] = JumpLength[3];
            return JumpCall;
        }


        /// <summary>
        /// Returns the jump length to our own code in the new bytes of the diverted code path to newly allocated memory.
        /// </summary>
        public byte[] Get_OwnCodeJumpBytes(IntPtr DestinationAddressPointer)
        {
            /// Calculate the jump length to our own code from here.
            byte[] JumpCall = new byte[5]; // Initialize new space for making the jump call.
            // - JumpInstructionLength - RegistersToBackup is an offset from the beginning to NewInstructionAddress to end of the jump call.
            byte[] JumpLength = BitConverter.GetBytes((int)DestinationAddressPointer - (int)NewInstructionAddress - JumpInstructionLength - RegistersToBackup);

            // Set up the jump call bytes!
            JumpCall[0] = 0xE8; // JMP CALL (ASM)
            JumpCall[1] = JumpLength[0];
            JumpCall[2] = JumpLength[1];
            JumpCall[3] = JumpLength[2];
            JumpCall[4] = JumpLength[3];
            return JumpCall;
        }

        // This will activate the hook
        /// <summary>
        /// Activates the hook via the use of manual hooking. Doing this is technically faster on performance by a negligible amount, it is an option, but please for the sake of interoperability with other mods, consider 'Subscribing' this address to the Mod Loader [Currently Not Implemented].
        /// </summary>
        public void Activate()
        {
            Marshal.Copy(NewBytes, 0, HookAddress, CustomNumberOfBytes); // Copy the new bytes to the newly allocated memory where our hook will take over.
        }
        /// <summary>
        /// Deactivates the hook via the use of manual hooking. Doing this is technically faster on performance by a negligible amount, it is an option, but please for the sake of interoperability with other mods, consider 'Subscribing & Unsubscribing' this address to the Mod Loader [Currently Not Implemented].
        /// </summary>
        public void Deactivate() { Marshal.Copy(OriginalBytes, 0, HookAddress, CustomNumberOfBytes); }

        /// <summary>
        /// When we no longer need the hook, we can get rid of it and all of its traces entirely.
        /// </summary>
        public void Dispose()
        {
            Deactivate();
            Protection Dummy;
            VirtualProtect(HookAddress, (uint)CustomNumberOfBytes, OriginalMemoryProtection, out Dummy);
        }

        /// <summary>
        /// Allows for allocation of space inside the target process, in our case, Sonic Heroes. The return value for this method is the address at which the new memory has been reserved. You may use this extra space to e.g. insert assembly code to which you may jump to.
        /// </summary>
        /// <param name="Process">The process object of Sonic Heroes, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="Length">Length of free bytes you want to allocate.</param>
        /// <returns>Base pointer address to the newly allocated memory.</returns>
        public static IntPtr AllocateMemory(Process Process, int Length)
        {
            // Call VirtualAllocEx to allocate memory of fixed chosen size.
            return VirtualAllocEx(Process.Handle, IntPtr.Zero, (IntPtr)Length,
                AllocationType.Commit | AllocationType.Reserve,
                MemoryProtection.ExecuteReadWrite);
        }

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
        IntPtr dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize,
                Protection flNewProtect, out Protection lpflOldProtect);

        public enum Protection
        {
            PAGE_NOACCESS = 0x01,
            PAGE_READONLY = 0x02,
            PAGE_READWRITE = 0x04,
            PAGE_WRITECOPY = 0x08,
            PAGE_EXECUTE = 0x10,
            PAGE_EXECUTE_READ = 0x20,
            PAGE_EXECUTE_READWRITE = 0x40,
            PAGE_EXECUTE_WRITECOPY = 0x80,
            PAGE_GUARD = 0x100,
            PAGE_NOCACHE = 0x200,
            PAGE_WRITECOMBINE = 0x400
        }
    }

    /// <summary>
    /// This class allows you to place a jump in game memory, this is pretty much identical to the hook class, except that you can jump anywhere you want and must provide an address to jump to, that's about it. Note: If you are a C++ user, you may use this as an alternative to "Hook", and place a jump back to the opcode right after the jump using Inline Assembly, with C# this is not possible, P/Invoke C/C++ code will not work either.
    /// </summary>
    class Jump : IDisposable
    {
        /// <summary>
        /// This is the number of bytes that the hook in question will by default replace, we can't always use this number but we will do our best. 
        /// </summary>
        const int DefaultNumberOfBytes = 5;
        /// <summary>
        /// This is the address which we will be jumping, the address where a call jmp is placed to redirect our program flow elsewhere.
        /// </summary>
        IntPtr JumpAddress;
        /// <summary>
        /// This will store the old original memory protection which we will restore along with the original bytes should we wish to fully dispose of the hook.
        /// </summary>
        Protection OriginalMemoryProtection;
        /// <summary>
        /// The original source array of bytes which we will be a jump at.
        /// </summary>
        byte[] OriginalBytes;
        /// <summary>
        /// The new bytes which we will place to make a jump.
        /// </summary>
        byte[] NewBytes;


        /// <summary>
        /// This class allows you to place a jump in game memory, this is pretty much identical to the hook class, except that you can jump anywhere you want and must provide an address to jump to, that's about it. Note: If you are a C++ user, you may use this as an alternative to "Hook", and place a jump back to the opcode right after the jump using Inline Assembly, with C# this is not possible, P/Invoke C/C++ code will not work either.
        /// </summary>
        public Jump(IntPtr SourceAddressPointer, IntPtr DestinationAddressPointer)
        {
            // Generate the variables :)
            OriginalBytes = new byte[DefaultNumberOfBytes];

            // Remove protection from the old set of bytes such that we may remove the 
            VirtualProtect(SourceAddressPointer, DefaultNumberOfBytes, Protection.PAGE_EXECUTE_READWRITE, out OriginalMemoryProtection);
            // Copy original bytes from the source address to the byte array.
            Marshal.Copy(SourceAddressPointer, OriginalBytes, 0, DefaultNumberOfBytes);
            // This is the amount of bytes/length as an array which we will jump/insert into the jump instruction.
            byte[] JumpLength = BitConverter.GetBytes((int)DestinationAddressPointer - (int)SourceAddressPointer - DefaultNumberOfBytes);
            // Write the new bytes to make a call to our own code.
            NewBytes = new byte[DefaultNumberOfBytes]
            {
                // JMP (ASM)
                0xE9, JumpLength[0], JumpLength[1], JumpLength[2], JumpLength[3]
            };
            // Set the address which we will start hooking in the future.
            JumpAddress = SourceAddressPointer;
        }

        /// <summary>
        /// Activates the jump via the use of manual jumping. 
        /// </summary>
        public void Activate() { Marshal.Copy(NewBytes, 0, JumpAddress, DefaultNumberOfBytes); }
        /// <summary>
        /// Deactivates the jump via the use of manual jumping. 
        /// </summary>
        public void Deactivate() { Marshal.Copy(OriginalBytes, 0, JumpAddress, DefaultNumberOfBytes); }

        /// <summary>
        /// When we no longer need the hook, we can get rid of it and all of its traces entirely.
        /// </summary>
        public void Dispose()
        {
            Deactivate();
            Protection Dummy;
            VirtualProtect(JumpAddress, DefaultNumberOfBytes, OriginalMemoryProtection, out Dummy);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize,
                Protection flNewProtect, out Protection lpflOldProtect);

        public enum Protection
        {
            PAGE_NOACCESS = 0x01,
            PAGE_READONLY = 0x02,
            PAGE_READWRITE = 0x04,
            PAGE_WRITECOPY = 0x08,
            PAGE_EXECUTE = 0x10,
            PAGE_EXECUTE_READ = 0x20,
            PAGE_EXECUTE_READWRITE = 0x40,
            PAGE_EXECUTE_WRITECOPY = 0x80,
            PAGE_GUARD = 0x100,
            PAGE_NOCACHE = 0x200,
            PAGE_WRITECOMBINE = 0x400
        }
    }

}
