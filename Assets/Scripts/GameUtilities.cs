using v3 = UnityEngine.Vector3;

namespace GameUtilities
{
    public struct UtilityMethods
    {
        public static v3 YVector(float y) => v3.up * y;
        public static v3 YVector(v3 v) => YVector(v.y);
        public static v3 HorizontalVector(v3 v) => new v3(v.x, 0f, v.z);
        public static float HorizontalDistance(v3 a, v3 b) => v3.Distance(HorizontalVector(a), HorizontalVector(b));
    }
}
