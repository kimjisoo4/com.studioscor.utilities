using UnityEngine;
using System.Collections.Generic;


namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Scene/new SceneContainer", fileName = "SceneContainer_")]
    public class SceneContainer : BaseScriptableObject
    {
        [SerializeField] public LoadSceneAsync[] _Scenes;
        public IReadOnlyCollection<LoadSceneAsync> Scenes => _Scenes;
    }
}
