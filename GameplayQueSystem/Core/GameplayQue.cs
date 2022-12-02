using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using StudioScor.GameplayTagSystem;

namespace StudioScor.GameplayQueSystem
{
    [CreateAssetMenu(menuName ="GameplayQue/new GameplayQue", fileName = "Que_")]
    public class GameplayQue : ScriptableObject
    {
        [SerializeField] private GameplayTag _Que;
        [SerializeField] private QueFX[] _QueFXs;

        public bool TryPlayQue(GameplayTag[] ques, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            if (ques.Contains(_Que))
            {
                PlayQue(position, rotation, scale);

                return true;

            }
            else
            {
                return false;
            }
        }
        public bool TryPlayQue(IReadOnlyCollection<GameplayTag> ques, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            if (ques.Contains(_Que))
            {
                PlayQue(position, rotation, scale);

                return true;

            }
            else
            {
                return false;
            }
        }

        public void PlayQue(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            foreach (QueFX fX in _QueFXs)
            {
                fX.PlayQue(position, rotation, scale);
            }
        }
    }
}
