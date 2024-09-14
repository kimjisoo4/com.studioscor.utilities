namespace StudioScor.Utilities
{
    public static partial class SUtility
    {
        public static bool CoinToss()
        {
            return RandomBool();
        }
        public static int DiceRoll()
        {
            return UnityEngine.Random.Range(1, 7);
        }
        public static bool RandomBool(float chance = 0.5f)
        {
            return UnityEngine.Random.value < chance;
        }
    }
}
