using UnityEngine;
using System.Collections.Generic;

namespace Neo.StateMachine.Wrappers {
    [DisallowMultipleComponent]
    public class InspectorTransition : AInspectorTransition<InspectorStateMachine> {
        //[FullSerializer.fsIgnore]
        [SerializeField]
        protected List< InspectorState >     m_NextStates;
        public List<InspectorState>         NextStates {
            get {
                return m_NextStates;
            }
        }

        protected override void Start()
        {
            base.Start();

            for (int ix = 0; ix < m_NextStates.Count; ++ix)
            {
                AddState(m_NextStates[ix]);
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Neo/Search States For References Of", true)]
        private bool SearchForReferencesOfValidator()
        {
            return Editor.StateMachineSearch.SearchForComponentReferencesOfValidator(GetType());
        }

        [ContextMenu("Neo/Search States For References Of")]
        private void SearchForReferencesOf()
        {
            Editor.StateMachineSearch.SearchForReferencesOf(this);
        }
#endif
    }
}