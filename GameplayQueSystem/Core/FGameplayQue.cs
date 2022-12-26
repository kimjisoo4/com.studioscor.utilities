using UnityEngine;
using System.Collections.Generic;
using StudioScor.GameplayTagSystem;

namespace StudioScor.GameplayQueSystem
{
    [System.Serializable]
    public struct FGameplayQue
    {
        [SerializeField] private GameplayQue[] _Ques;
        [SerializeField] private Vector3 _Offset;
        [SerializeField] private Vector3 _Rotation;
        [SerializeField] private Vector3 _Scale;

        public FGameplayQue(GameplayQue[] ques, Vector3 offset, Vector3 rotation, Vector3 scale)
        {
            _Ques = ques;
            _Offset = offset;
            _Rotation = rotation;
            _Scale = scale;
        }

        public bool HasQue()
        {
            return _Ques is not null;
        }

        public void SpawnQue(Transform transform)
        {
            if (_Ques is null)
                return;

            Vector3 position = transform.TransformPoint(_Offset);
            Quaternion rotation = transform.rotation * Quaternion.Euler(_Rotation);

            foreach (var que in _Ques)
            {
                que.PlayQue(position, rotation, _Scale);
            }
        }
        public void SpawnQue(Transform transform, Vector3 worldPosition)
        {
            if (_Ques is null)
                return;

            Vector3 position = worldPosition + transform.TransformDirection(_Offset);
            Quaternion rotation = transform.rotation * Quaternion.Euler(_Rotation);

            foreach (var que in _Ques)
            {
                que.PlayQue(position, rotation, _Scale);
            }
        }
    }

}
