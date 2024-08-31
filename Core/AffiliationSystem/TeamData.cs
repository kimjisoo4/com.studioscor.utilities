using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName ="StudioScor/Utilities/Relationship/new TeamData", fileName = "Team_")]
    public class TeamData : BaseScriptableObject, ISerializationCallbackReceiver
    {
        [Header(" [ Affiliation Team ] ")]
        [SerializeField] private string _id;
        [SerializeField] private List<TeamData> _friendlys;
        [SerializeField] private List<TeamData> _hostiles;

        private readonly List<TeamData> _runtimeFriendlys = new();
        private readonly List<TeamData> _runtimeHostiles = new();

        public string ID => _id;
        public IReadOnlyList<TeamData> Friendlys => _friendlys;
        public IReadOnlyList<TeamData> Hostiles => _hostiles;


        [ContextMenu(nameof(NameToID), false, 1000000)]
        private void NameToID()
        {
            _id = name;
        }

        public ERelationship CheckRelationship(TeamData team)
        {
            if (Friendlys.Contains(team))
            {
                Log($"{nameof(CheckRelationship)} - {nameof(ERelationship.Friendly)}");
                return ERelationship.Friendly;
            }
            else if (Hostiles.Contains(team))
            {
                Log($"{nameof(CheckRelationship)} - {nameof(ERelationship.Hostile)}");
                return ERelationship.Hostile;
            }
            else
            {
                Log($"{nameof(CheckRelationship)} - {nameof(ERelationship.Neutral)}");
                return ERelationship.Neutral;
            }
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _runtimeFriendlys.Clear();
            _runtimeFriendlys.AddRange(_friendlys);

            _runtimeHostiles.Clear();
            _runtimeHostiles.AddRange(_hostiles);
        }
    }
}