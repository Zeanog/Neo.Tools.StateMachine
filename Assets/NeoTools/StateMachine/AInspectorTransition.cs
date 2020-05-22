using UnityEngine;
using Neo.Utility;
using Neo.Utility.Extensions;

namespace Neo.StateMachine.Wrappers {
    public abstract class AInspectorTransition<TOwner> : AInspectorTransitionProxy<TOwner> where TOwner : MonoBehaviour, IStateMachineOwner {
        [SerializeField]
        //[FullSerializer.fsIgnore]
        protected bool                 m_ReadOnly = false;

        [SerializeField]
        //[FullSerializer.fsIgnore]
        protected string            m_Code = "";

        //[FullSerializer.fsIgnore]
        protected TOwner            m_Owner;

        //[FullSerializer.fsIgnore]
        protected Transition<TOwner>        m_Transition;
        public override Transition<TOwner>  Transition {
            get {
                return m_Transition;
            }
        }
    
        public string FullName {
            get {
                return transform.BuildFullName();
            }
        }
    
        protected virtual void  Awake() {
            m_Owner = transform.GetComponentInParent<TOwner>();
            ExceptionUtility.Verify<System.ArgumentNullException>( m_Owner != null, string.Format("Owner for transition '{0}' not found", name) );
    
            m_Transition = new Transition<TOwner>(FullName, m_Owner );
        }
    
        protected virtual void  Start() {
            if (!m_Transition.InitFromProgram(m_Code))
            {
                //m_Transition.InitFromProgram(m_Code);// Calling this again so we can step through if theres an error
                UnityEngine.Debug.LogError(string.Format("Unable to compile transition code on transition '{0}'", gameObject.name));
            }
        }
    
        public virtual void AddState( AInspectorState<TOwner> state )
        {
            m_Transition.AddState(state.State);
        }
    }
}