using UnityEngine;

namespace Neo.StateMachine.Wrappers {
    [DisallowMultipleComponent]
    public class InspectorTransitionProxy : AInspectorTransitionProxy<InspectorStateMachine> {
        //[FullSerializer.fsIgnore]
        [SerializeField]
        protected InspectorTransition   m_Transition;
    
        public override Transition<InspectorStateMachine>       Transition {
            get {
                return m_Transition.Transition;
            }
        }
    }
}