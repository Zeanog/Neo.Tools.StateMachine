using UnityEngine;
using Neo.StateMachine.Wrappers;

namespace Neo.StateMachine.Wrappers {
    public class InspectorStateMachineReference : MonoBehaviour {
        [SerializeField]
        protected InspectorStateMachine m_Reference;
        public InspectorStateMachine Reference {
            get => m_Reference;
        }
    }
}