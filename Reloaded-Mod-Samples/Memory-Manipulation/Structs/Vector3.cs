using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Reloaded_Mod_Template.Structs
{
    /// <summary>
    /// Defines a simple structure defining X/Y/Z coordinates.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3
    {
        public float xPosition;
        public float yPosition;
        public float zPosition;

        public Vector3(float xPosition, float yPosition, float zPosition)
        {
            this.xPosition = xPosition;
            this.yPosition = yPosition;
            this.zPosition = zPosition;
        }

        /* The code below performs equality checking between two vectors. */
        public bool Equals(Vector3 other)
        {
            return xPosition.Equals(other.xPosition) &&
                   yPosition.Equals(other.yPosition) &&
                   zPosition.Equals(other.zPosition);
        }

        // Overload operators
        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !a.Equals(b);
        }
    }
}
