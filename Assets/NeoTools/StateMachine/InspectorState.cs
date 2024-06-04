using UnityEngine;
using UnityEngine.Events;
using Neo.Utility;

namespace Neo.StateMachine.Wrappers {
    // This whole class is a hack to work around the fact that Unity wont show the generic events in the parent class in the inspector
    [DisallowMultipleComponent]
    public class InspectorState : AInspectorState<InspectorStateMachine>
    {
        //Pick a better name maybe
        public bool Busy;//Use in code to determine if we are "in use" with out having to have many calls from event handlers when we enter and exit a state

        [System.Serializable]
        public new class EnterExitUnityEvent : UnityEvent<InspectorStateMachine, State<InspectorStateMachine>> { }

        [SerializeField]
        protected EnterExitUnityEvent   m_OnEnter;
        public bool HasListenerToOnEnter(GameObject go)
        {
            return HasListeningTo(m_OnEnter, go);
        }

        [SerializeField]
        protected EnterExitUnityEvent   m_OnExit;
        public bool HasListenerToOnExit(GameObject go)
        {
            return HasListeningTo(m_OnExit, go);
        }

        protected static bool HasListeningTo(EnterExitUnityEvent ev, GameObject go)
        {
            Object obj;
            System.Type objType;
            for (int ix = 0; ix < ev.GetPersistentEventCount(); ++ix)
            {
                obj = ev.GetPersistentTarget(ix);
                objType = obj.GetType();
                if (objType == typeof(GameObject))
                {
                    return go == (obj as GameObject);
                }
                else if (objType.IsSubclassOf(typeof(Component)))
                {
                    Component comp = obj as Component;
                    return go == comp.gameObject;
                }
            }

            return false;
        }

        protected override void Awake()
        {
            base.Awake();

            State.OnEnter += m_OnEnter.Invoke;
            State.OnExit += m_OnExit.Invoke;

            //m_OnEnter.AddListener(delegate (InspectorStateMachine ism, State<InspectorStateMachine> nextState ) {
            //    //Log.FormatObject("Entering state '{0}'", gameObject.name);


            //});

            //m_OnExit.AddListener(delegate (InspectorStateMachine ism, State<InspectorStateMachine> nextState)
            //{
            //    //Log.FormatObject("Exiting state '{0}'", gameObject.name);
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
        [ContextMenu("Neo/Search Transitions For References Of")]
        private void SearchForReferencesOf()
        {
            Editor.StateMachineSearch.SearchForReferencesOf(this);
        }
#endif
    }
}