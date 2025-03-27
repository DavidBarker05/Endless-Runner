using Vector3 = UnityEngine.Vector3;
using Math = System.Math;
using MethodImpl = System.Runtime.CompilerServices.MethodImplAttribute;
using MethodImplOptions = System.Runtime.CompilerServices.MethodImplOptions;

namespace GameUtilities
{
    public struct UtilityMethods
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 YVector(float y)
        {
            return new Vector3(0f, y, 0f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 YVector(Vector3 v)
        {
            return new Vector3(0f, v.y, 0f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 HorizontalVector(Vector3 v)
        {
            return new Vector3(v.x, 0f, v.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float HorizontalDistance(Vector3 a, Vector3 b)
        {
            float dx = a.x - b.x;
            float dz = a.z - b.z;
            return (float)Math.Sqrt(dx * dx + dz * dz);
        }
    }
}
