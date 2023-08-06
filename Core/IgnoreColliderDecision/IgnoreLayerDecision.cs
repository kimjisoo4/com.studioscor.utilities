using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Ignore Collider Decision /new Layer Decision", fileName = "CollisionDecision_Layer_")]
    public class IgnoreLayerDecision : IgnoreColliderDecision
    {
        [Header(" [ Layers ] ")]
        [SerializeField] private LayerMask layers;
        [SerializeField] private bool isIgnoreLayer = false;

        public override bool Decision(Collider other)
        {
            return CheckIgnoreLayer(other);
        }

        protected bool CheckIgnoreLayer(Collider other)
        {
            if (other.gameObject.ContainLayer(layers))
                return !isIgnoreLayer;
            else
                return isIgnoreLayer;
        }
    }
}