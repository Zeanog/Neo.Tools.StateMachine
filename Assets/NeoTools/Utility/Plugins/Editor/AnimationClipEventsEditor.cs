using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AnimationClipEvents))]
public class AnimationClipEventsEditor : Editor
{
    protected SerializedProperty controller;
    protected SerializedProperty clips;
    protected AnimationClipEvents self;

    protected void OnEnable()
    {
        controller = serializedObject.FindProperty("controller");
        clips = serializedObject.FindProperty("clips");

        self = (target as AnimationClipEvents);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //if (controller != null)
        //{
        //    EditorGUILayout.PropertyField(controller);
        //}

        if (clips != null)
        {

            if (clips.arraySize <= 0 && GUILayout.Button("Repopulate"))
            {
                self.Rebuild();
                EditorUtility.SetDirty(self.gameObject);
            }

            if (clips.arraySize > 0)
            {
                if (GUILayout.Button("Clear"))
                {
                    self.Clear();
                    EditorUtility.SetDirty(self.gameObject);
                }

                EditorGUILayout.PropertyField(clips);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}