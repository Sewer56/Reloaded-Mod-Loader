using System;

namespace Reloaded.Utilities.Mathematics
{
    public static class Mathematics
    {
        /// <summary>
        /// Defines a physical address range with a minimum and maximum address.
        /// </summary>
        public struct AddressRange
        {
            public IntPtr StartPointer;
            public IntPtr EndPointer;

            public AddressRange(IntPtr startPointer, IntPtr endPointer)
            {
                StartPointer = startPointer;
                EndPointer = endPointer;
            }

            /// <summary>
            /// Returns true if the other address range is completely inside
            /// the current address range.
            /// </summary>
            public bool Contains(AddressRange otherRange)
            {
                if ((long)otherRange.StartPointer >= (long)this.StartPointer &&
                    (long)otherRange.EndPointer <= (long)this.EndPointer)
                    return true;
                
                return false;
            }
        }

        /// <summary>
        /// Rounds up a specified number to the next multiple of X.
        /// </summary>
        /// <param name="number">The number to round up.</param>
        /// <param name="multiple">The multiple the number should be rounded to.</param>
        /// <returns></returns>
        public static int RoundUp(int number, int multiple)
        {
            if (multiple == 0)
                return number;

            int remainder = number % multiple;
            if (remainder == 0)
                return number;

            return number + multiple - remainder;
        }


        /// <summary>
        /// Rounds up a specified number to the next multiple of X.
        /// </summary>
        /// <param name="number">The number to round up.</param>
        /// <param name="multiple">The multiple the number should be rounded to.</param>
        /// <returns></returns>
        public static long RoundUp(long number, long multiple)
        {
            if (multiple == 0)
                return number;

            long remainder = number % multiple;
            if (remainder == 0)
                return number;

            return number + multiple - remainder;
        }


        /// <summary>
        /// Rounds up a specified number to the previous multiple of X.
        /// </summary>
        /// <param name="number">The number to round down.</param>
        /// <param name="multiple">The multiple the number should be rounded to.</param>
        /// <returns></returns>
        public static long RoundDown(long number, long multiple)
        {
            if (multiple == 0)
                return number;

            long remainder = number % multiple;
            if (remainder == 0)
                return number;

            return number - remainder;
        }

        /// <summary>
        /// Returns a set percentage of a passed in number.
        /// </summary>
        /// <param name="number">The number to get percentage of.</param>
        /// <param name="percentage">e.g. 50 means 50%</param>
        /// <returns></returns>
        public static long GetPercentageInteger(long number, int percentage)
        {
            return (long) (number / 100F * (float) percentage);
        }
        
        /// <summary>
        /// Returns a set percentage of a passed in number.
        /// </summary>
        /// <param name="number">The number to get percentage of.</param>
        /// <param name="percentage">e.g. 50 means 50%</param>
        /// <returns></returns>
        public static double GetPercentage(long number, int percentage)
        {
            return number / 100F * percentage;
        }

        /// <summary>
        /// Returns a set percentage of a passed in number.
        /// </summary>
        /// <param name="number">The number to get percentage of.</param>
        /// <param name="percentage">e.g. 50 means 50%</param>
        /// <returns></returns>
        public static double GetPercentage(double number, double percentage)
        {
            return number / 100F * percentage;
        }
    }
}
