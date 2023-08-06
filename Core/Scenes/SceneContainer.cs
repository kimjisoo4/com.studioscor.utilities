using UnityEngine;
using System.Collections.Generic;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Scene/new SceneContainer", fileName = "SceneContainer_")]
    public class SceneContainer : BaseScriptableObject
    {
        [Header(" [ Scene Container ] ")]
        [SerializeField] public LoadSceneAsync[] scenes;
        public IReadOnlyCollection<LoadSceneAsync> Scenes => scenes;
    }
}
