using Vector3 = UnityEngine.Vector3;
using Math = System.Math;
using MethodImpl = System.Runtime.CompilerServices.MethodImplAttribute;
using MethodImplOptions = System.Runtime.CompilerServices.MethodImplOptions;

namespace GameUtilities
{
    public struct UtilityMethods
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 XVector(Vector3 v)
        {
            return new Vector3(v.x, 0f, 0f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 YVector(float y)
        {
            return new Vector3(0f, y, 0f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ZVector(float z)
        {
            return new Vector3(0f, 0f, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 YZVector(Vector3 v)
        {
            return new Vector3(0f, v.y, v.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float XDistance(Vector3 a, Vector3 b)
        {
            float dx = a.x - b.x;
            return (float)Math.Sqrt(dx * dx);
        }
    }
}
