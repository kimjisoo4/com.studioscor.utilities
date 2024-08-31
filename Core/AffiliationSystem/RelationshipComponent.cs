using UnityEngine;

namespace StudioScor.Utilities
{
    public class RelationshipComponent : BaseMonoBehaviour, IRelationshipSystem
    {
        [Header(" [ Affiliation Component ] ")]
        [SerializeField] private TeamData _team;

        public TeamData Team => _team;

        public event IRelationshipSystem.RelationshipTeamStateHandler OnChangedTeam;

        public void ChangeReleationshipTeam(TeamData team)
        {
            if (_team == team)
                return;

            var prevTeam = _team;
            _team = team;

            Invoke_OnChangedTeam(prevTeam);
        }

        private void Invoke_OnChangedTeam(TeamData prevTeam)
        {
            Log($"{nameof(OnChangedTeam)} - Current Team - {_team} || Prev Team - {prevTeam}");

            OnChangedTeam?.Invoke(this, _team, prevTeam);
        }
    }
}