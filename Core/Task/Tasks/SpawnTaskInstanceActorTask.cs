using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class SpawnTaskInstanceActorTask : Task, ISubTask
    {
        [Header(" [ Spawn Projectile Task ] ")]
        [SerializeField] private SimplePoolContainer _spawnActor;
        [SerializeField][Range(0f, 1f)] private float _instanceSpawnTime = 0.2f;

#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private IPositionVariable _position = new LocalPositionVariable();

#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private IRotationVariable _rotation = new LocalRotationVariable();

        private bool _wasSpawn;
        private float _spawnTime;
        private SpawnTaskInstanceActorTask _original;

        protected override void SetupTask()
        {
            base.SetupTask();

            _position.Setup(Owner);
            _rotation.Setup(Owner);
        }
        public override ITask Clone()
        {
            var clone = new SpawnTaskInstanceActorTask();

            clone._original = this;
            clone._position = _position.Clone();
            clone._rotation = _rotation.Clone();

            return clone;
        }

        protected override void EnterTask()
        {
            base.EnterTask();

            _wasSpawn = false;

            _spawnTime = _original is null ? _instanceSpawnTime : _original._instanceSpawnTime;
        }
        public void FixedUpdateSubTask(float deltaTime, float normalizedTime)
        {
            return;
        }

        public void UpdateSubTask(float deltaTime, float normalizedTime)
        {
            if (_wasSpawn)
                return;

            if (normalizedTime > _spawnTime)
            {
                _wasSpawn = true;

                SpawnTaskInstanceActor();
            }
        }

        private void SpawnTaskInstanceActor()
        {
            var projectilePool = _original is null ? _spawnActor : _original._spawnActor;

            var projectile = projectilePool.Get();
            Vector3 spawnPosition = _position.GetValue();
            Quaternion spawnRotation = _rotation.GetValue();

            projectile.SetPositionAndRotation(spawnPosition, spawnRotation);

            var slash = projectile.GetComponent<ITaskInstanceActor>();

            slash.SetOwner(Owner);

            slash.Activate();
        }
    }
}