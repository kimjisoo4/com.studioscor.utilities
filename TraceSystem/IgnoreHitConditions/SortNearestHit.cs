using UnityEngine;
using System.Collections.Generic;


namespace StudioScor.TraceSystem
{
    [CreateAssetMenu(fileName = "SortNearestHit", menuName = "Trace/IgnoreHitCondition/New SortNearestHit")]
    public class SortNearestHit : IgnoreHitCondition
    {
        public override void CheckIgnore(TraceBase owner, ref List<RaycastHit> hits)
        {
            Vector3 ownerForward = owner.Transform.forward;

            float angle = 0f;
            bool isInsert = false;

            List<float> nearestAngles = new();
            List<RaycastHit> nearestHits = new();

            for (int i = 0; i < hits.Count; i++)
            {
                Vector3 direction = hits[i].transform.position - owner.Transform.position;

                angle = Vector3.Angle(direction, ownerForward);

                angle = Mathf.Abs(angle);

                if(i == 0)
                {
                    nearestAngles.Add(angle);
                    nearestHits.Add(hits[i]);
                }

                isInsert = false;

                for(int j = 0; j < nearestAngles.Count; j++)
                {
                    if (angle < nearestAngles[j])
                    {
                        isInsert = true;

                        nearestAngles.Insert(j, angle);
                        nearestHits.Insert(j, hits[i]);

                        break;
                    }
                }

                if(!isInsert)
                {
                    nearestAngles.Add(angle);
                    nearestHits.Add(hits[i]);
                }
            }

            hits = nearestHits;
        }
    }
}