using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using Neo.StateMachine.Wrappers;

[DisallowMultipleComponent]
public class FPSGunExample : MonoBehaviour {
    [SerializeField]
    protected List<InspectorStateMachine> m_StateMachines;

    [SerializeField]
    protected List<WeaponDef> m_WeaponModes;

    protected Dictionary<string, Action> m_OnAnimationStartHandlers = new Dictionary<string, Action>();
    protected Dictionary<string, Action> m_OnAnimationCompleteHandlers = new Dictionary<string, Action>();

    void	Awake() {
        for(int ix = 0; ix < m_StateMachines.Count; ++ix )
        {
            m_StateMachines[ix].AddAssociation(this);
            m_StateMachines[ix].AddAssociation(m_WeaponModes[ix]);
        }
	}

    public void StartUsing(int index)
    {
        if (index < m_StateMachines.Count)
        {
            m_StateMachines[index].TriggerEvent("StartUsing");
        }
    }

    public void StopUsing(int index)
    {
        if (index < m_StateMachines.Count)
        {
            m_StateMachines[index].TriggerEvent("StopUsing");
        }
    }

    public void Reload()
    {
        foreach (var stateMachine in m_StateMachines)
        {
            stateMachine.TriggerEvent("Reload");
        }
    }

    public bool IsInUse {
        get {
            foreach (var stateMachine in m_StateMachines)
            {
                if( stateMachine.CurrentState != stateMachine.InitialState )
                {
                    return true;
                }
            }

            return false;
        }
    }

    public void Raise()
    {
        //m_StateMachine.TriggerEvent("Raise");
    }

    public void Lower()
    {
        //m_StateMachine.TriggerEvent("Lower");
    }
}