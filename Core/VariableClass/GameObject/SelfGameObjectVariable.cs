using System;
using UnityEngine;

namespace StudioScor.Utilities
{
    [Serializable]
    public class SelfGameObjectVariable : GameObjectVariable
    {
        public override IGameObjectVariable Clone()
        {
            return new SelfGameObjectVariable();
        }

        public override GameObject GetValue()
        { 
            return Owner;
        }
    }
}
