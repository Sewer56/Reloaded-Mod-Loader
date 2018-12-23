using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Reloaded.Assembler.Definitions
{
    public class FasmSimpleException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FasmException" /> class.
        /// </summary>
        /// <param name="errorCode">The error code returned.</param>
        public FasmSimpleException(FasmErrors errorCode) : base($"Failed to assemble FASM Mnemonics: Error code: {errorCode.ToString()}")
        {

        }
    }
}
