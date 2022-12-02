namespace StudioScor.AbilitySystem
{
    public class AbilityInputBuffer
    {
        private IAbilitySpec _AbilitySpec;
        private bool _Activate = false;
        private float _Duration = 0.2f;

        public void SetBuffer(IAbilitySpec abilitySpec, float duration = 0.2f)
        {
            _AbilitySpec = abilitySpec;
            _Activate = true;
            _Duration = duration;
        }

        public void CancelBuffer()
        {
            _AbilitySpec = null;
            _Activate = false;
        }

        public void Buffer(float deltaTime)
        {
            if (!_Activate || _AbilitySpec == null)
            {
                return;
            }

            _Duration -= deltaTime;

            if (_AbilitySpec.TryActiveAbility() || _Duration <= 0f)
            {
                CancelBuffer();
            }
        }
    }
}
