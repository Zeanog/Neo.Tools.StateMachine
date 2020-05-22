using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using Neo.StateMachine.Wrappers;

public class AssociateWithStateMachine : MonoBehaviour {
    [SerializeField]
    protected InspectorStateMachine     m_StateMachine;

    [Space]
    [Space]
    [SerializeField]
    protected List<UnityEngine.MonoBehaviour>          m_Associates;

    [Serializable]
    public class NotificationUnityEvent : UnityEvent<InspectorStateMachine> {}
    public NotificationUnityEvent   OnRegisterAssociation;

    protected void Awake() {
        foreach(var assoc in m_Associates) {
            m_StateMachine.AddAssociation(assoc);
        }

        OnRegisterAssociation?.Invoke(m_StateMachine);
    }
}