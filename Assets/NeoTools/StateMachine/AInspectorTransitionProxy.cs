using UnityEngine;

namespace Neo.StateMachine.Wrappers {
    //[FullSerializer.fsObject(MemberSerialization = FullSerializer.fsMemberSerialization.OptIn)]
    public abstract class AInspectorTransitionProxy<TOwner> : MonoBehaviour where TOwner : MonoBehaviour, IStateMachineOwner {
        public abstract Transition<TOwner>       Transition {
            get;
        }
    }
}