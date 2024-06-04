using UnityEngine;

namespace Neo.StateMachine.Wrappers {
    public abstract class AInspectorTransitionProxy<TOwner> : MonoBehaviour where TOwner : MonoBehaviour, IStateMachineOwner {
        public abstract Transition<TOwner>       Transition {
            get;
        }
    }
}