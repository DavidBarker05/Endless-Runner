namespace GameUtilities
{
    namespace UtilityMethods // A variety of different useful methods used throughout the code.
    {
        public struct UtilityMethods
        {
            /// <summary>
            /// Gets a y-axis <see cref="UnityEngine.Vector3"/> with a magnitude of the value provided.
            /// </summary>
            /// <param name="y">The value of the y-axis.</param>
            /// <returns>A y-axis <see cref="UnityEngine.Vector3"/> based on the original value.</returns>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static UnityEngine.Vector3 YVector(float y)
            {
                return new UnityEngine.Vector3(0f, y, 0f);
            }

            /// <summary>
            /// Gets a z-axis <see cref="UnityEngine.Vector3"/> with a magnitude of the value provided.
            /// </summary>
            /// <param name="z">The value of the z-axis.</param>
            /// <returns>A z-axis <see cref="UnityEngine.Vector3"/> based on the original value.</returns>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static UnityEngine.Vector3 ZVector(float z)
            {
                return new UnityEngine.Vector3(0f, 0f, z);
            }

            /// <summary>
            /// Gets the parent of a child <see cref="UnityEngine.GameObject"/>.
            /// </summary>
            /// <param name="o">The child <see cref="UnityEngine.GameObject"/>.</param>
            /// <returns>The parent of the child <see cref="UnityEngine.GameObject"/>.</returns>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static UnityEngine.GameObject Parent(UnityEngine.GameObject o)
            {
                return o.transform.parent.gameObject;
            }
        }
    }

    namespace GameEvents
    {
        /// <summary>
        /// An enum containing the different types of events that can be invoked during the game by <see cref="GameManager.InvokeEvent(EventType, UnityEngine.Component, object)"/>.
        /// </summary>
        public enum EventType
        {
            /// <summary>
            /// <para>
            /// Empty event type that's used for exception handling or edge cases.
            /// </para>
            /// <para>
            /// Adding or removing listeners and invoking this event does nothing.
            /// </para>
            /// </summary>
            Empty,
            /// <summary>
            /// The event to be invoked after passing an obstacle.
            /// </summary>
            ObstaclePassed,
            /// <summary>
            /// <para>
            /// The event to be invoked every fixed update after a <see cref="JumpBoostPickup"/> has been collected.
            /// </para>
            /// <para>
            /// Gives the jump boost effect.
            /// </para>
            /// </summary>
            JumpBoostPickupEffect,
            /// <summary>
            /// <para>
            /// The event to be invoked after a <see cref="BonusPickup"/> has been collected.
            /// </para>
            /// <para>
            /// Effect gives bonus score.
            /// </para>
            /// </summary>
            BonusPickupEffect,
            /// <summary>
            /// <para>
            /// The event to be invoked every fixed update after a <see cref="InvulnerabilityPickup"/> has been collected.
            /// </para>
            /// <para>
            /// Gives the invulnerability effect.
            /// </para>
            /// </summary>
            InvulnerabilityPickupEffect,
            /// <summary>
            /// <para>
            /// The event to be invoked every fixed update after a <see cref="BossOnePickup"/> has been collected.
            /// </para>
            /// <para>
            /// Effect sets the <see cref="LevelOneBoss"/> back by a certain amount every fixed update.
            /// </para>
            /// </summary>
            BossOnePickupEffect,
            /// <summary>
            /// The event to be invoked when the first boss spawns.
            /// </summary>
            BossOneSpawn,
            /// <summary>
            /// The event to be invoked when the second boss spawns.
            /// </summary>
            BossTwoSpawn,
            /// <summary>
            /// The event to be invoked when the first boss is beaten.
            /// </summary>
            BossOneBeaten,
            /// <summary>
            /// The event to be invoked when the second boss is beaten.
            /// </summary>
            BossTwoBeaten,
        }

        /// <summary>
        /// Interface for listeners to be added to game events.
        /// </summary>
        public interface IEventListener
        {
            /// <summary>
            /// The method of the listener to be called when the <see cref="EventType"/> it listens to is invoked by <see cref="GameManager.InvokeEvent(EventType, UnityEngine.Component, object)"/>.
            /// </summary>
            /// <param name="eventType">The <see cref="EventType"/> being invoked.</param>
            /// <param name="sender">The <see cref="UnityEngine.Component"/> that invoked the event.</param>
            /// <param name="param">
            /// <para>
            /// A parameter that is passed when the event is invoked.
            /// </para>
            /// <para>
            /// The parameter type and use varies for each event.
            /// </para>
            /// </param>
            public void OnEvent(EventType eventType, UnityEngine.Component sender, object param = null);
        }
    }
}
