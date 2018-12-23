using System;
using System.Collections.Generic;
using System.Text;

namespace Reloaded.Utilities.Arrays
{
    /// <summary>
    /// Provides a generic slicing implementation based on copying for Generic
    /// arrays.
    /// </summary>
    public static class Slicing
    {
        /// <summary>
        /// Get the array slice between the two indexes.
        /// This is inclusive for start index, exclusive for end index.
        /// </summary>
        public static T[] Slice<T>(this T[] source, int start, int end)
        {
            // Handles negative offsets from end.
            if (end < 0)
                end = source.Length + end;
            
            // Get length of array of copy.
            int length = end - start;

            // Create array and copy.
            T[] destination = new T[length];
            Array.Copy(source, start, destination, 0, length);

            return destination;
        }
    }
}
