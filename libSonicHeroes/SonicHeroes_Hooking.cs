using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

/// <summary>
/// This namespace stores all code manipulation, code redirection, jumping among other methods responsible for manipulating the direct flow of program execution.
/// </summary>
namespace SonicHeroes.Hooking
{
    /// <summary>
    /// This class allows you to create a hook for the game, allowing you to replace an ingame piece of code with a call, or quite simply a statement to go off execute your own function/code. A hook requires at least 5 bytes and some code which you would be happy to scrap in order to instead run your hook. A hooking method which does not require scrapping existing game code will come soon.
    /// </summary>
    class Hook : IDisposable
    {
        /// <summary>
        /// This is the number of bytes that the hook in question will by default replace, we can't always use this number but we will do our best. 
        /// </summary>
        const int DefaultNumberOfBytes = 5;

        /// <summary>
        /// An optional user defined number of bytes to replace while performing a hook, you may use this to ensure that no stray bytes are left from another instruction.
        /// </summary>
        int CustomNumberOfBytes = 0;

        /// <summary>
        /// This is the address which we will be hooking, the address where a call jmp is placed to redirect our program flow to our own function.
        /// </summary>
        IntPtr HookAddress;
        /// <summary>
        /// [Unused in C#] The address which will be jumped back to after the hook operation completes, might be useful for the C++ junkies who might want to port this or give this a try if calling a C++ DLL and wanting to jump back themselves.
        /// </summary>
        int JumpBackAddress;
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
        public Hook
            (IntPtr SourceAddressPointer, Delegate DestinationAddressPointer) 
            : this(SourceAddressPointer, Marshal.GetFunctionPointerForDelegate(DestinationAddressPointer)) { }

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
        // Hook length must be at least 6 bytes.
        // When creating a hook, you must ensure to not leave any stray bytes open in the wild.
        public Hook(IntPtr SourceAddressPointer, IntPtr DestinationAddressPointer)
        {
            // Generate the variables :)
            OriginalBytes = new byte[DefaultNumberOfBytes];
            JumpBackAddress = (int)SourceAddressPointer + DefaultNumberOfBytes;

            // Remove protection from the old set of bytes such that we may remove the 
            VirtualProtect(SourceAddressPointer, DefaultNumberOfBytes, Protection.PAGE_EXECUTE_READWRITE, out OriginalMemoryProtection);
            // Copy original bytes from the source address to the byte array.
            Marshal.Copy(SourceAddressPointer, OriginalBytes, 0, DefaultNumberOfBytes); 
            // This is the amount of bytes/length as an array which we will jump/insert into the jump instruction.
            byte[] JumpLength = BitConverter.GetBytes((int)DestinationAddressPointer - (int)SourceAddressPointer - DefaultNumberOfBytes);
            // Write the new bytes to make a call to our own code.
            NewBytes = new byte[DefaultNumberOfBytes]
            {
                // Call JMP (ASM)
                0xE8, JumpLength[0], JumpLength[1], JumpLength[2], JumpLength[3]
            };
            // Set the address which we will start hooking in the future.
            HookAddress = SourceAddressPointer;
        }

        // Creates a hook.
        // Hook length defines the amount of bytes which will be replaced by the hook.
        // Hook length must be at least 6 bytes.
        // When creating a hook, you must ensure to not leave any stray bytes open in the wild.
        public Hook(IntPtr SourceAddressPointer, IntPtr DestinationAddressPointer, int HookLength)
        {
            // Generate the variables :)
            CustomNumberOfBytes = HookLength;
            OriginalBytes = new byte[CustomNumberOfBytes];
            JumpBackAddress = (int)SourceAddressPointer + CustomNumberOfBytes;

            // Remove protection from the old set of bytes such that we may remove the 
            VirtualProtect(SourceAddressPointer, (uint)CustomNumberOfBytes, Protection.PAGE_EXECUTE_READWRITE, out OriginalMemoryProtection);
            // Copy original bytes from the source address to the byte array.
            Marshal.Copy(SourceAddressPointer, OriginalBytes, 0, CustomNumberOfBytes);
            // This is the amount of bytes/length as an array which we will jump/insert into the jump instruction.
            byte[] JumpLength = BitConverter.GetBytes((int)DestinationAddressPointer - (int)SourceAddressPointer - CustomNumberOfBytes);
            
            // Write the new bytes to make a call to our own code.
            NewBytes = new byte[CustomNumberOfBytes];
            NewBytes[0] = 0xE8; // Call JMP (ASM).
            NewBytes[1] = JumpLength[0];
            NewBytes[2] = JumpLength[1];
            NewBytes[3] = JumpLength[2];
            NewBytes[4] = JumpLength[3];
            // Write NOP for the remainder of the NewBytes array.
            for (int x = 4; x < CustomNumberOfBytes; x++) { JumpLength[x] = 0x90; }

            // Set the address which we will start hooking in the future.
            HookAddress = SourceAddressPointer;
        }

        // This will activate the hook
        /// <summary>
        /// Activates the hook via the use of manual hooking. Doing this is technically faster on performance by a negligible amount, it is an option, but please for the sake of interoperability with other mods, consider 'Subscribing' this address to the Mod Loader [Currently Not Implemented].
        /// </summary>
        public void Activate() { Marshal.Copy(NewBytes, 0, HookAddress, DefaultNumberOfBytes); }
        /// <summary>
        /// Deactivates the hook via the use of manual hooking. Doing this is technically faster on performance by a negligible amount, it is an option, but please for the sake of interoperability with other mods, consider 'Subscribing & Unsubscribing' this address to the Mod Loader [Currently Not Implemented].
        /// </summary>
        public void Deactivate() { Marshal.Copy(OriginalBytes, 0, HookAddress, DefaultNumberOfBytes); }

        /// <summary>
        /// When we no longer need the hook, we can get rid of it and all of its traces entirely.
        /// </summary>
        public void Dispose()
        {
            Deactivate();
            Protection Dummy;
            VirtualProtect(HookAddress, DefaultNumberOfBytes, OriginalMemoryProtection, out Dummy);
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

        // Creates a jump.
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
