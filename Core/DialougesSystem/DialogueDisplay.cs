using System.Security.Cryptography;
using UnityEngine;

namespace StudioScor.Utilities.DialogueSystem
{
    public interface IDialogueDisplay
    {
        public delegate void DialogueDisplayEventHandler(IDialogueDisplay dialogueDisplay);
        public delegate void DialogueChangeEventHandler(IDialogueDisplay dialogueDisplay, IDialogue currentDialolgue, IDialogue prevDialogue);
        public bool IsWriting { get; }
        
        public void Init();
        public void Activate();
        public void Deactivate();
        public void SetDialogue(IDialogue dialogue);
        public void StartWriting();
        public void EndWriting();
        public void SkipWriting();

        public event DialogueChangeEventHandler OnChangedDialogue;
        public event DialogueDisplayEventHandler OnActivated;
        public event DialogueDisplayEventHandler OnFinishedBlendIn;
        public event DialogueDisplayEventHandler OnStartedBlendOut;
        public event DialogueDisplayEventHandler OnDeactivated;
        public event DialogueDisplayEventHandler OnStartedWriting;
        public event DialogueDisplayEventHandler OnSkipedWriting;
        public event DialogueDisplayEventHandler OnEndedWriting;
    }

    public abstract class DialogueDisplay : BaseMonoBehaviour, IDialogueDisplay
    {
        [Header(" [ Dialogue Display ] ")]
        [SerializeField] private GameObject _dialogueDisplayActor;

        [Header(" Unity Events ")]
        [SerializeField] private ToggleableUnityEvent _onActivated;
        [SerializeField] private ToggleableUnityEvent _onFinishedBlendIn;
        [SerializeField] private ToggleableUnityEvent _onStartedBlendOut;
        [SerializeField] private ToggleableUnityEvent _onDeactivated;
        [SerializeField] private ToggleableUnityEvent _onStartedWriting;
        [SerializeField] private ToggleableUnityEvent _onSkipedWriting;
        [SerializeField] private ToggleableUnityEvent _onEndedWriting;

        private bool _wasInit = false;
        private bool _isWriting;
        private IDialogue _dialogue;

        public IDialogue Dialogue => _dialogue;
        public bool IsWriting => _isWriting;

        public event IDialogueDisplay.DialogueDisplayEventHandler OnActivated;
        public event IDialogueDisplay.DialogueDisplayEventHandler OnFinishedBlendIn;
        public event IDialogueDisplay.DialogueDisplayEventHandler OnStartedBlendOut;
        public event IDialogueDisplay.DialogueDisplayEventHandler OnDeactivated;
        public event IDialogueDisplay.DialogueDisplayEventHandler OnStartedWriting;
        public event IDialogueDisplay.DialogueDisplayEventHandler OnSkipedWriting;
        public event IDialogueDisplay.DialogueDisplayEventHandler OnEndedWriting;
        public event IDialogueDisplay.DialogueChangeEventHandler OnChangedDialogue;


        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            if (_wasInit)
                return;

            _wasInit = true;

            OnInit();
        }

        public virtual void Activate()
        {
            Invoke_OnActivated();
            _dialogueDisplayActor.SetActive(true);
            Invoke_OnFinishedBlendIn();
        }

        public virtual void Deactivate()
        {
            Invoke_OnStartedBlendOut();
            _dialogueDisplayActor.SetActive(false);
            Invoke_OnDeactivated();
        }

        public void SetDialogue(IDialogue dialogue)
        {
            if (_dialogue == dialogue)
                return;

            var prev = Dialogue;
            _dialogue = dialogue;

            Invoke_OnChangedDialogue(prev);
        }
        public void StartWriting()
        {
            if (_isWriting)
                return;

            _isWriting = true;

            OnStartWriting();

            Invoke_OnStartedWriting();
        }
        public void EndWriting()
        {
            if (!_isWriting)
                return;

            _isWriting = false;
            _dialogue = null;

            OnEndWriting();

            Invoke_OnEndedWriting();
        }
        public void SkipWriting()
        {
            if (!_isWriting)
                return;

            _isWriting = false;

            OnSkipWriting();

            Invoke_OnSkipedWriting();


            _dialogue = null;

            OnEndWriting();

            Invoke_OnEndedWriting();
        }

        protected virtual void OnInit() { }
        protected abstract void OnStartWriting();
        protected virtual void OnEndWriting() { }
        protected virtual void OnSkipWriting() { }


        protected void Invoke_OnStartedWriting()
        {
            Log(nameof(OnStartedWriting));

            _onStartedWriting.Invoke();
            OnStartedWriting?.Invoke(this);
        }
        protected void Invoke_OnSkipedWriting()
        {
            Log(nameof(OnSkipedWriting));

            _onSkipedWriting.Invoke();
            OnSkipedWriting?.Invoke(this);
        }
        protected void Invoke_OnEndedWriting()
        {
            Log(nameof(OnEndedWriting));

            _onEndedWriting.Invoke();
            OnEndedWriting?.Invoke(this);
        }
        protected void Invoke_OnActivated()
        {
            Log(nameof(OnActivated));

            _onActivated.Invoke();
            OnActivated?.Invoke(this);
        }
        protected void Invoke_OnDeactivated()
        {
            Log(nameof(OnDeactivated));

            _onDeactivated.Invoke();
            OnDeactivated?.Invoke(this);
        }
        protected void Invoke_OnFinishedBlendIn()
        {
            Log(nameof(OnFinishedBlendIn));

            _onFinishedBlendIn.Invoke();
            OnFinishedBlendIn?.Invoke(this);
        }
        protected void Invoke_OnStartedBlendOut()
        {
            Log(nameof(OnStartedBlendOut));

            _onStartedBlendOut.Invoke();
            OnStartedBlendOut?.Invoke(this);
        }

        protected void Invoke_OnChangedDialogue(IDialogue prevDialogue)
        {
            Log($"{nameof(OnChangedDialogue)} - Current : {Dialogue} || Prev : {prevDialogue}");

            OnChangedDialogue?.Invoke(this, Dialogue, prevDialogue);
        }
    }
}
