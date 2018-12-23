using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Reloaded.Memory.Buffers.Structs
{
    /// <summary>
    /// Sits at the top of every Reloaded buffer and identifies the buffer as Reloaded managed.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public unsafe struct MemoryBufferMagic
    {
        /// <summary>
        /// Contains the size of the struct.
        /// </summary>
        public const int MagicSize = 256;

        /// <summary>
        /// Standard pseudo-random generated signature to mark the start of a Reloaded buffer.
        /// </summary>
        public fixed byte ReloadedIdentifier[MagicSize];

        /// <summary>
        /// Generates a new buffer "magic" header; of size 256 bytes.
        /// </summary>
        /// <param name="initialize">Set this to true to create and calculate magic bytes.</param>
        public MemoryBufferMagic(bool initialize)
        {
            // We need the "magic" header to ensure that when we are looking through arbitrary page regions in memory;
            // we do not accidentally assume a non-Reloaded allocated region of memory as an Reloaded allocated region.

            // As these regions will be reused by different programs and mods; we need to ensure that they will be fairly unique.
            // This simple pseudo-random number generation algorithm will ensure at least that each buffer 
            // should have a consistent signature in the header by which it can be identified that will likely not cause
            // collisions with non-Reloaded data.

            // Collision Probability: (1/(256^256)) * (TotalApplicationMemoryUsageBytes - AllReloadedBufferBytes) 
            // or
            // 1/32317006071311007300714876688669951960444102669715484032130345427524655138867890893197201411522913463688717960921898019494119559150490921095088152386448283120630877367300996091750197750389652106796057638384067568276792218642619756161838094338476170470581645852036305042887575891541065808607552399123930385521914333389668342420684974786564569494856176035326322058077805659331026192708460314150258592864177116725943603718461857357598351152301645904403697613233287231227125684710820209725157101726931323469678542580656697935045997268352998638215525166389437335543602135433229604645318478604952148193555853611059596230656
            // * (TotalApplicationMemoryUsageBytes - AllReloadedBufferBytes).

            if (initialize)
                PseudoGenerate();
        }

        /// <summary>
        /// Returns true if two of the magic sequences inside the structure are equivalent.
        /// </summary>
        public bool MagicEquals(ref MemoryBufferMagic other)
        {
            fixed (byte* thisIdentifier = this.ReloadedIdentifier)
            fixed (byte* otherIdentifier = other.ReloadedIdentifier)
            {
                Span<byte> aBytes = new Span<byte>(thisIdentifier, MagicSize);
                Span<byte> bBytes = new Span<byte>(otherIdentifier, MagicSize);
                return aBytes.SequenceEqual(bBytes);
            }
        }

        /// <summary>
        /// Generates a pseudo random set of bytes in the place of "Magic".
        /// </summary>
        private void PseudoGenerate()
        {
            fixed (byte* identifierPtr = ReloadedIdentifier)
            {
                byte randomNumber = 17;
                for (int x = 0; x < MagicSize; x++)
                {
                    identifierPtr[x] = randomNumber;
                    randomNumber = (byte)((randomNumber + 13) * 31);
                }
            }
        }
    }
}
