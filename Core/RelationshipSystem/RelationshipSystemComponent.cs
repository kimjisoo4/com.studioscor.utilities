using StudioScor.Utilities;
using UnityEngine;

namespace StudioScor.RelationshipSystem
{
    public class RelationshipSystemComponent : BaseMonoBehaviour, IRelationshipSystem
    {
        [Header(" [ Relationship Component ] ")]
        [SerializeField] private TeamData _defaultTeam;

        public TeamData DefaultTeam => _defaultTeam;
        public TeamData Team { get; private set; }


        public event IRelationshipSystem.RelationshipTeamStateHandler OnChangedTeam;

        public void ResetTeam()
        {
            ChangeTeam(_defaultTeam);
        }
        public void ChangeTeam(TeamData team)
        {
            if (Team == team)
                return;

            var prevTeam = Team;
            Team = team;

            RaiseOnChangedTeam(prevTeam);
        }

        public ERelationship CheckRelationShip(TeamData teamData)
        {
            return Team ? Team.CheckRelationship(teamData) : ERelationship.Neutral;
        }

        private void RaiseOnChangedTeam(TeamData prevTeam)
        {
            Log($"{nameof(OnChangedTeam)} - Current Team - {Team} || Prev Team - {prevTeam}");

            OnChangedTeam?.Invoke(this, Team, prevTeam);
        }
    }
}