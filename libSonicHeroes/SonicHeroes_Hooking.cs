using System;
using System.Runtime.InteropServices;
using SonicHeroes.Networking;
using System.Diagnostics;
using System.Net.Sockets;
using static SonicHeroes.Networking.Client_Functions;
using System.Collections.Generic;

/// <summary>
/// This namespace stores all code manipulation, code redirection, jumping among other methods responsible for manipulating the direct flow of program execution.
/// </summary>
namespace SonicHeroes.Hooking
{
    /// <summary>
    /// This class allows you to create a hook for the game, allowing you to replace an ingame piece of code with a call, or quite simply a statement to go off execute your own function/code. A hook requires at least 5 bytes and some code which you would be happy to scrap in order to instead run your hook. You must set a length of 5 + any stray/leftover instruction bytes AND make sure you are not in a register dependent assembly code as this solution does not backup registers. For more information, do refer to the Wiki on Github.
    /// </summary>
    public class Hook : IDisposable
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
        /// Hold a copy of the delegate to the method we want to execute. Otherwise the .NET Garbage Collector will nuke it and spectacularly crash Sonic Heroes since it probably thinks the game is garbage.
        /// </summary>
        private Delegate CustomMethodDelegate;
        /// <summary>
        /// Might aswell also store the function pointer to this delegate, .NET Garbage Collector is a scary monster! Choo Choo!
        /// </summary>
        private IntPtr FunctionPointerForCustomMethodDelegate;

        /// <summary>
        /// This will effectively call the other FunctionHook, but with an IntPtr to the destination of the method we will want to use, as this one will retrieve an address to the destination method using the delegate signature. 
        /// </summary>
        /// <param name="SourceAddressPointer">The address at which we will start our hook process.</param>
        /// <param name="DestinationAddressPointer">Delegate to the method we will want to run. (DelegateName)Method</param>
        /// <param name="HookLength">The amount of bytes the hook lasts, all stray bytes will be replaced with NOP/No Operation.</param>
        public Hook (IntPtr SourceAddressPointer, Delegate DestinationAddressPointer, int HookLength)
        {
            CustomMethodDelegate = DestinationAddressPointer;
            FunctionPointerForCustomMethodDelegate =  Marshal.GetFunctionPointerForDelegate(DestinationAddressPointer);
            Hook_Hook(SourceAddressPointer, Marshal.GetFunctionPointerForDelegate(DestinationAddressPointer), HookLength);
        }

        // Creates a hook.
        // Hook length defines the amount of bytes which will be replaced by the hook.
        // Hook length must be at least 5 bytes.
        // When creating a hook, you must ensure to not leave any stray bytes open in the wild.
        public void Hook_Hook(IntPtr SourceAddressPointer, IntPtr DestinationAddressPointer, int HookLength)
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
    /// This class is an alternative for the Hook class, this generates a call to your code, however with the use of a clever jump following, still executes the original code after your code has finished executing. To use this hook, you require at least a hook length of 5 bytes + any stray bytes from any instruction. For more information, do refer to the Wiki on Github.
    /// </summary>
    public class Injection // : IDisposable
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
        public IntPtr HookAddress;
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
        /// Hold a copy of the delegate to the method we want to execute. Otherwise the .NET Garbage Collector will nuke it and spectacularly crash Sonic Heroes since it probably thinks the game is garbage.
        /// </summary>
        private Delegate CustomMethodDelegate;
        /// <summary>
        /// Might aswell also store the function pointer to this delegate, .NET Garbage Collector is a scary monster! Choo Choo!
        /// </summary>
        public IntPtr Function_Pointer_Own_Method_Call;

        /// <summary>
        /// List of subscribed methods!
        /// </summary>
        private List<Delegate> Subscribed_Delegates = new List<Delegate>();

        /// <summary>
        /// If an address has already been activated, do not allow for the same action to occur again.
        /// </summary>
        private bool Already_Activated = false;

        /// <summary>
        /// This class is an alternative for the Hook class, this generates a call to your code, however with the use of a clever jump following, still executes the original code after your code has finished executing.
        /// </summary>
        /// <param name="SourceAddressPointer">The address at which we will start our hook process.</param>
        /// <param name="DestinationAddressPointer">Delegate to the method we will want to run. (DelegateName)Method</param>
        /// <param name="HookLength">The amount of bytes the hook lasts, all stray bytes will be replaced with NOP/No Operation (technically), however the opcode call itself will be fully preserved, do not worry.</param>
        public Injection (IntPtr SourceAddressPointer, Delegate DestinationAddressPointer, int HookLength, SonicHeroes.Networking.WebSocket_Client Mod_Loader_Server_Socket)
        {
            CustomMethodDelegate = DestinationAddressPointer;
            Function_Pointer_Own_Method_Call = Marshal.GetFunctionPointerForDelegate(CustomMethodDelegate);
            Injection_Hook(SourceAddressPointer, Function_Pointer_Own_Method_Call, HookLength, Mod_Loader_Server_Socket);
        }

