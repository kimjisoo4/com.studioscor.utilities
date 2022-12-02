using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.RotationSystem
{
    public class RotationSystemWithTransform : RotationSystem
    {
        protected override void UpdateRotation(float deltaTime)
        {
            Vector3 rotation = transform.eulerAngles;

            float angle = Mathf.MoveTowardsAngle(rotation.y, TurnEulerAngles.y, deltaTime * TurnSpeed);

            rotation.y = angle;

            transform.eulerAngles = rotation;
        }
    }

}
