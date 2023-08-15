using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

#if UNITY_EDITOR
namespace Neo.StateMachine.Editor {
    [ExecuteAlways]
    class StateMachineDebugger : EditorWindow {
        [MenuItem("Neo/StateMachine/Debugger  %#&d")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            StateMachineDebugger window = (StateMachineDebugger)EditorWindow.GetWindow<StateMachineDebugger>();
            window.ShowPopup();
            window.Repaint();
        }

        protected void OnDestroy()
        {
            Selection.selectionChanged -= SelectionChanged;
        }

        protected void OnEnable()
        {
            Selection.selectionChanged -= SelectionChanged;
            Selection.selectionChanged += SelectionChanged;
        }

        protected void SelectionChanged()
        {
            if (Selection.activeGameObject == null)
            {
                return;
            }

            var monitor = Selection.activeGameObject.GetComponent<Wrappers.InspectorStateMachine>();
            if (monitor != null)
            {
                MonitoredStateMachine = monitor;
            }

            Repaint();
        }

        public class StateSnapshot {
            public State<Wrappers.InspectorStateMachine> Current {
                get;
                protected set;
            }

            public List<string> DecomposedName = new List<string>();

            public void Update(Wrappers.InspectorStateMachine monitoredStateMachine, State<Wrappers.InspectorStateMachine> state)
            {
                DecomposedName.Clear();

                Current = state;
                var alias = Wrappers.InspectorStateMachine.FindInspectorAlias(monitoredStateMachine.transform, Current);
                alias?.gameObject.BuildFullName(DecomposedName);
            }
        }

        public class TransitionSnapshot {
            public Transition<Wrappers.InspectorStateMachine> Current {
                get;
                protected set;
            }

            public List<string> DecomposedName = new List<string>();

            public void Update(Wrappers.InspectorStateMachine monitoredStateMachine, Transition<Wrappers.InspectorStateMachine> state)
            {
                DecomposedName.Clear();

                Current = state;
                var alias = Wrappers.InspectorStateMachine.FindInspectorAlias(monitoredStateMachine.transform, Current);
                alias?.gameObject.BuildFullName(DecomposedName);
            }
        }

        protected StateSnapshot         m_PreviousState = new StateSnapshot();
        protected TransitionSnapshot    m_TransitionUsed = new TransitionSnapshot();
        protected StateSnapshot         m_CurrentState = new StateSnapshot();

        protected void OnMonitoredStateChange(State<Wrappers.InspectorStateMachine> current, Transition<Wrappers.InspectorStateMachine> transitionUsed, State<Wrappers.InspectorStateMachine> previous)
        {
            m_PreviousState.Update(m_MonitoredStateMachine, previous);
            m_TransitionUsed.Update(m_MonitoredStateMachine, transitionUsed);
            m_CurrentState.Update(m_MonitoredStateMachine, current);

            Repaint();
        }

        protected Wrappers.InspectorStateMachine m_MonitoredStateMachine = null;
        public Wrappers.InspectorStateMachine MonitoredStateMachine {
            get {
                return m_MonitoredStateMachine;
            }

            protected set {
                if (m_MonitoredStateMachine != null)
                {
                    m_MonitoredStateMachine.OnStateChange -= OnMonitoredStateChange;
                }

                m_MonitoredStateMachine = value;
                if (m_MonitoredStateMachine != null)
                {
                    m_MonitoredStateMachineName = m_MonitoredStateMachine.gameObject.BuildFullName();
                    m_MonitoredStateMachine.OnStateChange += OnMonitoredStateChange;
                }
            }
        }
        protected string m_MonitoredStateMachineName;

        protected void OnGUIName(string label, List<string> name)
        {
            using (var nameBuilderSlip = Neo.Utility.DataStructureLibrary<System.Text.StringBuilder>.Instance.CheckOut())
            using (var tabBuilderSlip = Neo.Utility.DataStructureLibrary<System.Text.StringBuilder>.Instance.CheckOut())
            {
                EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
                nameBuilderSlip.Value.Clear();
                tabBuilderSlip.Value.Clear();

                int indention = 0;
                foreach (var subName in name)
                {
                    EditorGUILayout.BeginHorizontal();

                    nameBuilderSlip.Value.Append(string.Format("/{0}", subName));

                    EditorGUILayout.LabelField(tabBuilderSlip.Value.ToString(), GUILayout.MaxWidth(indention));
                    if (EditorGUILayout.LinkButton(subName))
                    {
                        GameObject stateGO = GameObject.Find(nameBuilderSlip.Value.ToString());
                        Selection.activeGameObject = stateGO;

                    }
                    indention += 20;
                    tabBuilderSlip.Value.Append("     ");

                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        public void OnGUI()
        {
            EditorGUILayout.LabelField("Selected State Machine", EditorStyles.boldLabel);

            if (MonitoredStateMachine != null)
            {
                EditorGUILayout.BeginHorizontal();
                if (EditorGUILayout.LinkButton(m_MonitoredStateMachineName))
                {
                    GameObject stateGO = GameObject.Find(m_MonitoredStateMachineName);
                    Selection.activeGameObject = stateGO;
                }
                EditorGUILayout.Space();
                if (GUILayout.Button("Clear", GUILayout.MaxWidth(75)))
                {
                    MonitoredStateMachine = null;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                OnGUIName("Current State:", m_CurrentState.DecomposedName);
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                OnGUIName("Transition Used:", m_TransitionUsed.DecomposedName);
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                OnGUIName("Previous State:", m_PreviousState.DecomposedName);

                EditorGUILayout.Space();
            }
            else
            {
                EditorGUILayout.LabelField("Select a state machine game object");
            }
        }
    }
}
#endif