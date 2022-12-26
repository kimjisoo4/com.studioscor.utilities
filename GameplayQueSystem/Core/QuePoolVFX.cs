using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StudioScor.Utilities;
using StudioScor.GameplayQueSystem;

using UnityEngine.ResourceManagement;

namespace StudioScor.GameplayQueSystem
{
    [CreateAssetMenu(menuName ="StudioScor/GameplayQue/new PoolVFX", fileName = "VFX_")]
    public class QuePoolVFX : QueFX
    {
        [SerializeField] private SimplePooledObject _Pooled;
        [SerializeField] private int _StartSize = 5;

        private SimplePool _Pool;
        private Transform _Container;

        public SimplePooledObject GetItem
        {
            get
            {
                if (!_Container)
                {
                    var container = new GameObject(name);

                    _Container = container.transform;

                    _Pool = new(_Pooled, _Container, _StartSize);
                }

                return _Pool.Get();
            }
        }

        public override void PlayQue(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            var item = GetItem;

            item.transform.position = position;
            item.transform.rotation = rotation;
            item.transform.localScale = scale;

            item.gameObject.SetActive(true);
        }
    }
}
