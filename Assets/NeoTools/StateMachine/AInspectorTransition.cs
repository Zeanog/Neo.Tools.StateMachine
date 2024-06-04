using UnityEngine;
using Neo.Utility;
using Neo.Utility.Extensions;

namespace Neo.StateMachine.Wrappers {
    public abstract class AInspectorTransition<TOwner> : AInspectorTransitionProxy<TOwner> where TOwner : MonoBehaviour, IStateMachineOwner {
        [SerializeField]
        protected bool                 m_ReadOnly = false;

        [SerializeField]
        protected string            m_Code = "";
        public string               Code {
            get {
                return m_Code;
            }
        }

        [SerializeField]
        protected TOwner            m_Owner;

        [HideInInspector]
        protected Transition<TOwner>        m_Transition;
        public override Transition<TOwner>  Transition {
            get {
                EnsureTransitionInstance();
                return m_Transition;
            }
        }
    
        public string FullName {
            get {
                return transform.BuildFullName();
            }
        }

        protected void EnsureTransitionInstance()
        {
            if( m_Owner != null && m_Transition != null )
            {
                return;
            }

            m_Owner = transform.GetComponentInParent<TOwner>();
            ExceptionUtility.Verify<System.ArgumentNullException>(m_Owner != null, string.Format("Owner for transition '{0}' not found", name));

            m_Transition = new Transition<TOwner>(FullName, m_Owner);
        }
    
        protected virtual void  Awake() {
            EnsureTransitionInstance();
        }
    
        protected virtual void Start() {
            if (!m_Transition.InitFromProgram(m_Code))
            {
                //m_Transition.InitFromProgram(m_Code);// Calling this again so we can step through if theres an error
                UnityEngine.Debug.LogError(string.Format("Unable to compile transition code on transition '{0}'", gameObject.name));
            }
        }
    
        public virtual void AddState( AInspectorState<TOwner> state )
        {
            System.Diagnostics.Debug.Assert(m_Transition != null);
            System.Diagnostics.Debug.Assert(state != null);
            System.Diagnostics.Debug.Assert(state.State != null);
            m_Transition.AddState(state.State);
        }
    }
}