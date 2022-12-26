using UnityEngine;
using System.Diagnostics;

namespace StudioScor.AbilitySystem
{
    public abstract class AbilityAction
    {
        [field: SerializeField] public bool IsAlwaysPass = false;
        public abstract AbilityActionSpec CreateSpec(IAbilitySpec abilitySpec);
    }

    public abstract class AbilityActionSpec
    {
        private readonly AbilityAction _AbilityAction;
        private readonly IAbilitySpec _AbilitySpec;
        private bool _IsPlaying;

        #region EDITOR ONLY

        [Conditional("UNITY_EDITOR")]
        protected void Log(object massage)
        {
#if UNITY_EDITOR
            if (_AbilitySpec.Ability.UseDebug)
                UnityEngine.Debug.Log(_AbilitySpec.Ability.name + " [ " + GetType().Name + " ] : " + massage, _AbilitySpec.Ability);
#endif
        }
        #endregion

        protected AbilityAction AbilityAction => _AbilityAction;
        protected IAbilitySpec AbilitySpec => _AbilitySpec;

        protected bool IsPlaying => _IsPlaying;


        protected AbilityActionSpec(AbilityAction actionBlock, IAbilitySpec abilitySpec)
        {
            _AbilityAction = actionBlock;
            _AbilitySpec = abilitySpec;

            Setup();
        }

        protected virtual void Setup()
        {

        }

        public virtual bool CanAction()
        {
            return _AbilityAction.IsAlwaysPass || CanEnterAction();
        }
        public virtual bool CanEnterAction()
        {
            return !IsPlaying;
        }

        public void ForceEnterAction()
        {
            if (_IsPlaying)
                return;

            _IsPlaying = true;

            EnterAction();
        }
        public void EndAction()
        {
            if (!_IsPlaying)
                return;

            _IsPlaying = false;

            ExitAction();
        }
        protected abstract void EnterAction();
        public bool TryAction()
        {
            if (CanEnterAction())
            {
                ForceEnterAction();

                return true;
            }

            return false;
        }
        protected virtual void ExitAction()
        {

        }

        protected virtual void UpdateAction(float normalizedTime)
        {

        }
        public void OnUpdateAction(float normalizedTime)
        {
            if (!IsPlaying)
                return;

            UpdateAction(normalizedTime);
        }
    }
}
