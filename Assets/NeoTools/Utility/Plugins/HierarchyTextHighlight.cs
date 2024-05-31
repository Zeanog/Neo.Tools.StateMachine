using UnityEngine;

namespace Neo.Utility {
    public abstract class AHierarchyDecorator : MonoBehaviour {
        [System.Serializable]
        public enum DecorateBehaviour {
            OnlySelf,
            SelfAndChildren
        }
    
        [SerializeField]
        protected DecorateBehaviour    m_Behaviour = DecorateBehaviour.SelfAndChildren;
        public DecorateBehaviour       Behaviour {
            get {
                return m_Behaviour;
            }
    
            set {
                m_Behaviour = value;
            }
        }
    
        public abstract Color        Color {
            get;
            protected set;
        }
    
        public abstract void     Draw( Rect selectionRect, GUIStyle style, float inactiveIntensity);
    }
    
    [ExecuteInEditMode]
    public class HierarchyTextHighlight : AHierarchyDecorator {
        [SerializeField]
        protected Color     m_TextColor = Color.white;
        public override Color        Color {
            get {
                return m_TextColor;
            }
    
            protected set {
                m_TextColor = value;
            }
        }
    
        public override void     Draw( Rect selectionRect, GUIStyle style, float inactiveIntensity) {
            DrawText( selectionRect, Color, inactiveIntensity, style, gameObject );
        }
    
        public static void  DrawText( Rect selectionRect, Color color, float inactiveIntensity, GUIStyle style, GameObject self ) {
            if( Event.current.type != EventType.Repaint ) {
                return;
            }

#if UNITY_EDITOR
            if( UnityEditor.Selection.activeGameObject == self ) {
                return;
            }
#endif
    
            Color   adjustedColor = color;
            if( !self.activeSelf || !self.activeInHierarchy ) {
                adjustedColor *= inactiveIntensity;
                adjustedColor.a = color.a;
            }
    
            style.normal.textColor = adjustedColor;
            style.hover.textColor = adjustedColor; 
            style.focused.textColor = adjustedColor; 
            style.active.textColor = adjustedColor;

#if UNITY_EDITOR
            UnityEditor.EditorGUI.LabelField( selectionRect, self.name, style );
#endif
        }
    }
}