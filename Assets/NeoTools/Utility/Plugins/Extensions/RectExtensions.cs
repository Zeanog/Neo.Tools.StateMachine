using UnityEngine;

namespace Neo.Utility.Extensions {
    public static class RectExtensions {
        public static Vector2   RandomPoint( this Rect self ) {
            return new Vector2( UnityEngine.Random.Range(self.xMin, self.xMax), UnityEngine.Random.Range(self.yMin, self.yMax) );
        }
    }
}