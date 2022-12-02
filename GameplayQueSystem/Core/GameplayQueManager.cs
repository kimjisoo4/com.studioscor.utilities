using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using StudioScor.Utilities;
using StudioScor.GameplayTagSystem;

namespace StudioScor.GameplayQueSystem
{
    public class GameplayQueManager : Singleton<GameplayQueManager>
    {
        [SerializeField] private GameplayQue[] _Ques;

        public void PlayQue(IReadOnlyCollection<GameplayTag> ques, Vector3 position = default, Quaternion rotation = default, Vector3 scale = default)
        {
            if (scale == default)
                scale = Vector3.one;

            foreach (GameplayQue gameplayQue in _Ques)
            {
                gameplayQue.TryPlayQue(ques, position, rotation, scale);
            }
        }

        public void PlayQue(GameplayTag[] ques, Vector3 position = default, Quaternion rotation = default, Vector3 scale = default)
        {
            if (scale == default)
                scale = Vector3.one;

            foreach (GameplayQue gameplayQue in _Ques)
            {
                gameplayQue.TryPlayQue(ques, position, rotation, scale);
            }
        }
    }
}
