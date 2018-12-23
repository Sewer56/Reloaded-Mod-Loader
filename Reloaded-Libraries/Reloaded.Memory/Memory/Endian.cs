using System;
using System.Collections.Generic;
using System.Text;

namespace Reloaded.Memory
{
    public static class Endian
    {
        /// <summary>
        /// Reverses the endian of a primitive value (not struct).
        /// </summary>
        /// <returns>The endian type of a primitive value. (not struct)</returns>
        public static T Reverse<T>(ref T type, bool marshalElement) where T : unmanaged
        {
            // Declare an array for storing the data.
            byte[] data = Struct.GetBytes(ref type, marshalElement);
            Array.Reverse(data);

            // Use this base object for the storage of the value we are retrieving.
            return Struct.FromArray<T>(data, 0, marshalElement);
        }


    }
}
