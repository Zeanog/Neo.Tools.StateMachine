using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Neo.Utility.Extensions.Unity {
    public static class ReorderableListExtensions {
        public delegate void ElementCallbackDelegate( SerializedProperty element, Rect rect, bool isActive, bool isFocused );
    
        public static ReorderableList  Create( SerializedObject owner, string listName, string displayName ) {
            ReorderableList list = new ReorderableList( owner, owner.FindProperty(listName), true, !string.IsNullOrEmpty(displayName), true, true );
    
            list.drawHeaderCallback = delegate( Rect rect ) {
                EditorGUI.LabelField( new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), displayName );
            };
    
            return list;
        }
    
        public static void  OnDrawElement( ReorderableList list, ReorderableListExtensions.ElementCallbackDelegate handler ) {
            list.drawElementCallback = delegate( Rect rect, int index, bool isActive, bool isFocused ) {
                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;
    
                handler( element, rect, isActive, isFocused );
            };
        }
    }
}