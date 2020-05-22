//using UnityEngine;
using UnityEditor;
using Neo.Utility.Unity;

namespace Neo.Utility.Unity {
    [CustomEditor(typeof(HierarchyTextHighlight))]
    public class HierarchyTextHighlightEditor : Editor {
        protected SerializedProperty    m_Color;
        //protected SerializedProperty    m_Offset;
    
        protected void  Awake() {
            m_Color = serializedObject.FindProperty( "m_TextColor" );
            //m_Offset = serializedObject.FindProperty( "m_Offset" );
        }
    
        protected virtual void OnEnable() {
            
        }
    
        public override void OnInspectorGUI() {
            serializedObject.Update();
    
            //if( m_Offset != null ) {
            //    EditorGUILayout.PropertyField( m_Offset );
            //}

            if (m_Color != null)
            {
                EditorGUILayout.PropertyField(m_Color);
            }

            HierarchyTextHighlight text = target as HierarchyTextHighlight;
            text.Behaviour = (HierarchyTextHighlight.DecorateBehaviour)EditorGUILayout.EnumPopup( "Color Control Behaviour", text.Behaviour );
    
            serializedObject.ApplyModifiedProperties();
    	}
    }
}