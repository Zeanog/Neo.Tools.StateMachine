namespace Neo.Utility {
    public class Debug : UnityEngine.MonoBehaviour {
        public void     Log( string msg ) {
            UnityEngine.Debug.Log( msg );
        }

        public void     LogWarning( string msg ) {
            UnityEngine.Debug.LogWarning( msg );
        }

        public void     LogError( string msg ) {
            UnityEngine.Debug.LogError( msg );
        }

        public void     Log( UnityEngine.Object obj ) {
            UnityEngine.Debug.Log( obj );
        }

        public void     LogWarning( UnityEngine.Object obj ) {
            UnityEngine.Debug.LogWarning( obj );
        }

        public void     LogError( UnityEngine.Object obj ) {
            UnityEngine.Debug.LogError( obj );
        }
    }
}