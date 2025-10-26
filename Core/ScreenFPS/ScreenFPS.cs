using UnityEngine;

namespace StudioScor.Utilities
{
    public class ScreenFPS : BaseStateMono
    {
        [Header(" [ Screen FPS ] ")]
        [SerializeField] private bool _verticalFlip = false;

        [SerializeField] private float _high = 0f;
        [SerializeField] private float _low = 10f;

        [Header(" GUI ")]
        [SerializeField] private Vector2 _position = new Vector2(10f, 10f);
        [SerializeField] private Vector2 _size = new Vector2(100, 20);
        [SerializeField] private int _fontSize = 12;

        private float _fps;
        private Rect _rect;

        private GUIStyle _lowStyle;
        private GUIStyle _normalStyle;
        private GUIStyle _highStyle;

        private bool _prevVerticalFlip;
        private Vector2 _prevPosition;
        private Vector2 _prevSize;
        private int _prevFontSize;


        protected override void OnValidate()
        {
#if UNITY_EDITOR
            base.OnValidate();

            if(Application.isPlaying && didAwake)
            {
                if (_prevVerticalFlip != _verticalFlip || _position != _prevPosition || _size != _prevSize || _fontSize != _prevFontSize)
                {
                    _prevVerticalFlip = _verticalFlip;
                    _prevPosition = _position;
                    _prevSize = _size;
                    _prevFontSize = _fontSize;

                    UpdateGUI();
                }
            }
#endif
        }


        private void Awake()
        {
            _lowStyle = new();
            _highStyle = new();
            _normalStyle = new();
        }

        void Start()
        {
            if (!IsPlaying)
                ForceEnterState();
        }

        protected override void EnterState()
        {
            UpdateGUI();
        }

        private void UpdateGUI()
        {
            _highStyle.normal.textColor = Color.green;
            _normalStyle.normal.textColor = Color.white;
            _lowStyle.normal.textColor = Color.red;

            _rect = new Rect(_position, _size);

            _highStyle.fontSize = _fontSize;
            _normalStyle.fontSize = _fontSize;
            _lowStyle.fontSize = _fontSize;

            if (_verticalFlip)
            {
                _rect.x = Screen.width - _rect.x - _rect.width;

                _highStyle.alignment = TextAnchor.MiddleRight;
                _normalStyle.alignment = TextAnchor.MiddleRight;
                _lowStyle.alignment = TextAnchor.MiddleRight;
            }
            else
            {
                _highStyle.alignment = TextAnchor.MiddleLeft;
                _normalStyle.alignment = TextAnchor.MiddleLeft;
                _lowStyle.alignment = TextAnchor.MiddleLeft;
            }
        }

        private void Update()
        {
            _fps += (Time.deltaTime - _fps) * 0.1f;
        }

        private void OnGUI()
        {
            float targetFPS = Application.targetFrameRate;

            float fps = 1.0f / _fps;
            GUIStyle guiStyle;

            if (fps.InRange(targetFPS - _low, targetFPS + _high))
            {
                guiStyle = _normalStyle;
            }   
            else if (fps < _low)
            {
                guiStyle = _lowStyle;
            }
            else
            {
                guiStyle = _highStyle;
            }

            var sb = SUtility.GetStringBuilder();

            sb.Append("[ FPS : ");
            sb.Append(fps.ToString("N2"));
            sb.Append(" ]");

            GUI.Label(_rect, sb.ToString(), guiStyle);
        }
    }
}
