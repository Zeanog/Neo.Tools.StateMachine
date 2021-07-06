using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Neo.StateMachine.Editor {
    [ExecuteAlways]
    class StateMachineDebugger : EditorWindow {
        [MenuItem("Neo/StateMachine/Debugger")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            StateMachineDebugger window = (StateMachineDebugger)EditorWindow.GetWindow(typeof(StateMachineDebugger));
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

        protected void OnMonitoredStateChange(State<Neo.StateMachine.Wrappers.InspectorStateMachine> current, State<Neo.StateMachine.Wrappers.InspectorStateMachine> previous)
        {
            Repaint();
        }

        protected Wrappers.InspectorStateMachine  m_MonitoredStateMachine = null;
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

                using (var nameBuilderSlip = Neo.Utility.DataStructureLibrary<System.Text.StringBuilder>.Instance.CheckOut())
                using ( var tabBuilderSlip = Neo.Utility.DataStructureLibrary<System.Text.StringBuilder>.Instance.CheckOut() ) {
                    EditorGUILayout.LabelField("Current State:", EditorStyles.boldLabel);
                    nameBuilderSlip.Value.Clear();
                    tabBuilderSlip.Value.Clear();

                    int indention = 0;
                    foreach (var subName in currentStateNameSlip.Value)
                    {
                        EditorGUILayout.BeginHorizontal();

                        nameBuilderSlip.Value.Append(string.Format("/{0}", subName));

                        EditorGUILayout.LabelField(tabBuilderSlip.Value.ToString(), GUILayout.MaxWidth(20 * indention));

                        if (EditorGUILayout.LinkButton(subName))
                        {
                            GameObject stateGO = GameObject.Find(nameBuilderSlip.Value.ToString());
                            Selection.activeGameObject = stateGO;

                        }
                        indention += 1;
                        tabBuilderSlip.Value.Append("     ");

                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Previous State:", EditorStyles.boldLabel);
                    nameBuilderSlip.Value.Clear();
                    tabBuilderSlip.Value.Clear();

                    indention = 0;
                    foreach (var subName in previousStateNameSlip.Value)
                    {
                        EditorGUILayout.BeginHorizontal();

                        nameBuilderSlip.Value.Append(string.Format("/{0}", subName));

                        EditorGUILayout.LabelField(tabBuilderSlip.Value.ToString(), GUILayout.MaxWidth(20 * indention));
                        if (EditorGUILayout.LinkButton(subName))
                        {
                            GameObject stateGO = GameObject.Find(nameBuilderSlip.Value.ToString());
                            Selection.activeGameObject = stateGO;

                        }
                        indention += 1;
                        tabBuilderSlip.Value.Append("     ");

                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
        }
    }
}