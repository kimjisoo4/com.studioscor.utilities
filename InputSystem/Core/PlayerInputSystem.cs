using UnityEngine;
using System.Collections.Generic;

using UnityEngine.InputSystem;


namespace StudioScor.InputSystem
{
    public class PlayerInputSystem : MonoBehaviour
    {
        [SerializeField] private PlayerInput _PlayerInput;
        [SerializeField] private List<InputButton> _InputButtons;

        public PlayerInput PlayerInput => _PlayerInput;
        public IReadOnlyList<InputButton> InputButtons;


#if UNITY_EDITOR
        private void Reset()
        {
            TryGetComponent(out _PlayerInput);
        }
#endif

        private void Awake()
        {
            foreach (var input in _InputButtons)
            {
                input.Setup(_PlayerInput);
            }
        }
    }

}