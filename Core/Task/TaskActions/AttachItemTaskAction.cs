using UnityEngine;

namespace StudioScor.Utilities
{
    public class AttachItemTaskAction : TaskAction
    {
        [Header(" [ Attach Item Task Action ] ")]
        [SerializeReference]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReferenceDropdown]
#endif
        private IGameObjectVariable _attachItem;
        [SerializeReference]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReferenceDropdown]
#endif
        private ITransformVariable _attachPoint;

        public override void Setup(GameObject owner)
        {
            base.Setup(owner);

            _attachItem.Setup(owner);
        }
        public override ITaskAction Clone()
        {
            var clone = new AttachItemTaskAction();

            clone._attachItem = _attachItem.Clone();
            clone._attachPoint = _attachPoint.Clone();

            return clone;
        }
        public override void Action(GameObject target)
        {
            var attachItem = _attachItem.GetValue();

            _attachPoint.Setup(target);
            var attachPoint = _attachPoint.GetValue();

            attachItem.transform.SetParent(attachPoint.transform, false);
        }
    }
}
