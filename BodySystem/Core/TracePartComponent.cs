using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace StudioScor.BodySystem
{
    public class TracePartComponent : BodyPartComponent
    {
        [SerializeField] private float _Radius;
        private LayerMask _LayerMask;

        [SerializeField] private List<BodyPoint> _BodyPoints;


#if UNITY_EDITOR

        [ContextMenu("New Create Point")]
        public void CreatePoint()
        {
            var newPoint = new GameObject();

            newPoint.transform.SetParent(this.transform);

            if (_BodyPoints.Count > 0)
            {
                newPoint.transform.position = _BodyPoints.Last().Transform.position;
            }
            else
            {
                newPoint.transform.position = transform.position;
            }

            BodyPoint newbodyPoint = new BodyPoint(newPoint.transform);

            _BodyPoints.Add(newbodyPoint);

            newPoint.name = "Point [" + (_BodyPoints.Count - 1) + "]";

        }
        private void OnDrawGizmos()
        {
            if (_BodyPoints is null)
                return;

            if (UseDebug)
            {
                Gizmos.color = Color.red;

                foreach (BodyPoint bodyPoint in _BodyPoints)
                {
                    if (bodyPoint.Transform)
                        Gizmos.DrawWireSphere(bodyPoint.Transform.position, _Radius);
                }
            }
        }
        private void OnDrawGizmosSelected()
        {
            if (_BodyPoints is null)
                return;

            Gizmos.color = Color.red;

            foreach (BodyPoint bodyPoint in _BodyPoints)
            {
                if (bodyPoint.Transform)
                    Gizmos.DrawWireSphere(bodyPoint.Transform.position, _Radius);
            }
        }
#endif


        public void OnTrace(LayerMask layerMask)
        {
            _LayerMask = layerMask;

            foreach (BodyPoint bodyPoint in _BodyPoints)
            {
                bodyPoint.PrevPosition = bodyPoint.Transform.position;
            }

        }
        public void OffTrace()
        {

        }

        public bool UpdateTrace(out List<RaycastHit> hits)
        {
            hits = new List<RaycastHit>();

            foreach (BodyPoint bodyPoint in _BodyPoints)
            {
                Vector3 position = bodyPoint.Transform.position;
                Vector3 prevPosition = bodyPoint.PrevPosition;

                if (UseDebug)
                {
                    Debug.DrawRay(prevPosition, position - prevPosition, Color.red, 1f);
                }


                hits.AddRange(Physics.SphereCastAll(prevPosition, _Radius, position - prevPosition, 0.01f, _LayerMask).ToList());

                bodyPoint.PrevPosition = position;
            }


            return hits.Count > 0;
        }
    }
}
