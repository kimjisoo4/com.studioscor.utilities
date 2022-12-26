namespace StudioScor.AbilitySystem
{
    public class AbilityInputBuffer
    {
        private IAbilitySpec _AbilitySpec;
        private bool _Activate = false;
        private float _RemainTime = 0.2f;

        
        public void SetBuffer(IAbilitySpec abilitySpec, float remainTime = 0.2f)
        {
            _AbilitySpec = abilitySpec;
            _Activate = true;
            _RemainTime = remainTime;
        }

        public void ResetAbilityInputBuffer()
        {
            _AbilitySpec = null;
            _Activate = false;
            _RemainTime = 0f;
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

            _RemainTime -= deltaTime;

            if (_AbilitySpec.TryActiveAbility() || _RemainTime <= 0f)
            {
                CancelBuffer();
            }
        }
    }
}
