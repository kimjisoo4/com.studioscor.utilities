using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.Utilities.DialogueSystem
{
    public class DialogueSystemComponent : BaseMonoBehaviour, IDialogueSystem
    {
        [Header(" [ Dialouge System Component ] ")]
        [Header(" Unity Events ")]
        [SerializeField] private ToggleableUnityEvent _onStartedDialogue;
        [SerializeField] private ToggleableUnityEvent _onCanceledDialogue;
        [SerializeField] private ToggleableUnityEvent _onFinishedDialogue;
        [SerializeField] private ToggleableUnityEvent _onEndedDialogue;

        private IDialogue _startDialogue;
        private IDialogue _currentDialouge;
        private readonly Queue<IDialogue> _nextDialogues = new();

        private bool _isEnded = false;

        public bool IsPlaying => _currentDialouge is not null;
        public IDialogue StartDialogue => _startDialogue;
        public IDialogue CurrentDialouge => _currentDialouge;

        public event IDialogueSystem.DialogueSystemEventHandler OnStartedDialogue;
        public event IDialogueSystem.DialogueSystemEventHandler OnFinishedDialogue;
        public event IDialogueSystem.DialogueSystemEventHandler OnCanceledDialogue;
        public event IDialogueSystem.DialogueSystemEventHandler OnEndedDialogue;

        public void PlayDialogue(IDialogue dialogue)
        {
            if (_isEnded)
            {
                _nextDialogues.Enqueue(dialogue);
            }
            else
            {
                if (_startDialogue is null)
                {
                    _startDialogue = dialogue;
                }

                _currentDialouge = dialogue;

                Invoke_OnStartedDiaogue();
            }
        }

        public void NextDialogue(IDialogue dialogue = null)
        {
            if(dialogue is not null)
            {
                PlayDialogue(dialogue);
            }
            else if (_currentDialouge is not null)
            {
                if (_currentDialouge.NextDialogue is not null)
                {
                    PlayDialogue(_currentDialouge.NextDialogue);
                }
                else
                {
                    FinishDialogue();
                }
            }
            else
            {
                return;
            }
        }

        public void CancelDialogue()
        {
            _isEnded = true;

            Invoke_OnCanceledDialogue();

            EndDialogue();
        }

        public void FinishDialogue()
        {
            _isEnded = true;

            Invoke_OnFinishedDiaogue();

            EndDialogue();
        }

        private void EndDialogue()
        {
            Invoke_OnEndedDiaogue();

            _startDialogue = null;
            _currentDialouge = null;

            _isEnded = false;

            if(_nextDialogues.Count > 0)
            {
                PlayDialogue(_nextDialogues.Dequeue());
            }
        }

        private void Invoke_OnStartedDiaogue()
        {
            Log($"{nameof(OnStartedDialogue)}");

            _onStartedDialogue?.Invoke();
            OnStartedDialogue?.Invoke(this);
        }
        private void Invoke_OnCanceledDialogue()
        {
            Log($"{nameof(OnCanceledDialogue)}");

            _onCanceledDialogue?.Invoke();
            OnCanceledDialogue?.Invoke(this);
        }
        private void Invoke_OnFinishedDiaogue()
        {
            Log($"{nameof(OnFinishedDialogue)}");

            _onFinishedDialogue?.Invoke();
            OnFinishedDialogue?.Invoke(this);
        }
        private void Invoke_OnEndedDiaogue()
        {
            Log($"{nameof(OnEndedDialogue)}");

            _onEndedDialogue?.Invoke();
            OnEndedDialogue?.Invoke(this);
        }

        
    }

}
