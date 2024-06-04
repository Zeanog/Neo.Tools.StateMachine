using UnityEngine;
using UnityEngine.Events;
using Neo.Utility;

namespace Neo.StateMachine.Wrappers {
    public abstract class AInspectorState<TOwner> : MonoBehaviour where TOwner : MonoBehaviour, IStateMachineOwner {
        [System.Serializable]
        protected class EnterExitUnityEvent : UnityEvent<TOwner, State<TOwner>> {}

#if UNITY_EDITOR
        [HideInInspector]
        public System.Collections.Generic.List<AInspectorTransitionProxy<TOwner>> m_Transitions = new System.Collections.Generic.List<AInspectorTransitionProxy<TOwner>>();
        public System.Collections.Generic.List<AInspectorTransitionProxy<TOwner>> Transitions {
            get {
                EnsureTransitionRegistration();
                return m_Transitions;
            }
        }
#endif

        public virtual State<TOwner> State {
            get;
        } = new State<TOwner>();

        protected virtual void  Awake() {
        }

        protected virtual void OnDestroy() {
            //m_Impl = null;
        }

        protected virtual void EnsureTransitionRegistration()
        {
            if (State.ExitTransitionList.Count > 0 )
            {
                return;
            }

            foreach (Transform child in transform)
            {
                if (!child.gameObject.activeInHierarchy)
                {
                    continue;
                }
                AInspectorTransitionProxy<TOwner> transitionProxy = child.GetComponent(typeof(AInspectorTransitionProxy<TOwner>)) as AInspectorTransitionProxy<TOwner>;
                ExceptionUtility.Verify<System.ArgumentNullException>(transitionProxy != null && transitionProxy.Transition != null, string.Format("Can't find TransitionProxy on {0}/{1}", name, child.name));
                State.AddTransition(transitionProxy.Transition);

#if UNITY_EDITOR
                m_Transitions.Add(transitionProxy);
#endif
            }
        }

        protected virtual void  Start() {
            EnsureTransitionRegistration();
        }
    }
}