using UnityEngine;
using UnityEngine.Events;
using Neo.Utility;

namespace Neo.StateMachine.Wrappers {
    public abstract class AInspectorState<TOwner> : MonoBehaviour where TOwner : MonoBehaviour, IStateMachineOwner {
        //[FullSerializer.fsIgnore]
        [System.Serializable]
        protected class EnterExitUnityEvent : UnityEvent<TOwner, State<TOwner>> {}

        //[FullSerializer.fsIgnore]
        protected State<TOwner> m_Impl;
    
        public virtual State<TOwner> State {
            get {
                return m_Impl;
            }
        }
    
        protected virtual void  Awake() {
            m_Impl = new State<TOwner>();
        }

        protected virtual void OnDestroy() {
            m_Impl = null;
        }
    
        protected virtual void  Start() {
            foreach( Transform child in transform ) {
                if( !child.gameObject.activeInHierarchy ) {
                    continue;
                }
                AInspectorTransitionProxy<TOwner> transitionProxy = child.GetComponent( typeof(AInspectorTransitionProxy<TOwner>) ) as AInspectorTransitionProxy<TOwner>;
                ExceptionUtility.Verify<System.ArgumentNullException>( transitionProxy != null && transitionProxy.Transition != null, string.Format("Can't find TransitionProxy on {0}/{1}", name, child.name) );
                m_Impl.AddTransition( transitionProxy.Transition );
            }
        }
    }
}