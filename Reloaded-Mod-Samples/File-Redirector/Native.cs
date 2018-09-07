using System;
using System.IO;
using System.Runtime.InteropServices;
using Reloaded.Process.Functions.X64Functions;
using Reloaded.Process.Functions.X86Functions;

namespace Reloaded_Mod_Template
{
    /// <summary>
    /// Contains the delegates for the individual native methods
    /// that are hooked by in the filesystem monitor.
    /// </summary>
    class Native
    {
        /*
            Please note the ReloadedFunctionAttribute present in this class on the 
            individual delegate types.

            In order to support custom calling conventions, Reloaded generates CDECL compatible
            function wrappers on the fly at runtime.

            Hooking with Reloaded requires that you set the UnmanagedFunctionAttribute as 
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] regardless of the function or convention.

            Under or before this Attribute, you specify the ReloadedFunctionAttribute with the real calling convention used.
            This can either be instantiated from a pre-specified set of common calling conventions or be manually defined,
            in terms of the registers passed, the register which returns the value, caller and callee cleanup etc.
        */

        /// <summary>
        /// Creates or opens a file or I/O device. 
        /// The most commonly used I/O devices are as follows: file, file stream, directory, physical disk,
        /// volume, console buffer, tape drive, communications resource, mailslot, and pipe.
        /// The function returns a handle that can be used to access the file or device for various 
        /// types of I/O depending on the file or device and the flags and attributes specified.
        /// </summary>
        /// <param name="filename">
        ///     The name of the file or device to be created or opened. 
        ///     You may use either forward slashes (/) or backslashes (\) in this name.
        /// </param>
        /// <param name="access">The requested access to the file or device, which can be summarized as read, write, both or neither zero).</param>
        /// <param name="share">
        /// The requested sharing mode of the file or device, which can be read, write, both, delete, all of these,
        /// or none (refer to the following table on MSDN). Access requests to attributes or extended attributes are not affected by this flag.
        /// </param>
        /// <param name="securityAttributes"
        ///     >A pointer to a SECURITY_ATTRIBUTES structure that contains two separate but related data members: an optional security descriptor, 
        /// and a Boolean value that determines whether the returned handle can be inherited by child processes.
        /// </param>
        /// <param name="creationDisposition">
        ///     An action to take on a file or device that exists or does not exist.
        ///     For devices other than files, this parameter is usually set to OPEN_EXISTING.
        ///     See MSDN
        /// </param>
        /// <param name="flagsAndAttributes">
        ///     The file or device attributes and flags, FILE_ATTRIBUTE_NORMAL being the most common default value for files.
        ///     This parameter can include any combination of the available file attributes (FILE_ATTRIBUTE_*). All other file attributes override FILE_ATTRIBUTE_NORMAL.
        /// </param>
        /// <param name="templateFile">
        ///     A valid handle to a template file with the GENERIC_READ access right. 
        ///     The template file supplies file attributes and extended attributes for the file that is being created.
        ///     This parameter can be NULL.
        /// </param>
        /// <returns>If the function succeeds, the return value is an open handle to the specified file, device, named pipe, or mail slot.</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [X64ReloadedFunction(X64CallingConventions.Microsoft)]
        [ReloadedFunction(CallingConventions.Stdcall)]
        public delegate IntPtr CreateFileA(
            [MarshalAs(UnmanagedType.LPStr)] string filename,
            [MarshalAs(UnmanagedType.U4)] FileAccess access,
            [MarshalAs(UnmanagedType.U4)] FileShare share,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
            IntPtr templateFile);


        /// <summary>
        /// Creates or opens a file or I/O device. 
        /// The most commonly used I/O devices are as follows: file, file stream, directory, physical disk,
        /// volume, console buffer, tape drive, communications resource, mailslot, and pipe.
        /// The function returns a handle that can be used to access the file or device for various 
        /// types of I/O depending on the file or device and the flags and attributes specified.
        /// </summary>
        /// <param name="filename">
        ///     The name of the file or device to be created or opened. 
        ///     You may use either forward slashes (/) or backslashes (\) in this name.
        /// </param>
        /// <param name="access">The requested access to the file or device, which can be summarized as read, write, both or neither zero).</param>
        /// <param name="share">
        /// The requested sharing mode of the file or device, which can be read, write, both, delete, all of these,
        /// or none (refer to the following table on MSDN). Access requests to attributes or extended attributes are not affected by this flag.
        /// </param>
        /// <param name="securityAttributes"
        ///     >A pointer to a SECURITY_ATTRIBUTES structure that contains two separate but related data members: an optional security descriptor, 
        /// and a Boolean value that determines whether the returned handle can be inherited by child processes.
        /// </param>
        /// <param name="creationDisposition">
        ///     An action to take on a file or device that exists or does not exist.
        ///     For devices other than files, this parameter is usually set to OPEN_EXISTING.
        ///     See MSDN
        /// </param>
        /// <param name="flagsAndAttributes">
        ///     The file or device attributes and flags, FILE_ATTRIBUTE_NORMAL being the most common default value for files.
        ///     This parameter can include any combination of the available file attributes (FILE_ATTRIBUTE_*). All other file attributes override FILE_ATTRIBUTE_NORMAL.
        /// </param>
        /// <param name="templateFile">
        ///     A valid handle to a template file with the GENERIC_READ access right. 
        ///     The template file supplies file attributes and extended attributes for the file that is being created.
        ///     This parameter can be NULL.
        /// </param>
        /// <returns>If the function succeeds, the return value is an open handle to the specified file, device, named pipe, or mail slot.</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [X64ReloadedFunction(X64CallingConventions.Microsoft)]
        [ReloadedFunction(CallingConventions.Stdcall)]
        public delegate IntPtr CreateFileW(
            [MarshalAs(UnmanagedType.LPWStr)] string filename,
            [MarshalAs(UnmanagedType.U4)] FileAccess access,
            [MarshalAs(UnmanagedType.U4)] FileShare share,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
            IntPtr templateFile);
    }
}
