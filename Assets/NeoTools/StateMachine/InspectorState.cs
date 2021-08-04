using UnityEngine;
using UnityEngine.Events;

namespace Neo.StateMachine.Wrappers {
    // This whole class is a hack to work around the fact that Unity wont show the generic events in the parent class in the inspector
    [DisallowMultipleComponent]
    public class InspectorState : AInspectorState<InspectorStateMachine>
    {
        [System.Serializable]
        public new class EnterExitUnityEvent : UnityEvent<InspectorStateMachine, State<InspectorStateMachine>> { }

        //[FullSerializer.fsIgnore]
        [SerializeField]
        protected EnterExitUnityEvent   m_OnEnter;

        //[FullSerializer.fsIgnore]
        [SerializeField]
        protected EnterExitUnityEvent   m_OnExit;

        protected override void Awake()
        {
            base.Awake();

            State.OnEnter += m_OnEnter.Invoke;
            State.OnExit += m_OnExit.Invoke;

            m_OnEnter.AddListener(delegate (InspectorStateMachine ism, State<InspectorStateMachine> nextState ) {
                Debug.LogFormat("Entering state '{0}'", gameObject.name);

#if UNITY_EDITOR
                UnityEditor.EditorGUIUtility.PingObject(gameObject.GetInstanceID());
#endif
            });

            //m_OnExit.AddListener(delegate ( InspectorStateMachine ism, State<InspectorStateMachine> nextState ) {
            //    Debug.LogFormat("Exiting state '{0}'", gameObject.name);
            //});
        }

        protected override void OnDestroy()
        {
            if (State != null)
            {
                State.OnEnter -= m_OnEnter.Invoke;
                State.OnExit -= m_OnExit.Invoke;
            }

            base.OnDestroy();
        }

#if UNITY_EDITOR
        [ContextMenu("Search For References Of", true)]
        private bool SearchForReferencesOfValidator()
        {
            return Editor.StateMachineSearch.SearchForReferencesOfValidator(GetType());
        }

        [ContextMenu("Search For References Of")]
        private void SearchForReferencesOf()
        {
            Editor.StateMachineSearch.SearchForReferencesOf(this);
        }
#endif
    }
}