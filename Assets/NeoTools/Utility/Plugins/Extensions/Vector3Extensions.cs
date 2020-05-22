using UnityEngine;

namespace Neo.Utility.Extensions {
    public static class Vector3Extensions {
        public static Vector3   Range( Vector3 min, Vector3 max ) {
            return new Vector3( Random.Range(min[0], max[0]), Random.Range(min[1], max[1]), Random.Range(min[2], max[2]) );
        }

        public static Vector3   Parse( string val ) {
            //Format: (#,#,#)
            int startIndex = val.IndexOf( '(' );
            ++startIndex;

            int endIndex = val.LastIndexOf( ')' );
            if( endIndex < 0 ) {
                endIndex = val.Length;
            }
            int length = (endIndex - startIndex);

            val = val.Substring( startIndex, length );
            string[] comps = val.Split( ',' );
            return new Vector3( float.Parse(comps[0]), float.Parse(comps[1]), float.Parse(comps[2]) );
        }
    }
}