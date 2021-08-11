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

        protected void OnMonitoredStateChange(State<Wrappers.InspectorStateMachine> current, Transition<Wrappers.InspectorStateMachine> transitionUsed, State<Wrappers.InspectorStateMachine> previous)
        {
            m_TransitionUsed = transitionUsed;
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
                m_MonitoredStateMachineName = m_MonitoredStateMachine.gameObject.BuildFullName();
                m_MonitoredStateMachine.OnStateChange += OnMonitoredStateChange;
            }
        }
        protected string m_MonitoredStateMachineName;

        protected Transition<Wrappers.InspectorStateMachine> m_TransitionUsed;

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
            using (var currentStateNameSlip = Neo.Utility.DataStructureLibrary<List<string>>.Instance.CheckOut())
            using (var previousStateNameSlip = Neo.Utility.DataStructureLibrary<List<string>>.Instance.CheckOut())
            {
                currentStateNameSlip.Value.Clear();
                previousStateNameSlip.Value.Clear();

                if (Selection.activeGameObject == null)
                {
                    EditorGUILayout.LabelField("Select a state machine game object");
                }
                else if (MonitoredStateMachine == null)
                {
                    var monitor = Selection.activeGameObject.GetComponent<Wrappers.InspectorStateMachine>();
                    if (monitor != null)
                    {
                        MonitoredStateMachine = monitor;
                    }
                    else
                    {
                        EditorGUILayout.LabelField("Select a state machine game object");
                    }
                }

                EditorGUILayout.LabelField("Selected State Machine", EditorStyles.boldLabel);

                if (MonitoredStateMachine != null)
                {
                    if (MonitoredStateMachine.CurrentState != null)
                    {
                        MonitoredStateMachine.CurrentState.gameObject.BuildFullName(currentStateNameSlip);
                    }

                    if (MonitoredStateMachine.PreviousState != null)
                    {
                        MonitoredStateMachine.PreviousState.gameObject.BuildFullName(previousStateNameSlip);
                    }

                    if (EditorGUILayout.LinkButton(m_MonitoredStateMachineName))
                    {
                        GameObject stateGO = GameObject.Find(m_MonitoredStateMachineName);
                        Selection.activeGameObject = stateGO;
                    }
                }

                EditorGUILayout.Space();

                OnGUIName("Current State:", currentStateNameSlip.Value);
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                using (var transitionUsedNameSlip = Neo.Utility.DataStructureLibrary<List<string>>.Instance.CheckOut())
                {
                    transitionUsedNameSlip.Value.Clear();

                    if (m_TransitionUsed != null)
                    {
                        var inspTrans = Wrappers.InspectorStateMachine.FindInspectorTransition(m_MonitoredStateMachine.transform, m_TransitionUsed);
                        if(inspTrans != null)
                        {
                            inspTrans.gameObject.BuildFullName(transitionUsedNameSlip.Value);
                        }
                    }
                    OnGUIName("Transition Used:", transitionUsedNameSlip.Value);
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                OnGUIName("Previous State:", previousStateNameSlip.Value);

                EditorGUILayout.Space();
            }
        }
    }
}
#endif