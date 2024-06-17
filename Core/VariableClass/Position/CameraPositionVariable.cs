using UnityEngine;
using System;

namespace StudioScor.Utilities
{
    [Serializable]
    public class CameraPositionVariable : PositionVariable
    {
        [Header(" [ Camera Position Variable ] ")]
        private Camera _camera;

        public override IPositionVariable Clone()
        {
            return new CameraPositionVariable();
        }

        public override Vector3 GetValue()
        {
            if(!_camera)
            {
                _camera = Camera.main;
            }

            return _camera.transform.position;
        }
    }
}
