using System;
using UnityEngine;
#if SCOR_ENABLE_LOCALIZATION
using UnityEngine.Localization;
#endif

namespace StudioScor.Utilities.DialogueSystem
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Dialouge/new Dialouge", fileName = "Dialouge_")]
    public class  Dialogue : BaseScriptableObject, IDialogue
    {
        [Header(" [ Dialogue ] ")]
#if SCOR_ENABLE_LOCALIZATION
        [SerializeField] private LocalizedString _dialogueText;
#else
        [SerializeField] private string _dialogueText;
#endif
        [SerializeField] private Dialogue _nextDialogue;

        public virtual string DialogueText
        {
            get
            {
#if SCOR_ENABLE_LOCALIZATION
                return _dialogueText.GetLocalizedString();
#else
                return _dialogueText;
#endif
            }
        }

        public virtual IDialogue NextDialogue => _nextDialogue;

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode());
        }

        public override bool Equals(object other)
        {
            if (base.Equals(other))
                return true;

            if (ReferenceEquals(this, other))
                return true;

            return false;
        }

        public static bool operator ==(Dialogue lhs, IDialogue rhs)
        {
            return ReferenceEquals(lhs, rhs);
        }
        public static bool operator !=(Dialogue lhs, IDialogue rhs)
        {
            return !(lhs == rhs);
        }
    }
}
