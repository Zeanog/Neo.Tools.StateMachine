using UnityEngine;
using Neo.Utility;
using Neo.StateMachine.Internal;
using Neo.Utility.Extensions;
using System;

namespace Neo.StateMachine.Wrappers {
    //[FullSerializer.fsObject(MemberSerialization = FullSerializer.fsMemberSerialization.Default)]
    //[DisallowMultipleComponent]
    public class InspectorStateMachine : MonoBehaviour, IStateMachineOwner {
        protected StateMachine<InspectorStateMachine>    m_Controller;
        public StateMachine<InspectorStateMachine> Controller {
            get {
                return m_Controller;
            }
        }

        //[FullSerializer.fsIgnore]
        [SerializeField]
        protected InspectorState     m_InitialState;

        //[FullSerializer.fsProperty]
        public InspectorState CurrentState {
            get {
                return m_Controller == null ? null : FindOwner(transform, m_Controller.CurrentState);
            }

            set {
                m_Controller.ChangeState(value.State, this);
            }
        }

        public InspectorState PreviousState {
            get {
                return m_Controller == null ? null : FindOwner(transform, m_Controller.PreviousState);
            }
        }

        public Action<State<InspectorStateMachine>, State<InspectorStateMachine>> OnStateChange;

        protected void Awake()
        {
            Clock.Time = delegate () { return UnityEngine.Time.time; };
            Clock.DeltaTime = delegate () { return UnityEngine.Time.deltaTime; };
            Log.SetErrorHandler(UnityEngine.Debug.LogError);
            Log.SetWarningHandler(UnityEngine.Debug.LogWarning);
            Log.SetLogHandler(UnityEngine.Debug.Log);
            Log.SetExceptionHandler(UnityEngine.Debug.LogException);

            m_Controller = new StateMachine<InspectorStateMachine>(this);
            m_Controller.OnStateChange += delegate (State<InspectorStateMachine> current, State<InspectorStateMachine> previous)
            {
                OnStateChange?.Invoke(current, previous);
            };
        }
    
        protected System.Collections.IEnumerator  Start() {
            ExceptionUtility.Verify<System.NullReferenceException>( m_InitialState != null, "Undefined 'm_InitialState'" );

            using(var slip = DataStructureLibrary<WaitForEndOfFrame>.Instance.CheckOut()) {
                yield return slip.Value;
            }

            m_Controller.ChangeState( m_InitialState.State, this );
            StartCoroutine( "ProcessStateMachine" );
        }

        protected void OnDestroy()
        {
            m_Controller = null;
        }

        protected System.Collections.IEnumerator    ProcessStateMachine() {
            while( true ) {
                using(var slip = DataStructureLibrary<WaitForEndOfFrame>.Instance.CheckOut()) {
                    yield return slip.Value;
                }

                m_Controller.Evaluate();
            }
        }
    
        public void  RegisterEvent( StaticString key, TransitionEventDelegate d ) {
            m_Controller.RegisterEvent(key, d);
        }

        public void     TriggerEvent( string name ) {
            m_Controller.TriggerEvent(name);
        }

        public void AddAssociation( System.Object obj ) {
            m_Controller.AddAssociation( obj );
        }

        public void RemoveAssociation(System.Object obj)
        {
            m_Controller.RemoveAssociation(obj);
        }

        public TransitionOnStateDelegate FindAssociatedOnStateMethod( string methodName ) {
            return m_Controller.FindAssociatedOnStateMethod(methodName);
        }

        public TransitionOnDelayDelegate FindAssociatedOnDelayMethod(string methodName)
        {
            return m_Controller.FindAssociatedOnDelayMethod(methodName);
        }

        public void RegisterPlug(StaticString name, TransitionPlug<float> plug)
        {
            m_Controller.RegisterPlug(name, plug);
        }

        public TransitionPlug<float> FindPlug(StaticString name)
        {
            return m_Controller.FindPlug(name);
        }

        protected static InspectorState FindOwner( Transform self, State<InspectorStateMachine> internalState )
        {
            return self.VisitComponentInChildren<InspectorState>(delegate (InspectorState state) {
                return state.State == internalState;
            });
        }
    }
}