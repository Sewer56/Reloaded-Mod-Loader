using System;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using Reloaded.Process.Functions.X64Functions;
using Reloaded.Process.Functions.X86Functions;
using Reloaded.Process.Memory;

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
        public delegate IntPtr CreateFile(
            [MarshalAs(UnmanagedType.LPTStr)] string filename,
            [MarshalAs(UnmanagedType.U4)] FileAccess access,
            [MarshalAs(UnmanagedType.U4)] FileShare share,
            IntPtr securityAttributes, // optional SECURITY_ATTRIBUTES struct or IntPtr.Zero
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


        /// <summary>
        /// Creates a new file or directory, or opens an existing file, device, directory, or volume.
        /// (The description here is a partial, lazy copy from MSDN)
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [X64ReloadedFunction(X64CallingConventions.Microsoft)]
        [ReloadedFunction(CallingConventions.Stdcall)]
        public delegate int NtCreateFile(out IntPtr handle, FileAccess access, ref OBJECT_ATTRIBUTES objectAttributes,
            ref IO_STATUS_BLOCK ioStatus, ref long allocSize, uint fileAttributes, FileShare share, uint createDisposition, uint createOptions,
            IntPtr eaBuffer, uint eaLength);

        /// <summary>
        /// A driver sets an IRP's I/O status block to indicate the final status of an I/O request, before calling IoCompleteRequest for the IRP.
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct IO_STATUS_BLOCK
        {
            public UInt32 status;
            public IntPtr information;
        }

        /// <summary>
        /// The OBJECT_ATTRIBUTES structure specifies attributes that can be applied to objects or object
        /// handles by routines that create objects and/or return handles to objects.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_ATTRIBUTES : IDisposable
        {
            /// <summary>
            /// Lengthm of this structure.
            /// </summary>
            public int Length;

            /// <summary>
            /// Optional handle to the root object directory for the path name specified by the ObjectName member.
            /// If RootDirectory is NULL, ObjectName must point to a fully qualified object name that includes the full path to the target object.
            /// If RootDirectory is non-NULL, ObjectName specifies an object name relative to the RootDirectory directory.
            /// The RootDirectory handle can refer to a file system directory or an object directory in the object manager namespace.
            /// </summary>
            public IntPtr RootDirectory;

            /// <summary>
            /// Pointer to a Unicode string that contains the name of the object for which a handle is to be opened.
            /// This must either be a fully qualified object name, or a relative path name to the directory specified by the RootDirectory member.
            /// </summary>
            private IntPtr objectName;

            /// <summary>
            /// Bitmask of flags that specify object handle attributes. This member can contain one or more of the flags in the following table (See MSDN)
            /// </summary>
            public uint Attributes;

            /// <summary>
            /// Specifies a security descriptor (SECURITY_DESCRIPTOR) for the object when the object is created.
            /// If this member is NULL, the object will receive default security settings.
            /// </summary>
            public IntPtr SecurityDescriptor;

            /// <summary>
            /// Optional quality of service to be applied to the object when it is created.
            /// Used to indicate the security impersonation level and context tracking mode (dynamic or static).
            /// Currently, the InitializeObjectAttributes macro sets this member to NULL.
            /// </summary>
            public IntPtr SecurityQualityOfService;

            /// <summary>
            /// You ain't gonna need it but it's here anyway.
            /// </summary>
            /// <param name="name">Specifies the full path of the file.</param>
            /// <param name="attrs">Attributes for the file.</param>
            public OBJECT_ATTRIBUTES(string name, uint attrs)
            {
                Length = 0;
                RootDirectory = IntPtr.Zero;
                objectName = IntPtr.Zero;
                Attributes = attrs;
                SecurityDescriptor = IntPtr.Zero;
                SecurityQualityOfService = IntPtr.Zero;

                Length = Marshal.SizeOf(this);
                ObjectName = new UNICODE_STRING(name);
            }

            /// <summary>
            /// Gets or sets the file path of the files loaded in or out.
            /// </summary>
            public UNICODE_STRING ObjectName
            {
                get => (UNICODE_STRING)Marshal.PtrToStructure(objectName, typeof(UNICODE_STRING));

                set
                {
                    // Check if we need to delete old memory.
                    bool fDeleteOld = objectName != IntPtr.Zero;

                    // Allocates the necessary bytes for the string.
                    if (!fDeleteOld)
                        objectName = Marshal.AllocHGlobal(Marshal.SizeOf(value));

                    // Deallocate old string while writing the new one.
                    Marshal.StructureToPtr(value, objectName, fDeleteOld);
                }
            }

            /// <summary>
            /// Disposes of the actual object name (file name) in question.
            /// </summary>
            public void Dispose()
            {
                if (objectName != IntPtr.Zero)
                {
                    Marshal.DestroyStructure(objectName, typeof(UNICODE_STRING));
                    Marshal.FreeHGlobal(objectName);
                    objectName = IntPtr.Zero;
                }
            }
        }


        /// <summary>
        /// Does this really need to be explained to you?
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct UNICODE_STRING : IDisposable
        {
            public ushort Length;
            public ushort MaximumLength;
            private IntPtr buffer;

            public UNICODE_STRING(string s)
            {
                Length = (ushort)(s.Length * 2);
                MaximumLength = (ushort)(Length + 2);
                buffer = Marshal.StringToHGlobalUni(s);
            }

            /// <summary>
            /// Disposes of the current file name assigned to this Unicode String.
            /// </summary>
            public void Dispose()
            {
                Marshal.FreeHGlobal(buffer);
                buffer = IntPtr.Zero;
            }

            /// <summary>
            /// Returns a string with the contents
            /// </summary>
            /// <returns></returns>
            [HandleProcessCorruptedStateExceptions]
            public override string ToString()
            {
                try
                {
                    byte[] uniString = Program.GameProcess.Memory.ReadRaw(buffer, Length);
                    return Encoding.Unicode.GetString(uniString);
                }
                catch { return ""; }

            }
        }
    }
}
