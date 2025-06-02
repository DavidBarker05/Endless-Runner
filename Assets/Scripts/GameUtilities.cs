using Vector3 = UnityEngine.Vector3;
using GameObject = UnityEngine.GameObject;
using Math = System.Math;
using MethodImpl = System.Runtime.CompilerServices.MethodImplAttribute;
using MethodImplOptions = System.Runtime.CompilerServices.MethodImplOptions;

namespace GameUtilities
{
    /// <summary>
    /// A variety of different useful methods used throughout the code
    /// </summary>
    public struct UtilityMethods
    {
        /// <summary>
        /// Gets a vector containing only the x component of the original vector
        /// </summary>
        /// <param name="v">The original vector</param>
        /// <returns>New vector that contains only the x component of the original vector</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 XVector(Vector3 v)
        {
            return new Vector3(v.x, 0f, 0f);
        }

        /// <summary>
        /// Gets a y-axis vector with a magnitude of the value provided
        /// </summary>
        /// <param name="y">The value of the y-axis</param>
        /// <returns>A y-axis vector based on the original value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 YVector(float y)
        {
            return new Vector3(0f, y, 0f);
        }

        /// <summary>
        /// Gets a z-axis vector with a magnitude of the value provided
        /// </summary>
        /// <param name="z">The value of the z-axis</param>
        /// <returns>A z-axis vector based on the original value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ZVector(float z)
        {
            return new Vector3(0f, 0f, z);
        }

        /// <summary>
        /// Gets a vector containing only the y and z components of the original vector
        /// </summary>
        /// <param name="v">The original vector</param>
        /// <returns>New vector that contains only the y and z components of the original vector</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 YZVector(Vector3 v)
        {
            return new Vector3(0f, v.y, v.z);
        }

        /// <summary>
        /// Gets the parent of a child game object
        /// </summary>
        /// <param name="o">The child game object</param>
        /// <returns>The parent of the child game object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject Parent(GameObject o)
        {
            return o.transform.parent.gameObject;
        }

        /// <summary>
        /// Gets the distance on the x-axis between two vectors
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">Vector b</param>
        /// <returns>The distance on the x-axis between a and b</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float XDistance(Vector3 a, Vector3 b)
        {
            float dx = a.x - b.x;
            return (float)Math.Sqrt(dx * dx);
        }
    }

    namespace GameEvents
    {
        public enum EventType
        {
            Empty,
            ObstaclePassed,
            JumpBoostPickupEffect,
            BonusPickupEffect,
            Pickup3,
            Pickup4,
            BossOneSpawn,
            BossTwoSpawn,
            BossOneBeaten,
            BossTwoBeaten,
        }

        public interface IEventListener
        {
            public void OnEvent(EventType eventType, UnityEngine.Component sender, object param = null);
        }
    }
}
