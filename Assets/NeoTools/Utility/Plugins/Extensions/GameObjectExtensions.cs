using UnityEngine;
using Neo.Utility;
using Neo.Utility.Extensions;
#if UNITY_EDITOR
using UnityEditor;
#endif

//namespace Neo.Utility.Extensions {
public static class GameObjectExtensions {
    public static string BuildFullName( this GameObject self ) {
        using(var builder = DataStructureLibrary<System.Text.StringBuilder>.Instance.CheckOut()) {
            builder.Value.Clear();
            BuildFullName(self, builder.Value);
            return builder.Value.ToString();
        }
    }

    public static void BuildFullName( this GameObject self, System.Text.StringBuilder builder ) {
        self.transform.BuildFullName(builder);
    }

    public static void BuildFullName( this GameObject self, System.Collections.Generic.List<string> names ) {
        self.transform.BuildFullName(names);
    }

    public static TComp EnsureComponent<TComp>( this GameObject self ) where TComp : Component {
        TComp comp = self.GetComponent<TComp>();
        if(comp == null) {
            comp = self.AddComponent<TComp>();
        }
        return comp;
    }

    public static Component EnsureComponent( this GameObject self, System.Type type ) {
        Component comp = self.GetComponent(type);
        if(comp == null) {
            comp = self.AddComponent(type);
        }
        return comp;
    }

    public static Component EnsureComponent( this GameObject self, string typeName ) {
        return EnsureComponent(self, System.Type.GetType(typeName));
    }

#if UNITY_EDITOR
    public static void RemoveMissingComponents( this GameObject self ) {
        int numRemoved = 0;
        SerializedObject so;
        SerializedProperty sp;

        Transform t = self.transform;
        so = new SerializedObject(t.gameObject);
        sp = so.FindProperty("m_Component");

        Component[] cs = t.gameObject.GetComponents<Component>();
        for(int iy = cs.Length - 1; iy >= 0; --iy) {
            Component c = cs[iy];
            if(c == null) {
                sp.DeleteArrayElementAtIndex(iy);
                ++numRemoved;
            }
        }

        so.ApplyModifiedProperties();
    }

    public static void RemoveMissingComponentsInChildren( this GameObject self ) {
        self.RemoveMissingComponents();
        foreach(Transform child in self.transform) {
            child.gameObject.RemoveMissingComponentsInChildren();
        }
    }
#endif
}
//}