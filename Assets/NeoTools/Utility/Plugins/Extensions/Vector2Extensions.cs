using UnityEngine;

namespace Neo.Utility.Extensions {
    public static class Vector2Extensions {
        public static Vector2   Range( Vector2 min, Vector2 max ) {
            return new Vector2( Random.Range(min[0], max[0]), Random.Range(min[1], max[1]) );
        }

        public static Vector2   Parse( string val ) {
            //Format: (#,#)
            int startIndex = val.IndexOf( '(' );
            ++startIndex;

            int endIndex = val.LastIndexOf( ')' );
            if( endIndex < 0 ) {
                endIndex = val.Length;
            }
            int length = (endIndex - startIndex);

            val = val.Substring( startIndex, length );
            string[] comps = val.Split( ',' );
            return new Vector2( float.Parse(comps[0]), float.Parse(comps[1]) );
        }
    }
}