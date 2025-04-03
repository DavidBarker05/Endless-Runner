public interface IPickup
{
    public PickupManager PickupManager { get; set; }
    public PlayerManager PlayerManager { get; set; }
    public string Name { get; }
    public float Duration { get; }

    public delegate void Effect(float useTime);
}
