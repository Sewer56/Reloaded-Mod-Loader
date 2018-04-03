using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloaded.GameProcess.Functions.CustomFunctionFactory
{
    /// <summary>
    /// Class which stores the function information for custom functions
    /// to be called or hooked upon.
    /// See <see cref="X86CallingConventions" /> for information on settings
    /// for common calling conventions.
    /// </summary>
    public class ReloadedFunction : Attribute
    {
        /// <summary>
        /// Specifies the registers in left to right parameter order to pass to the custom function to be called.
        /// </summary>
        public Register[] SourceRegisters { get; set; }

        /// <summary>
        /// Specifies the register to return the value from the funtion in (mov eax, source).
        /// This is typically eax.
        /// </summary>
        public Register ReturnRegister { get; set; }

        /// <summary>
        /// Defines the stack cleanup rule for the function.
        /// Callee: Stack pointer restored inside the game function we are executing).
        /// Caller: Stack pointer restored in our own wrapper function.
        /// </summary>
        public StackCleanup Cleanup { get; set; }

        /// <summary>
        /// Specifies the target X86 ISA register for a specific parameter.
        /// </summary>
        public enum Register
        {
            eax,
            ebx,
            ecx,
            edx,
            esi,
            edi,
            ebp,
            esp
        }
        
        /// <summary>
        /// Declares who performs the stack cleanup duty.
        /// </summary>
        public enum StackCleanup
        {
            Caller,
            Callee
        }
    }
}
