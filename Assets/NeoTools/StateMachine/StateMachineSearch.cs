using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using Neo.StateMachine.Wrappers;

namespace Neo.StateMachine.Editor {
#if UNITY_EDITOR
    public static class GameObjectSearchExtensions {
        [MenuItem("GameObject/Neo/Search States Enter\\Exit events For References Of", true)]
        public static bool SearchForReferencesOfValidator()
        {
            return Selection.activeGameObject != null && Selection.gameObjects.Length == 1;
        }

        [MenuItem("GameObject/Neo/Search States Enter\\Exit events For References Of")]
        public static void SearchForReferencesOf()
        {
            StateMachineSearch.SearchForReferencesOf(Selection.activeGameObject);
        }
    }

    public class StateMachineSearch : EditorWindow {
        [MenuItem("Neo/StateMachine/Search Code %#&s")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            StateMachineSearch window = (StateMachineSearch)EditorWindow.GetWindowWithRect<StateMachineSearch>(new Rect(0, 0, 300, 150));
            window.ShowPopup();
            window.Repaint();
        }

        protected static string m_SearchString;
        protected static List<string> m_PreviousCodeSearches = new List<string>();
        protected static int m_SelectedSearchIndex = 0;

        public void OnGUI()
        {
            EditorGUILayout.LabelField("Search code for");
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            m_SearchString = EditorGUILayout.TextField(m_SearchString, GUILayout.MaxWidth(250));

            if (GUILayout.Button("Search") && !string.IsNullOrEmpty(m_SearchString))
            {
                ClearForSearch();

                int index = m_PreviousCodeSearches.FindIndex(delegate (string prevSearch)
                {
                    return prevSearch.Equals(m_SearchString);
                });

                if (index < 0)
                {
                    m_PreviousCodeSearches.Insert(0, m_SearchString);
                    if (m_PreviousCodeSearches.Count > 5)
                    {
                        m_PreviousCodeSearches.RemoveAt(m_PreviousCodeSearches.Count - 1);
                    }
                } else
                {
                    string str = m_PreviousCodeSearches[index];
                    m_PreviousCodeSearches.RemoveAt(index);
                    m_PreviousCodeSearches.Insert(0, str);
                }
                m_SelectedSearchIndex = 0;

                var stateMachines = GameObject.FindObjectsByType<InspectorStateMachine>(FindObjectsSortMode.None);
                foreach (var stateMachine in stateMachines)
                {
                    SearchCode(stateMachine.InitialState, m_SearchString);
                }

                Selection.objects = m_SelectionGOs.ToArray();
            }
            EditorGUILayout.EndHorizontal();

            int newSelection = EditorGUILayout.Popup(m_SelectedSearchIndex, m_PreviousCodeSearches.ToArray());
            if (newSelection != m_SelectedSearchIndex)
            {
                m_SelectedSearchIndex = newSelection;
                m_SearchString = m_PreviousCodeSearches[newSelection];
            }
            EditorGUILayout.EndVertical();
        }

        protected static List<GameObject> m_SelectionGOs = new List<GameObject>();
        protected static Dictionary<object, object> m_VisitedInSearch = new Dictionary<object, object>();

        protected static List<System.Type> m_ValidComponentSearchTypes = new List<System.Type>() { typeof(InspectorTransition), typeof(InspectorState) };
        protected static Dictionary<System.Type, Action<InspectorStateMachine[], Component>> m_ComponentSearchHandlers = new Dictionary<Type, Action<InspectorStateMachine[], Component>>();

        static StateMachineSearch()
        {
            m_ComponentSearchHandlers.Add(typeof(InspectorTransition), delegate (InspectorStateMachine[] stateMachines, Component comp)
            {
                foreach (var stateMachine in stateMachines)
                {
                    SearchStatesForTransition(stateMachine.InitialState, comp as InspectorTransition);
                }
            });

            m_ComponentSearchHandlers.Add(typeof(InspectorState), delegate (InspectorStateMachine[] stateMachines, Component comp)
            {
                foreach (var stateMachine in stateMachines)
                {
                    SearchTransitionsForState(stateMachine.InitialState, comp as InspectorState);
                }
            });
        }

