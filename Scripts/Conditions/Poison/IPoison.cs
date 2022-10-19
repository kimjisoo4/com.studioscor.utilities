namespace KimScor.Utilities
{
    public interface IPoison
    {
        public bool TryApplyPoison(IPoisonOwner poisonOwner);
        public bool CanApplyPoison(IPoisonOwner poisonOwner);
        public void ApplyPoison(IPoisonOwner poisonOwner);
        public void UpdatePoison(float deltaTime);
        public float Damage { get; }
        public int Level { get; }
        public PoisonType Type { get; }
    }
}