        // Hook Length Must be 5 bytes + any stray bytes!
        public void Injection_Hook(IntPtr SourceAddressPointer, IntPtr DestinationAddressPointer, int HookLength, SonicHeroes.Networking.WebSocket_Client Mod_Loader_Server_Socket)
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

            /// Minor extra check to see if our hook address is already hooked!
            byte[] Hook_Address_Function_Address = SonicHeroes.Networking.Client_Functions.Serialize_Subscribe_Hook_Handler(HookAddress, (IntPtr)0); // We do not need a function address, we are only checking hook.
            byte[] Response = Mod_Loader_Server_Socket.SendData_Alternate(Message_Type.Client_Call_Check_Address_Hook_State, Hook_Address_Function_Address, true);
            if (Response[0] == (byte)Client_Functions.Message_Type.Reply_Function_Already_Hooked) { Already_Activated = true; }
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
        /// Activates the hook via the use of manual hooking. Doing this is technically faster on performance by a negligible amount, it is an option, but please for the sake of interoperability with other mods, consider 'Subscribing' this address to the Mod Loader.
        /// </summary>
        public void Activate()
        {
            // If not yet active!
            if (! Already_Activated)
            {
                Marshal.Copy(NewBytes, 0, HookAddress, CustomNumberOfBytes); // Copy the new bytes to the newly allocated memory where our hook will take over.
            }
        }

        /// <summary>
        /// Deactivates the hook via the use of manual hooking. Doing this is technically faster on performance by a negligible amount, it is an option, but please for the sake of interoperability with other mods, consider 'Subscribing & Unsubscribing' this address to the Mod Loader [Currently Not Implemented].
        /// </summary>
        /// public void Deactivate() { Marshal.Copy(OriginalBytes, 0, HookAddress, CustomNumberOfBytes); }

        /// <summary>
        /// Subscribes a function pointer to the hooked address. Basically adds a method address to list of addresses held in the mod loader.
        /// </summary>
        public void Subscribe(SonicHeroes.Networking.WebSocket_Client SocketX, Delegate Function_To_Run)
        {
            // Subscribed Delegate must be kept within the class otherwise the game crashes after ~3 seconds of running... Why? I have no freaking idea!
            Subscribed_Delegates.Add(Function_To_Run);
            Subscribe_Internal(SocketX, Marshal.GetFunctionPointerForDelegate(Function_To_Run));
        }

        private void Subscribe_Internal(SonicHeroes.Networking.WebSocket_Client SocketX, IntPtr Function_Address)
        {
            byte[] Hook_Address_Function_Address = SonicHeroes.Networking.Client_Functions.Serialize_Subscribe_Hook_Handler(HookAddress, Function_Address);
            byte[] Response = SocketX.SendData_Alternate(Message_Type.Client_Call_Subscribe_DLL_Function, Hook_Address_Function_Address, true);
            if (Response[0] == (byte)Client_Functions.Message_Type.Reply_Function_Already_Hooked) { Already_Activated = true; }
        }

        /// <summary>
        /// When we no longer need the hook, we can get rid of it and all of its traces entirely.
        /// </summary>
        /*
        public void Dispose()
        {
            Deactivate();
            Protection Dummy;
            VirtualProtect(HookAddress, (uint)CustomNumberOfBytes, OriginalMemoryProtection, out Dummy);
        }
        */

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
    public class Jump : IDisposable
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

    /// <summary>
    /// Injecting own code is fun, but what about some ASM? Well, this will let you insert a code block of ASM of your own choice into the program and do whatever you want there, backups and restores registers fully, cool, isn't it? If you want to append more to ASM anywhere within existing code rather than running your own code block and having execution state registered to same as prior to that, consider ASM_Hook instead.
    /// </summary>
    public class ASM_Injection : IDisposable
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
        /// These are the bytes which will be stored at the new instruction address which correspond to assembly instructions. Here in this memory region, ASM to backup the registers will be written, our own assembly will be executed, registers will be restored and a jump will be made back.
        /// </summary>
        byte[] NewInstructionBytes;


