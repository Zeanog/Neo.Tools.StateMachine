using UnityEngine;
using System.Collections.Generic;
using Neo.StateMachine.Wrappers;

[DisallowMultipleComponent]
public class ProjectileWeapon : MonoBehaviour {
    [SerializeField]
    protected List<WeaponDef> m_WeaponModes;

    protected List<InspectorStateMachine> m_UseStateMachines = new List<InspectorStateMachine>();

    protected void	Awake() {
        m_UseStateMachines.Clear();
        for (int ix = 0; ix < m_WeaponModes.Count; ++ix )
        {
            var sm = m_WeaponModes[ix].GetComponentInChildren<InspectorStateMachine>();
            m_UseStateMachines.Add( sm );
            sm.AddAssociation(m_WeaponModes[ix]);
            sm.AddAssociation(this);
        }
    }

    public void TriggerStateEvent(string evtName, int index)
    {
        m_UseStateMachines[index].TriggerEvent(evtName, false);
    }

    public void StartUsing(int index)
    {
        if (index < m_UseStateMachines.Count && !IsInUse)
        {
            m_UseStateMachines[index].TriggerEvent("StartUsing");
        }
    }

    public void StopUsing(int index)
    {
        if (index < m_UseStateMachines.Count && IsInUse)
        {
            m_UseStateMachines[index].TriggerEvent("StopUsing", false);
        }
    }

    public void Reload()
    {
        foreach (var stateMachine in m_UseStateMachines)
        {
            stateMachine.TriggerEvent("Reload", false);
        }
    }

    public bool IsInUse {
        get {
            foreach (var stateMachine in m_UseStateMachines)
            {
                if(stateMachine.CurrentState !=null && stateMachine.CurrentState.Busy )
                {
                    return true;
                }
            }

            return false;
        }
    }

    public void Raise()
    {
        if (!IsInUse)
        {
            m_UseStateMachines[0].QueueEvent("Raise");//We call this early so we attempt to queue it
        }
    }
    public System.Action OnRaised;
    public void Raised()
    {
        OnRaised?.Invoke();
    }

    public void Lower()
    {
        if (!IsInUse)
        {
            m_UseStateMachines[0].TriggerEvent("Lower");
        }
    }
    public System.Action OnLowered;
    public void Lowered()
    {
        OnLowered?.Invoke();
    }
}