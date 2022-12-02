using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.StatusSystem
{
    [System.Serializable]
    public struct FInitializationStatus
    {
        public string StatusName;
        public StatusTag StatusTag;
        public float MinValue;
        public float MaxValue;
    }

    [CreateAssetMenu(fileName = "new Status", menuName = "Status/New Status")]
    public class StatusTag : ScriptableObject
    {
        [Header("[Name]")]
        [SerializeField] private string _StatusName;
        [Header("[Text]")]
        [SerializeField] private string _Description;

        public string StatusName => _StatusName;
        public string Description => _Description;
    }
}