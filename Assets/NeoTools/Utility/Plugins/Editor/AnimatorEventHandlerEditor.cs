using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditorInternal;
using Neo.Utility.Extensions.Unity;
using NUnit.Framework;
using Unity.Android.Gradle;

[CustomEditor(typeof(AnimatorToStateEventHandler), true)]
public class AnimatorToStateEventHandlerEditor : Editor {
    protected SerializedProperty            eventMapList;
    protected AnimatorToStateEventHandler   self;

    protected void OnEnable()
    {
        eventMapList = serializedObject.FindProperty("eventMap");

        self = (target as AnimatorToStateEventHandler);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (eventMapList != null)
        {
            EditorGUILayout.PropertyField(eventMapList);
        }

        serializedObject.ApplyModifiedProperties();
    }
}