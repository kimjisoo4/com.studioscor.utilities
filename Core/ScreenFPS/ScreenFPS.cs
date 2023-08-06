using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioScor.Utilities
{
    public class ScreenFPS : BaseStateMono
    {
        [Header(" [ Screen FPS ] ")]
        [SerializeField][SReadOnlyWhenPlaying] private bool useRight = false;

        [SerializeField] private float high = 50f;
        [SerializeField] private float low = 30f;
        
        private float fps;
        private Rect rect;

        private GUIStyle lowStyle;
        private GUIStyle normalStyle;
        private GUIStyle highStyle;


        private void Awake()
        {
            lowStyle = new();
            highStyle = new();
            normalStyle = new();
        }

        void Start()
        {
            if (!IsPlaying)
                ForceEnterState();
        }

        protected override void EnterState()
        {
            highStyle.normal.textColor = Color.green;
            normalStyle.normal.textColor = Color.white;
            lowStyle.normal.textColor = Color.red;

            rect = new Rect(10, 10, 100, 20);

            if (useRight)
            {
                rect.x = Screen.width - rect.x - rect.width;

                highStyle.alignment = TextAnchor.MiddleRight;
                normalStyle.alignment = TextAnchor.MiddleRight;
                lowStyle.alignment = TextAnchor.MiddleRight;
            }
            else
            {
                highStyle.alignment = TextAnchor.MiddleLeft;
                normalStyle.alignment = TextAnchor.MiddleLeft;
                lowStyle.alignment = TextAnchor.MiddleLeft;
            }
        }

        private void Update()
        {
            fps += (Time.deltaTime - fps) * 0.1f;
        }

        private void OnGUI()
        {
            float fps = 1.0f / this.fps;
            GUIStyle guiStyle;

            if (fps.InRange(low, high))
            {
                guiStyle = normalStyle;
            }
            else if (fps < low)
            {
                guiStyle = lowStyle;
            }
            else
            {
                guiStyle = highStyle;
            }

            GUI.Label(rect, $"[ FPS : {fps:N2} ]", guiStyle);
        }
    }
}
