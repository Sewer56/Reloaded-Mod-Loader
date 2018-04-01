using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reloaded.GameProcess.Functions.CustomFunctionFactory;

namespace Reloaded.GameProcess.Functions
{
    public class X86CallingConventions
    {
        /// <summary>
        /// Defines the individual supported X86 Calling Conventions for the
        /// </summary>
        public enum CallingConventions
        {
            /// <summary>
            /// Parameters are passed right to left onto the function pushing onto the stack.
            /// Calling function pops its own arguments from the stack. (The calling function must manually restore the stack to previous state)
            /// </summary>
            Cdecl,

            /// <summary>
            /// Parameters are passed right to left onto the function pushing onto the stack.
            /// Called function pops its own arguments from the stack.
            /// </summary>
            Stdcall,

            /// <summary>
            /// The first two arguments are passed in from left to right into ECX and EDX.
            /// The others are passed in right to left onto stack.
            /// Caller cleanup: If necessary, the stack is cleaned up by the caller.
            /// </summary>
            Fastcall,

            /// <summary>
            /// Variant of Stdcall where the pointer of the `this` object is passed into ECX and
            /// rest of the parameters passed as usual.
            /// 
            /// Callee cleans the stack.
            /// 
            /// You should define your delegates with the (this) object pointer (IntPtr) as first parameter from the left.
            /// 
            /// GCC thiscall: 
            ///     `this` object passed as first parameter, caller stack cleanup, use cdecl with `this` 
            ///     object pointer (IntPtr) as first parameter instead.
            /// </summary>
            Thiscall,

            /// <summary>
            /// Not a real calling convention, parameters and their registers are 
            /// manually specified by the user in a left to right order of parameters.
            /// </summary>
            Usercall
        }

        /// <summary>
        /// Retrieves calling convention function information for a specific calling convention.
        /// Currently supports thiscall and fastcall, C# can natively work with cdecl and stdcall already.
        /// </summary>
        /// <param name="convention">The information for </param>
        /// <returns></returns>
        public static FunctionInformation GetCallingConventionInformation(CallingConventions convention)
        {
            switch (convention)
            {
                case CallingConventions.Thiscall:
                    return new FunctionInformation()
                    {
                        returnRegister = FunctionInformation.TargetRegister.eax,
                        sourceRegisters = new[]
                        {
                            FunctionInformation.TargetRegister.ecx,
                        },
                        stackCleanup = FunctionInformation.StackCleanup.Callee
                    };

                case CallingConventions.Fastcall:
                    return new FunctionInformation()
                    {
                        returnRegister = FunctionInformation.TargetRegister.eax,
                        sourceRegisters = new[]
                        {
                            FunctionInformation.TargetRegister.ecx,
                            FunctionInformation.TargetRegister.edx
                        },
                        stackCleanup = FunctionInformation.StackCleanup.Caller
                    };

                // Default should comply with cdecl
                default:
                    return new FunctionInformation()
                    {
                        returnRegister =  FunctionInformation.TargetRegister.eax,
                        sourceRegisters = new FunctionInformation.TargetRegister[0],
                        stackCleanup = FunctionInformation.StackCleanup.Caller
                    };
            }
        }
    }
}
