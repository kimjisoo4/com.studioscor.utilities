namespace StudioScor.Utilities
{
    public interface IRelationshipSystem
    {
        public delegate void RelationshipTeamStateHandler(IRelationshipSystem relationshipSystem, TeamData currentTeam, TeamData prevTeam);
        public TeamData Team { get; }
        public void ChangeTeam(TeamData team);

        public event RelationshipTeamStateHandler OnChangedTeam;
    }
}