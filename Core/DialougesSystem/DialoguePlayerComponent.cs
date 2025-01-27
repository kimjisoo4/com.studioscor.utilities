using UnityEngine;

namespace StudioScor.Utilities.DialogueSystem
{
    public class DialoguePlayerComponent : BaseMonoBehaviour
    {
        [Header(" [ Dialogue Player Component ] ")]
        [SerializeField] private DialogueSystemComponent _dialogueSystem;
        [SerializeField] private Dialogue _dialogue;


        private void Start()
        {
            StartDialouge();
        }

        [ContextMenu(nameof(StartDialouge))]
        public void StartDialouge()
        {
            _dialogueSystem.PlayDialogue(_dialogue);
        }
    }

}
