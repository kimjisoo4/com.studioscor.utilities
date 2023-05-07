#if SCOR_ENABLE_VISUALSCRIPTING
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace StudioScor.Utilities.VisualScripting.Editor.Community
{
    public sealed class UnityEditorEvent : DeserializedRoutine
    {
        public static UnityEditorEvent instance { get; private set; }
        public static event Action<Event> onCurrentEvent = (e) => { };

        public override void Run()
        {
            instance = this;
            FieldInfo fieldInfo = typeof(EditorApplication).GetField("globalEventHandler", BindingFlags.Static | BindingFlags.NonPublic);
            EditorApplication.CallbackFunction callback = fieldInfo.GetValue(null) as EditorApplication.CallbackFunction;
            callback += UpdateActiveEvent;
            fieldInfo.SetValue(null, callback);
        }

        public void UpdateActiveEvent()
        {
            onCurrentEvent(Event.current);
        }
    }
}
#endif