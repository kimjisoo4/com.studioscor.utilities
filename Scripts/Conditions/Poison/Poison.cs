using UnityEngine;

namespace KimScor.Utilities
{
    public class Poison<T> : BaseStateClass, IPoison
    {
        [SerializeField, Min(0)] private float _Damage;
        [SerializeField, Min(0)] private float _Duration;
        [SerializeField, Min(0)] private float _Interval;
        [SerializeField] private int _Level;
        [SerializeField] private PoisonType _Type;

        private IPoisonOwner _PoisonOwner;
        private Timer _DurtaionTimer;
        private Timer _IntervalTimer;
        public float Damage => _Damage;
        public float Duration => _Duration;
        public float Interval => _Interval;
        public int Level => _Level;
        public bool HasInterval => Interval > 0f;
        public IState State => this;
        public IPoisonOwner PoisonOwner => _PoisonOwner;

        public PoisonType Type => _Type;

        public Poison(float damage, float duration, float interval, int level)
        {
            _Damage = damage;
            _Duration = duration;
            _Interval = interval;
            _Level = level;
        }

        protected override void EnterState()
        {
            if (_DurtaionTimer is null)
                _DurtaionTimer = new Timer(_Duration, true);
            else
                _DurtaionTimer.OnTimer(_Duration);

            if(HasInterval)
            {
                if (_IntervalTimer is null)
                    _IntervalTimer = new Timer(_Interval, true);
                else
                    _IntervalTimer.OnTimer(Interval);
            }
        }

        protected override void ExitState()
        {
            _DurtaionTimer.OnStopTimer();
            _IntervalTimer.OnStopTimer();

            _DurtaionTimer = null;
            _IntervalTimer = null;
        }

        protected virtual void TickState(float deltaTime)
        {
            _DurtaionTimer.UpdateTimer(deltaTime);

            if (HasInterval)
            {
                _IntervalTimer.UpdateTimer(deltaTime);

                if (_IntervalTimer.IsFinished)
                {
                    TakePoisonDamage();

                    _IntervalTimer.OnTimer(Interval);
                }
            }
        }

        protected virtual void TakePoisonDamage()
        {
            _PoisonOwner.TakePoisonDamage(this);
        }

        public bool TryApplyPoison(IPoisonOwner poisonOwner)
        {
            if(!CanApplyPoison(poisonOwner))
                return false;

            ApplyPoison(poisonOwner);

            return true;
        }

        public bool CanApplyPoison(IPoisonOwner poisonOwner)
        {
            if (!CanEnterState())
                return false;

            foreach (var poison in poisonOwner.Poisons)
            {
                if(poison.Type == Type)
                {
                    // TODO 레벨, 지속시간을 비교해서 무언가 처리하는게 옳을듯.
                    return false;
                }
            }

            return true;
        }

        public void ApplyPoison(IPoisonOwner poisonOwner)
        {
            _PoisonOwner = poisonOwner;

            OnEnterState();
        }

        public void UpdatePoison(float deltaTime)
        {
            TickState(deltaTime);
        }
    }
}