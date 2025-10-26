using StudioScor.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StudioScor.RelationshipSystem
{
    [CreateAssetMenu(menuName ="StudioScor/Utilities/Relationship/new TeamData", fileName = "Team_")]
    public class TeamData : BaseScriptableObject
    {
        [Header(" [ Affiliation Team ] ")]
        [SerializeField] private List<TeamData> _friendlys;
        [SerializeField] private List<TeamData> _hostiles;

        private readonly List<TeamData> _runtimeFriendlys = new();
        private readonly List<TeamData> _runtimeHostiles = new();

        public IReadOnlyList<TeamData> Friendlys => _runtimeFriendlys;
        public IReadOnlyList<TeamData> Hostiles => _runtimeHostiles;


        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                return;

            foreach (var teamData in _friendlys)
            {
                if (!teamData._friendlys.Contains(this))
                {
                    teamData._friendlys.Add(this);
                }
            }

            foreach (var teamData in _hostiles)
            {
                if (!teamData._hostiles.Contains(this))
                {
                    teamData._hostiles.Add(this);
                }
            }
#endif
        }

        protected override void OnReset()
        {
            base.OnReset();

            _runtimeFriendlys.Clear();
            _runtimeHostiles.Clear();

            _runtimeFriendlys.AddRange(_friendlys);
            _runtimeHostiles.AddRange(_hostiles);
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
    }
}