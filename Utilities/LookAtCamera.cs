using UnityEngine;

namespace StudioScor.Utilities
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _Transform;
        
        private void Awake()
        {
            _Transform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            Vector3 direction = _Transform.Direction(transform, false);

            direction.x = 0;

            Quaternion rotation = Quaternion.LookRotation(direction);

            transform.rotation = rotation;
        }
    }

}
