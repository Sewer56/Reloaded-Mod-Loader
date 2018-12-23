using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Reloaded.Assembler.Definitions
{
    public class FasmException : Exception
    {
        // TODO: This is unused: It will be used if FASMX64 ever gets proper 64bit addressing.

        /// <summary>
        /// Initializes a new instance of the <see cref="FasmException" /> class.
        /// </summary>
        /// <param name="errorCode">The error code returned.</param>
        /// <param name="errorLine">The line where the error has occured.</param>
        /// <param name="errorOffset">The offset within the data where the error line starts.</param>
        /// <param name="mnemonics">The assembled mnemonics when the error occurred.</param>
        public FasmException(FasmErrors errorCode, int errorLine, int errorOffset) : base($"Failed to assemble FASM Mnemonics: Error code: {errorCode}; Error line: {errorLine}; Error offset: {errorOffset}")
        {

        }
    }
}
