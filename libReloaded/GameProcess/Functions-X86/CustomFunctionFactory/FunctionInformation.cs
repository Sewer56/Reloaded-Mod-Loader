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
    /// </summary>
    public class FunctionInformation
    {
        /// <summary>
        /// Specifies the registers in left to right parameter order to pass to the custom function to be called.
        /// </summary>
        public TargetRegister[] sourceRegisters;

        /// <summary>
        /// Specifies the register to return the value from the funtion in (mov eax, source).
        /// This is typically eax.
        /// </summary>
        public TargetRegister returnRegister;

        /// <summary>
        /// Defines the stack cleanup rule for the function.
        /// Callee: Stack pointer restored inside the game function we are executing).
        /// Caller: Stack pointer restored in our own wrapper function.
        /// </summary>
        public StackCleanup stackCleanup;

        /// <summary>
        /// Specifies the target X86 ISA
        /// Register for a specific parameter.
        /// </summary>
        public enum TargetRegister
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
        /// Declares who performs the stack cleanup duty,
        /// the function that 
        /// </summary>
        public enum StackCleanup
        {
            Caller,
            Callee
        }
    }
}
