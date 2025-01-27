namespace StudioScor.Utilities.DialogueSystem
{
    public interface IDialogue
    {
        public string DialogueText { get; }
        public IDialogue NextDialogue { get; }
    }

}
