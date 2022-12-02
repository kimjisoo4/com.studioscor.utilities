using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StudioScor.GameplayQueSystem
{
    public abstract class QueFX : ScriptableObject
    {
        public abstract void PlayQue(Vector3 position, Quaternion rotation, Vector3 scale);
    }
}
