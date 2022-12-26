namespace StudioScor.GameplayTagSystem
{
    [System.Serializable]
    public struct FGameplayTags
    {
        public GameplayTag[] Owneds;
        public GameplayTag[] Blocks;
    }

    [System.Serializable]
    public struct FConditionTags
    {
        public GameplayTag[] Requireds;
        public GameplayTag[] Obstacleds;
    }
}
