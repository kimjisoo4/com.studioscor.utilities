using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.Utilities
{
    public class ScreenFPS : BaseStateMono
    {
        [Header(" [ Screen FPS ] ")]
        [SerializeField][SReadOnlyWhenPlaying] private bool _UseRight = false;

        [SerializeField] private float _High = 50f;
        [SerializeField] private float _Low = 30f;
        
        private float _FPS;
        private Rect _Rect;

        private GUIStyle _LowStyle;
        private GUIStyle _HighStyle;
        private GUIStyle _NormalStyle;


        private void Awake()
        {
            _LowStyle = new();
            _HighStyle = new();
            _NormalStyle = new();
        }

        void Start()
        {
            if (!IsPlaying)
                ForceEnterState();
        }

        protected override void EnterState()
        {
            _HighStyle.normal.textColor = Color.green;
            _NormalStyle.normal.textColor = Color.white;
            _LowStyle.normal.textColor = Color.red;

            _Rect = new Rect(10, 10, 100, 20);

            if (_UseRight)
            {
                _Rect.x = Screen.width - _Rect.x - _Rect.width;

                _HighStyle.alignment = TextAnchor.MiddleRight;
                _NormalStyle.alignment = TextAnchor.MiddleRight;
                _LowStyle.alignment = TextAnchor.MiddleRight;
            }
            else
            {
                _HighStyle.alignment = TextAnchor.MiddleLeft;
                _NormalStyle.alignment = TextAnchor.MiddleLeft;
                _LowStyle.alignment = TextAnchor.MiddleLeft;
            }
        }

        private void Update()
        {
            _FPS += (Time.deltaTime - _FPS) * 0.1f;
        }

        private void OnGUI()
        {
            float fps = 1.0f / _FPS;
            GUIStyle guiStyle;

            if (fps.InRange(_Low, _High))
            {
                guiStyle = _NormalStyle;
            }
            else if (fps < _Low)
            {
                guiStyle = _LowStyle;
            }
            else
            {
                guiStyle = _HighStyle;
            }

            GUI.Label(_Rect, $"[ FPS : {fps:N2} ]", guiStyle);
        }
    }
}