        //[MenuItem("GameObject/Neo/Search For References Of", true)]
        public static bool SearchForComponentReferencesOfValidator( Type searchType )
        {
            if (Selection.activeGameObject == null)
            {
                return false;
            }

            for (int ix = 0; ix < m_ValidComponentSearchTypes.Count; ++ix)
            {
                if (Selection.activeGameObject.GetComponent(m_ValidComponentSearchTypes[ix]))
                {
                    return true;
                }
            }

            return false;
        }

        public static void SearchForReferencesOf(GameObject searchFor)
        {
            ClearForSearch();

            try
            {
                var stateMachines = GameObject.FindObjectsByType<InspectorStateMachine>(FindObjectsSortMode.None);
                
                foreach( var stateMachine in stateMachines )
                {
                    SearchStatesForReferenceOf(stateMachine.InitialState, searchFor);
                }

                Selection.objects = m_SelectionGOs.ToArray();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public static void SearchForReferencesOf( Component comp )
        {
            ClearForSearch();

            try
            {
                var stateMachines = GameObject.FindObjectsByType<InspectorStateMachine>(FindObjectsSortMode.None);
                m_ComponentSearchHandlers[comp.GetType()].Invoke(stateMachines, comp);
                Selection.objects = m_SelectionGOs.ToArray();
            }
            catch( Exception ex )
            {
                Debug.LogException(ex);
            }
        }

        protected static void ClearForSearch()
        {
            m_SelectionGOs.Clear();
            m_VisitedInSearch.Clear();
        }

        protected static void SearchTransitionsForState(InspectorState state, InspectorState searchFor)
        {
            foreach (InspectorTransition nextTrans in state.Transitions)
            {
                SearchTransitionsForState(nextTrans, searchFor);
            }
        }

        protected static void SearchTransitionsForState(InspectorTransition transition, InspectorState searchFor)
        {
            if (m_VisitedInSearch.ContainsKey(transition))
            {
                return;
            }

            m_VisitedInSearch.Add(transition, null);

            foreach (InspectorState state in transition.NextStates)
            {
                if (searchFor == state)
                {
                    m_SelectionGOs.Add(transition.gameObject);
                }

                SearchTransitionsForState(state, searchFor);
            }
        }

        protected static void SearchStatesForTransition(InspectorState state, InspectorTransition searchFor)
        {
            if (m_VisitedInSearch.ContainsKey(state))
            {
                return;
            }

            m_VisitedInSearch.Add(state, null);

            foreach (InspectorTransition trans in state.Transitions)
            {
                if (searchFor == trans)
                {
                    m_SelectionGOs.Add(state.gameObject);
                }

                foreach (InspectorState nextState in trans.NextStates)
                {
                    SearchStatesForTransition(nextState, searchFor);
                }
            }
        }

        protected static void SearchCode(InspectorState state, string searchFor)
        {
            if (m_VisitedInSearch.ContainsKey(state))
            {
                return;
            }

            m_VisitedInSearch.Add(state, null);
            foreach (InspectorTransition transition in state.Transitions)
            {
                if (transition.Code.Contains(searchFor))
                {
                    m_SelectionGOs.Add(transition.gameObject);
                }

                foreach (InspectorState nextState in transition.NextStates)
                {
                    SearchCode(nextState, searchFor);
                }
            }
        }

        protected static void SearchStatesForReferenceOf(InspectorState state, GameObject searchFor )
        {
            if (m_VisitedInSearch.ContainsKey(state))
            {
                return;
            }

            m_VisitedInSearch.Add(state, null);

            if( state.HasListenerToOnEnter(searchFor) || state.HasListenerToOnEnter(searchFor) )
            {
                m_SelectionGOs.AddUnique(state.gameObject);
            }

            foreach (InspectorTransition transition in state.Transitions)
            {
                foreach (InspectorState nextState in transition.NextStates)
                {
                    SearchStatesForReferenceOf(nextState, searchFor);
                }
            }
        }
    }
#endif
}