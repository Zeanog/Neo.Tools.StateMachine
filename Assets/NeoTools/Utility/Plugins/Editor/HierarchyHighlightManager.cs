using UnityEngine;
using UnityEditor;
using Neo.Utility.Extensions;

namespace Neo.Utility {
    [InitializeOnLoad]
    public class HierarchyHighlightManager : EditorWindow {
        static HierarchyHighlightManager() {
            EditorApplication.hierarchyWindowItemOnGUI += Decorate;
        }
    
        protected static Mutable<Vector2>        m_TextOffset;
        public static Vector2   TextOffset {
            get {
                if( m_TextOffset == null ) {
                    string encodedVal = PlayerPrefs.GetString("HierarchyHighlight.Offset", "-0.51,0");
                    m_TextOffset = new Mutable<Vector2>( Vector2Extensions.Parse(encodedVal) );
                }
                return m_TextOffset.Value;
            }

            set {
                m_TextOffset.Value = value;
                if( m_TextOffset.HasChanged ) {
                    PlayerPrefs.SetString( "HierarchyHighlight.Offset", m_TextOffset.Value.ToString() );
                    EditorApplication.RepaintHierarchyWindow();
                }
            }
        }
    
        protected static GUIStyle       m_TextStyle = null;
        public static GUIStyle          TextStyle {
            get {
                if( m_TextStyle == null ) {
                    m_TextStyle = new GUIStyle();
                    m_TextStyle.fontStyle = FontStyle.Normal;
                    m_TextStyle.alignment = TextAnchor.MiddleLeft;
                }
                return m_TextStyle;
            }
        }

        public static FontStyle         TextFontStyle {
            get {
                return TextStyle.fontStyle;
            }

            set {
                if( TextStyle.fontStyle != value ) {
                    TextStyle.fontStyle = value;
                    EditorApplication.RepaintHierarchyWindow();
                }
            }
        }

        public static TextAnchor         TextAlignment {
            get {
                return TextStyle.alignment;
            }

            set {
                if( TextStyle.alignment != value ) {
                    TextStyle.alignment = value;
                    EditorApplication.RepaintHierarchyWindow();
                }
            }
        }
    
         // Add menu named "My Window" to the Window menu
        [MenuItem("Neo/HighlightManager")]
        static void Init() {
            // Get existing open window or if none, make a new one:
            HierarchyHighlightManager window = (HierarchyHighlightManager)EditorWindow.GetWindow(typeof(HierarchyHighlightManager));
            window.Show();
        }
    
        void OnGUI() {
            TextFontStyle = (FontStyle)EditorGUILayout.EnumPopup( "Font Style", TextFontStyle );
            TextAlignment = (TextAnchor)EditorGUILayout.EnumPopup( "Alignment", TextAlignment );
            TextOffset = EditorGUILayout.Vector2Field( "Highlight Text Offset", TextOffset );
        }
    
        protected static void   Decorate( int instanceId, Rect selectionRect ) {
            UnityEngine.Object obj = EditorUtility.InstanceIDToObject( instanceId );
            if( obj == null ) {
                return;
            }
    
            GameObject go = obj as GameObject;
            if( go == null ) {
                return;
            }
    
            selectionRect.x += TextOffset.x;
            selectionRect.y += TextOffset.y;
    
            //selectionRect.width *= m_Scale.x;
            //selectionRect.height *= m_Scale.y;
    
            AHierarchyDecorator comp = go.GetComponent<AHierarchyDecorator>();
            if( comp != null ) {
                comp.Draw( selectionRect, TextStyle, 0.5f );
                return;
            }
    
            // Find parent set to hi-light children
            HierarchyTextHighlight master = FindMaster<HierarchyTextHighlight>( go.transform );
            if( master != null ) {
                HierarchyTextHighlight.DrawText( selectionRect, master.Color, 0.5f, TextStyle, go );
            }
        }
    
        protected static TMaster FindMaster<TMaster>( Transform self ) where TMaster : AHierarchyDecorator {
            if( self == null ) {
                return null;
            }
    
            TMaster master;
            Transform parent = self.parent;
            while( parent != null ) {
                master = parent.GetComponent<TMaster>();
                if( master != null && master.Behaviour == HierarchyTextHighlight.DecorateBehaviour.SelfAndChildren ) {
                    return master;
                }
    
                parent = parent.parent;
            }
    
            return null;
        }
    }
}