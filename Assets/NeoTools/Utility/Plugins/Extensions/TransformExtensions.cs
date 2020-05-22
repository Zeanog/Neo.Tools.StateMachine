using UnityEngine;

namespace Neo.Utility.Extensions {
    public static class TransformExtensions {
    	public static void	Reset( this Transform self ) {
    		self.localPosition = Vector3.zero;
    		self.localRotation = Quaternion.identity;
    		self.localScale = Vector3.one;
    	}

        public static T    FindInAncestors<T>( this Transform self ) where T : Component {
            T behavior = null;
    
            Transform t = self;
            while( t != null ) {
                behavior = t.GetComponent<T>();
                if( behavior != null ) {
                    return behavior;
                }
    
                t = t.parent;
            }
    
            return null;
        }

        public static string BuildFullName(this Transform self)
        {
            using(var builder = DataStructureLibrary<System.Text.StringBuilder>.Instance.CheckOut()) {
                builder.Value.Clear();
                BuildFullName(self, builder.Value);
                return builder.Value.ToString();
            }
        }

        public static void BuildFullName(this Transform self, System.Text.StringBuilder builder)
        {
            if (self.parent)
            {
                BuildFullName(self.parent, builder);
                builder.AppendFormat("/");
            }
            builder.AppendFormat("{0}", self.name);
        }

        public static void BuildFullName(this Transform self, System.Collections.Generic.List<string> names)
        {
            if (self.parent)
            {
                BuildFullName(self.parent, names);
            }
            names.Add(self.name);
        }

        public static TComponent VisitComponentInChildren<TComponent>(this Transform self, System.Func<TComponent, bool> visitor) where TComponent : Component
        {
            TComponent comp = self.GetComponent<TComponent>();
            if (comp != null)
            {
                if (visitor(comp))
                {
                    return comp;
                }
            }
            foreach (Transform child in self.transform)
            {
                comp = child.VisitComponentInChildren(visitor);
                if (comp != null)
                {
                    return comp;
                }
            }

            return null;
        }
    }
}