        /// <summary>
        /// Injecting own code is fun, but what about some ASM? Well, this will let you insert a code block of ASM of your own choice into the program and do whatever you want there, backups and restores registers fully, cool, isn't it? If you want to append more to ASM anywhere within existing code rather than running your own code block and having execution state registered to same as prior to that, consider ASM_Hook instead.
        /// </summary>
        /// <param name="SourceAddressPointer">The address in memory where you will want to insert your own Assembly code to be ran.</param>
        /// <param name="Assembly_Bytes_To_Inject">Your assembly code written as an array of bytes.</param>
        /// <param name="HookLength">5 Bytes + Any Stray Bytes</param>
        public ASM_Injection(IntPtr SourceAddressPointer, byte[] Assembly_Bytes_To_Inject, int HookLength)
        {
            /// The Setup
            /// Getting Ready for Hooking!
            CustomNumberOfBytes = HookLength;         // Set hook length.
            OriginalBytes = new byte[CustomNumberOfBytes]; // Initialize storage of original bytes.
            NewBytes = new byte[CustomNumberOfBytes]; // Initialize storage of new bytes.
            HookAddress = SourceAddressPointer;       // Set the address which we will start hooking in the future.

            /// Backup Original Bytes
            /// | Remove protection from the old set of bytes such that we may alter the bytes & copy original bytes from the source address to the byte array.

            VirtualProtect(SourceAddressPointer, (uint)CustomNumberOfBytes, Protection.PAGE_EXECUTE_READWRITE, out OriginalMemoryProtection);
            Marshal.Copy(SourceAddressPointer, OriginalBytes, 0, CustomNumberOfBytes);

            /// Allocate New Memory
            /// Here in this memory region, ASM to backup the registers will be written, a call to our own method, will be performed, registers will be restored and a jump will be made back.

            // Total Bytes: Register Backup (6) + Assembly to Inject (x) + Register Restore (6) + Custom Bytes (Old Opcodes) + JMP Back (5);
            NewInstructionAddress = AllocateMemory(Process.GetCurrentProcess(), RegistersToBackup + Assembly_Bytes_To_Inject.Length + RegistersToBackup + CustomNumberOfBytes + JumpInstructionLength); // Allocate memory to write old bytes in Sonic Heroes.

            byte[] NewInstructionJumpLength = BitConverter.GetBytes((int)NewInstructionAddress - (int)SourceAddressPointer - JumpInstructionLength); // The jump length to get our code execution to move to 
            NewInstructionBytes = new byte[RegistersToBackup + Assembly_Bytes_To_Inject.Length + RegistersToBackup + CustomNumberOfBytes + JumpInstructionLength]; // Allocate byte array of required length.

            // Write the new bytes to jump to this new address where everything will happen.
            NewBytes[0] = 0xE9; // JMP (ASM).
            NewBytes[1] = NewInstructionJumpLength[0];
            NewBytes[2] = NewInstructionJumpLength[1];
            NewBytes[3] = NewInstructionJumpLength[2];
            NewBytes[4] = NewInstructionJumpLength[3];
            for (int x = 5; x < CustomNumberOfBytes; x++) { NewBytes[x] = 0x90; } // If necessary, replace any stray bytes with NOP.

            /// The Payload
            /// This is the bytes that we will write at the new Instruction Address to perform various actions.

            /// Insert the backup of registers into the payload.
            Array.Copy(ASM_PushRegistersBytes, 0, NewInstructionBytes, 0, ASM_PushRegistersBytes.Length);

            /// Insert own code into the payload.
            Array.Copy(Assembly_Bytes_To_Inject, 0, NewInstructionBytes, ASM_PushRegistersBytes.Length, Assembly_Bytes_To_Inject.Length);

            /// Insert the restore of registers into the payload.
            Array.Copy(ASM_PopRegistersBytes, 0, NewInstructionBytes, ASM_PushRegistersBytes.Length + Assembly_Bytes_To_Inject.Length, ASM_PushRegistersBytes.Length);

            /// Insert the original bytes to be executed!
            Array.Copy(OriginalBytes, 0, NewInstructionBytes, ASM_PushRegistersBytes.Length + Assembly_Bytes_To_Inject.Length + ASM_PushRegistersBytes.Length, OriginalBytes.Length);

            /// Get and write the jump back address to new array.
            byte[] Jump = Get_JumpBackBytes(SourceAddressPointer, Assembly_Bytes_To_Inject.Length);
            Array.Copy(Jump, 0, NewInstructionBytes, ASM_PushRegistersBytes.Length + Assembly_Bytes_To_Inject.Length + ASM_PopRegistersBytes.Length + CustomNumberOfBytes, Jump.Length);

            /// Write our payload which will be redirected to using activate and deactivate hook!
            Marshal.Copy(NewInstructionBytes, 0, NewInstructionAddress, NewInstructionBytes.Length);
        }

