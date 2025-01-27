namespace StudioScor.Utilities.DialogueSystem
{
    public interface IDialogueSystem
    {
        public delegate void DialogueSystemEventHandler(IDialogueSystem dialogueSystem);

        public IDialogue StartDialogue { get; }
        public IDialogue CurrentDialouge { get; }
        public bool IsPlaying { get; }

        public void PlayDialogue(IDialogue dialouge);
        public void NextDialogue(IDialogue dialouge = null);
        public void CancelDialogue();
        public void FinishDialogue();


        public event DialogueSystemEventHandler OnStartedDialogue;
        public event DialogueSystemEventHandler OnFinishedDialogue;
        public event DialogueSystemEventHandler OnCanceledDialogue;
        public event DialogueSystemEventHandler OnEndedDialogue;
    }

}
