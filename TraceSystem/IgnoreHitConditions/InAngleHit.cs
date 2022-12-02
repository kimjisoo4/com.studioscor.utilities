using UnityEngine;
using System.Collections.Generic;


namespace StudioScor.TraceSystem
{
    [CreateAssetMenu(fileName = "InAngleHit", menuName = "Trace/IgnoreHitCondition/New InAngleHit")]
    public class InAngleHit : IgnoreHitCondition
    {
        [SerializeField, Range(0f, 180f)] private float _Angle;
        public override void CheckIgnore(TraceBase owner, ref List<RaycastHit> hits)
        {
            Vector3 ownerForward = owner.Transform.forward;

            for (int i = hits.Count - 1; i >= 0; i--)
            {
                Vector3 direction = hits[i].transform.position - owner.Transform.position;
                
                float angle = Vector3.Angle(direction, ownerForward);

                if (angle > 180f)
                {
                    angle -= 360;
                }

                angle = Mathf.Abs(angle);

                if (angle > _Angle)
                {
                    hits.RemoveAt(i);
                }
            }

        }
    }
}