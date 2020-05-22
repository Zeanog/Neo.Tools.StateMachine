using UnityEngine;
using UnityEngine.Events;

namespace Neo.Utility {
    public class SystemEventDispatcher : MonoBehaviour {
        [System.Serializable]
        public class OnTrigger2DUnityEvent : UnityEvent<Collider2D> {}
        [System.Serializable]
        public class OnCollisionUnityEvent : UnityEvent<Collision> {}
        [System.Serializable]
        public class OnTriggerUnityEvent : UnityEvent<Collider> {}
    
        [SerializeField]
        protected UnityEvent        m_OnAwake;
        protected void              Awake() {
            m_OnAwake.Invoke();
        }
    
        [SerializeField]
        protected UnityEvent        m_OnStart;
        protected void              Start() {
            m_OnStart.Invoke();
        }
    
        [SerializeField]
        protected UnityEvent        m_OnEnable;
        protected void              OnEnable() {
            m_OnEnable.Invoke();
        }
    
        [SerializeField]
        protected UnityEvent        m_OnDisable;
        protected void              OnDisable() {
            m_OnDisable.Invoke();
        }
    
        [SerializeField]
        protected OnTrigger2DUnityEvent m_OnTriggerEnter2D;
        protected void OnTriggerEnter2D( Collider2D collision ) {
            m_OnTriggerEnter2D.Invoke( collision );
        }
    
        [SerializeField]
        protected OnTrigger2DUnityEvent m_OnTriggerStay2D;
        protected void OnTriggerStay2D( Collider2D collision ) {
            m_OnTriggerStay2D.Invoke( collision );
        }
    
        [SerializeField]
        protected OnTrigger2DUnityEvent m_OnTriggerExit2D;
        protected void OnTriggerExit2D( Collider2D collision ) {
            m_OnTriggerExit2D.Invoke( collision );
        }
    
        [SerializeField]
        protected OnTriggerUnityEvent m_OnTriggerEnter;
        protected void OnTriggerEnter( Collider other ) {
            m_OnTriggerEnter.Invoke( other );
        }
    
        [SerializeField]
        protected OnTriggerUnityEvent m_OnTriggerStay;
        protected void OnTriggerStay( Collider other ) {
            m_OnTriggerStay.Invoke( other );
        }
    
        [SerializeField]
        protected OnTriggerUnityEvent m_OnTriggerExit;
        protected void OnTriggerExit( Collider other ) {
            m_OnTriggerExit.Invoke( other );
        }
    
        [SerializeField]
        protected OnCollisionUnityEvent m_OnCollisionEnter;
        protected void OnCollisionEnter( Collision collision ) {
            m_OnCollisionEnter.Invoke( collision );
        }
    
        [SerializeField]
        protected OnCollisionUnityEvent m_OnCollisionStay;
        protected void OnCollisionStay( Collision collision ) {
            m_OnCollisionStay.Invoke( collision );
        }
    
        [SerializeField]
        protected OnCollisionUnityEvent m_OnCollisionExit;
        protected void OnCollisionExit( Collision collision ) {
            m_OnCollisionExit.Invoke( collision );
        }
    }
}