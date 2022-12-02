using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Diagnostics;

namespace StudioScor.TraceSystem
{

    public abstract class TraceBase
    {
        #region Events
        public delegate void OnHitHandler(TraceBase trace, RaycastHit hit);
        public delegate void OnHitsHandler(TraceBase trace, List<RaycastHit> hits);
        public delegate void OnIsHitHandler(TraceBase trace, bool isHit, List<Transform> hitList = null);
        #endregion

        public abstract LayerMask LayerMask { get; }
        public abstract IReadOnlyCollection<IgnoreHitCondition> IgnoreHitConditions { get; }
        public abstract bool IsIgnoreSelf { get; }
        public abstract Transform Transform { get; }
        public abstract bool UseDebugMode { get; }
        public abstract Vector3 Offset { get; }

        [Header("Debug")]
        private bool _IsHit = false;
        private bool _IsActive = false;
        private List<Transform> _HitList = new();
        private List<Transform> _IgnoreTransforms = new();
        public bool IsActive => _IsActive;
        public bool IsHit => _IsHit;
        public IReadOnlyList<Transform> HitList => _HitList;
        public IReadOnlyList<Transform> IgnoreTransforms => _IgnoreTransforms;

        public event OnHitHandler OnFirstHit;
        public event OnHitHandler OnHit;
        public event OnHitsHandler OnHits;
        public event OnIsHitHandler OnIsHit;

        public void OnTrace()
        {
            if (IsActive)
                return;

            Log("OnTrace");
                
            EnterTrace();
        }
        public void EndTrace()
        {
            if (!IsActive)
                return;

            Log("EndTrace");

            ExitTrace();
        }

        protected virtual void EnterTrace()
        {
            _HitList.Clear();

            _IsHit = false;
            _IsActive = true;
        }
        protected virtual void ExitTrace()
        {
            OnIsHit_CallBack();

            _HitList.Clear();
            _IsHit = false;
            _IsActive = false;
        }
        
        public void SetIgnoreTransform(List<Transform> ignoreTransform)
        {
            _IgnoreTransforms = ignoreTransform.ToList();
        }

        public void UpdateTrace()
        {
            if (!IsActive)
                return;

            RaycastHit[] hits = Trace();

            bool isHit = hits.Length > 0;

            if (isHit)
            {
                List<RaycastHit> currentHits = hits.ToList();

                IgnoreHitResult(ref currentHits);

                if (IsIgnoreSelf)
                {
                    for (int i = currentHits.Count - 1; i >= 0; i--)
                    {
                        if (currentHits[i].transform == Transform || currentHits[i].transform == Transform.root)
                        {
                            currentHits.RemoveAt(i);
                        }
                    }
                }

                if (IgnoreTransforms.Count > 0)
                {
                    for (int i = currentHits.Count - 1; i >= 0; i--)
                    {
                        if (IgnoreTransforms.Contains(currentHits[i].transform))
                        {
                            currentHits.RemoveAt(i);
                        }
                    }
                }

                if (currentHits.Count > 0)
                {
                    if (!_IsHit)
                    {
                        _IsHit = true;

                        Log("First Hit - " + currentHits[0].transform.name);

                        OnFirstHit_CallBack(currentHits[0]);
                    }

                    foreach (RaycastHit hit in currentHits)
                    {
                        Log("Hit - " + hit.transform.name);

                        _HitList.Add(hit.transform);

                        OnHit_CallBack(hit);
                    }

                    OnHits_CallBack(currentHits);
                }
            }
        }

        private void IgnoreHitResult(ref List<RaycastHit> hits)
        {
            if (IgnoreHitConditions.Count == 0)
            {
                return;
            }

            foreach (IgnoreHitCondition condition in IgnoreHitConditions)
            {
                condition.CheckIgnore(this, ref hits);
            }
        }

        #region EDITOR
        [Conditional("UNITY_EDITOR")]
        public void Log(string log)
        {
            if(UseDebugMode)
                UnityEngine.Debug.Log("[Trace] " +Transform.name + " : " + log, Transform);
        }
        #endregion

        [Conditional("UNITY_EDITOR")]
        public virtual void OnDrawGizmos()
        {
        }

        public abstract RaycastHit[] Trace();

        #region Event CallBack
        protected virtual void OnIsHit_CallBack()
        {
            OnIsHit?.Invoke(this, _IsHit, _HitList);
        }
        protected virtual void OnHit_CallBack(RaycastHit hit)
        {
            OnHit?.Invoke(this, hit);
        }
        protected virtual void OnHits_CallBack(List<RaycastHit> hits)
        {
            OnHits?.Invoke(this, hits);
        }
        protected virtual void OnFirstHit_CallBack(RaycastHit hit)
        {
            OnFirstHit?.Invoke(this, hit);
        }
        #endregion
    }
}