/// <summary>
/// Interface for pickups
/// </summary>
public interface IPickup
{
    /// <summary>
    /// Name of pickup
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Duration of pickup
    /// </summary>
    public float Duration { get; }
    /// <summary>
    /// The current use time of the pickup
    /// </summary>
    public float UseTime { get; set; }

    /// <summary>
    /// What the pickup does
    /// </summary>
    public void Effect();
}
