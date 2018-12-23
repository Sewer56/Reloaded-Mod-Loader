namespace Reloaded.Assembler.Definitions
{
    public enum FasmResult : int
    {

        Ok = 0,
        Working = 1,
        Error = 2,
        InvalidParameter = -1,
        OutOfMemory = -2,
        StackOverflow = -3,
        SourceNotFound = -4,
        UnexpectedEndOfSource = -5,
        CannotGenerateCode = -6,
        FormatLimitationsExcedded = -7,
        WriteFailed = -8
    }
}
