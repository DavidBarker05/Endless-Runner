/// <summary>
/// Interface for pickups
/// </summary>
public interface IPickup
{
    /// <summary>
    /// Pickup managager
    /// </summary>
    public PickupManager PickupManager { get; set; }
    /// <summary>
    /// Player manager
    /// </summary>
    public PlayerManager PlayerManager { get; set; }
    /// <summary>
    /// Name of pickup
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Duration of pickup
    /// </summary>
    public float Duration { get; }

    /// <summary>
    /// What the pickup does
    /// </summary>
    /// <param name="useTime">The current use time of the pickup</param>
    public delegate void Effect(float useTime);
}
