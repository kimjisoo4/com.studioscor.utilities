using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using StudioScor.GameplayTagSystem;

namespace StudioScor.GameplayQueSystem
{
    [CreateAssetMenu(menuName ="StudioScor/GameplayQue/new GameplayQue", fileName = "Que_")]
    public class GameplayQue : ScriptableObject
    {
        [SerializeField] private QueFX[] _QueFXs;

        public void PlayQue(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            foreach (QueFX fX in _QueFXs)
            {
                fX.PlayQue(position, rotation, scale);
            }
        }
    }
}
