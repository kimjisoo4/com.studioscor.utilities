using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class CameraDirectionVariable : DirectionVariable
    {
        [Header(" [ Camera Direction Variable ] ")]
        private Camera _camera;

        public override IDirectionVariable Clone()
        {
            return new CameraDirectionVariable();
        }

        public override Vector3 GetValue()
        {
            if(!_camera)
            {
                _camera = Camera.main;
            }

            return _camera.transform.forward;
        }
    }

}
