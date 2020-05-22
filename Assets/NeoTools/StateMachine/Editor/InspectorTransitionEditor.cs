using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Neo.StateMachine.Wrappers;
using Neo.Utility.Extensions.Unity;

namespace Neo.Utility.Unity {
    [CustomEditor(typeof(InspectorTransition))]
    public class InspectorTransitionEditor : Editor {
        protected ReorderableList   m_NextStates;
    
        protected SerializedProperty    m_Code;
        protected SerializedProperty    m_ReadOnly;
    
        protected InspectorTransition   Self {
            get {
                return target as InspectorTransition;
            }
        }
    
        protected void OnEnable() {
            m_ReadOnly = serializedObject.FindProperty("m_ReadOnly");
            m_Code = serializedObject.FindProperty("m_Code");
            m_Code.stringValue = "";
    
            m_NextStates = ReorderableListExtensions.Create( serializedObject, "m_NextStates", "Next State(s)" );
    
            ReorderableListExtensions.OnDrawElement( m_NextStates, delegate(SerializedProperty element, Rect rect, bool isActive, bool isFocused) {
                EditorGUI.PropertyField(rect, element, GUIContent.none);
            } );
        }
    
        public override void OnInspectorGUI() {
            serializedObject.Update();
    
            if( m_ReadOnly != null ) {
                EditorGUILayout.PropertyField(m_ReadOnly);
            }
    
            if( m_Code != null ) {
                EditorGUILayout.LabelField( "Code" );
    
                string code;
    
                if( m_ReadOnly.boolValue ) {
                    //Color prevColor = GUI.color;
                    //GUI.color *= 0.7f;
    
                    EditorGUILayout.LabelField( m_Code.stringValue, GUILayout.MinHeight(100f) );
    
                    //GUI.color = prevColor;
                } else {
                    code = EditorGUILayout.TextArea( m_Code.stringValue, GUILayout.MinHeight(100f) );
    
                    //if( string.IsNullOrEmpty(code) ) {
                    //    code = "{\n    <<Behavior Declaration(s)>>\n}\n<<Transition behavior expression>>";
                    //}
                    m_Code.stringValue = code;
                }
            }
    
            if( m_NextStates != null ) {
                m_NextStates.DoLayoutList();
            }
    
            serializedObject.ApplyModifiedProperties();
        }
    }
}