        public byte[] Get_JumpBackBytes(IntPtr SourceAddressPointer, int ASMInjectionLength)
        {
            /// Calculate the jump length to our own code from here.
            byte[] JumpCall = new byte[5]; // Initialize new space for making the jump call.

            // - JumpInstructionLength - RegistersToBackup is an offset from the beginning to NewInstructionAddress to end of the jump call.
            // There is NOT a second JumpInstructionLength as we are not jumping to where we originally made our jump, but the opcode right after.
            byte[] JumpLength = BitConverter.GetBytes((int)SourceAddressPointer - ((int)NewInstructionAddress + RegistersToBackup + ASMInjectionLength + RegistersToBackup + CustomNumberOfBytes));

            // Set up the jump call bytes!
            JumpCall[0] = 0xE9; // JMP (ASM)
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
    /// This will let you insert a code block of ASM of your own choice before the first instruction of the address and range address you are hooking in order to add additional assembly opcodes after a @gamespecified address.
    /// </summary>
    public class ASM_Hook : IDisposable
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
        /// This will point to where the backing up of the registers will occur, the method call for the dll and the restoration of the registers, running of the original code and jumping back will occur.
        /// </summary>
        IntPtr NewInstructionAddress;

        /// <summary>
        /// These are the bytes which will be stored at the new instruction address which correspond to assembly instructions. Here in this memory region, ASM to backup the registers will be written, our own assembly will be executed, registers will be restored and a jump will be made back.
        /// </summary>
        byte[] NewInstructionBytes;


        /// <summary>
        /// Sometimes you don't want to do ASM Injection, but just want to insert a few opcodes between a few existing opcodes to do one or more thing and call it a day. No problemo, we've got you covered!
        /// </summary>
        /// <param name="SourceAddressPointer">The address in memory where you will want to insert your own Assembly code to be ran.</param>
        /// <param name="Assembly_Bytes_To_Inject">Your assembly code written as an array of bytes.</param>
        /// <param name="HookLength">5 Bytes + Any Stray Bytes</param>
        public ASM_Hook(IntPtr SourceAddressPointer, byte[] Assembly_Bytes_To_Inject, int HookLength)
        {
            ///
            /// The Setup
            /// Getting Ready for Hooking!
            CustomNumberOfBytes = HookLength;         // Set hook length.
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

            // Total Bytes: Register Backup (6) + Assembly to Inject (x) + Register Restore (6) + Custom Bytes (Old Opcodes) + JMP Back (5);
            NewInstructionAddress = AllocateMemory(Process.GetCurrentProcess(), Assembly_Bytes_To_Inject.Length + CustomNumberOfBytes + JumpInstructionLength); // Allocate memory to write old bytes in Sonic Heroes.
            byte[] NewInstructionJumpLength = BitConverter.GetBytes((int)NewInstructionAddress - (int)SourceAddressPointer - JumpInstructionLength); // The jump length to get our code execution to move to 
            NewInstructionBytes = new byte[Assembly_Bytes_To_Inject.Length + CustomNumberOfBytes + JumpInstructionLength]; // Allocate byte array of required length.

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

            /// Insert own code into the payload.
            Array.Copy(Assembly_Bytes_To_Inject, 0, NewInstructionBytes, 0, Assembly_Bytes_To_Inject.Length);

            /// Insert the original bytes to be executed!
            Array.Copy(OriginalBytes, 0, NewInstructionBytes, Assembly_Bytes_To_Inject.Length, OriginalBytes.Length);

            /// Get and write the jump back address to new array.
            byte[] Jump = Get_JumpBackBytes(SourceAddressPointer, Assembly_Bytes_To_Inject.Length);
            Array.Copy(Jump, 0, NewInstructionBytes, Assembly_Bytes_To_Inject.Length + CustomNumberOfBytes, Jump.Length);

            /// Write our payload which will be redirected to using activate and deactivate hook!
            Marshal.Copy(NewInstructionBytes, 0, NewInstructionAddress, NewInstructionBytes.Length);
        }

        public byte[] Get_JumpBackBytes(IntPtr SourceAddressPointer, int ASMInjectionLength)
        {
            /// Calculate the jump length to our own code from here.
            byte[] JumpCall = new byte[5]; // Initialize new space for making the jump call.

            // - JumpInstructionLength - RegistersToBackup is an offset from the beginning to NewInstructionAddress to end of the jump call.
            // There is NOT a second JumpInstructionLength as we are not jumping to where we originally made our jump, but the opcode right after.
            byte[] JumpLength = BitConverter.GetBytes((int)SourceAddressPointer - ((int)NewInstructionAddress + ASMInjectionLength+ CustomNumberOfBytes));

            // Set up the jump call bytes!
            JumpCall[0] = 0xE9; // JMP (ASM)
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

}
