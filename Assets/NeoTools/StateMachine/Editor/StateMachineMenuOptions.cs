using UnityEngine;
using UnityEditor;
using Neo.StateMachine.Wrappers;
using Neo.Utility.Extensions;

namespace Neo.StateMachine {
    public class StateMachineMenuOptions {
        private static void    Create<TWrapper>() where TWrapper : MonoBehaviour {
            GameObject go = new GameObject( typeof(TWrapper).Name );
    
            if( Selection.activeGameObject != null ) {
                go.transform.SetParent( Selection.activeGameObject.transform, false );
            }
            go.transform.Reset();

            Add<TWrapper>(go);
        }

        private static void Add<TWrapper>( GameObject go ) where TWrapper : MonoBehaviour
        {
            if( !go )
            {
                return;
            }

            go.EnsureComponent<TWrapper>();
        }

        [MenuItem("GameObject/Neo/Create/InspectorState")]
        private static void    OnCreateState() {
            Create<InspectorState>();
        }
    
        [MenuItem("GameObject/Neo/Create/InspectorTransition")]
        private static void    OnCreateTransition() {
            Create<InspectorTransition>();
        }
    
        [MenuItem("GameObject/Neo/Create/InspectorStateMachine")]
        private static void    OnCreateStateMachine() {
            Create<InspectorStateMachine>();
        }

        [MenuItem("Component/Neo/InspectorState")]
        private static void OnAddState()
        {
            Add<InspectorState>(Selection.activeGameObject);
        }

        [MenuItem("Component/Neo/InspectorTransition")]
        private static void OnAddTransition()
        {
            Add<InspectorTransition>(Selection.activeGameObject);
        }

        [MenuItem("Component/Neo/InspectorStateMachine")]
        private static void OnAddStateMachine()
        {
            Add<InspectorStateMachine>(Selection.activeGameObject);
        }
    }
}