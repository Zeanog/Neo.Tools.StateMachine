using UnityEngine;
using UnityEngine.Events;
using System;
using Neo.StateMachine.Wrappers;

[Serializable]
public class Magazine {
	[SerializeField]
	protected	int m_Capacity = 1;

	[SerializeField]
	protected	int	m_Count = 1;

	public int	Count {
		get {
			return m_Count;
		}
	}
	
	public	void	Reload() {
		m_Count = m_Capacity;
	}
	
	public void		Use() {
		m_Count = Mathf.Clamp( Count - 1, 0, m_Capacity );
	}
	
	public bool		IsEmpty {
		get {
			return Count <= 0;
		}
	}
	
	public bool		IsFull {
		get {
			return Count >= m_Capacity;
		}
	}
}

public class FPSGunExample : MonoBehaviour {
    [SerializeField]
    protected InspectorStateMachine m_StateMachine;

	[SerializeField]
	protected Shell			m_Shell;
	
	[SerializeField]
	protected Magazine		m_Clip;
	
	[SerializeField]
	protected float			m_RoundsPerSec;
	
	[SerializeField]
	protected float			m_Spread = 10.0f; // In Degrees
	
	void	Awake() {
		m_Shell.Awake();

        m_StateMachine.AddAssociation( this );
	}

    public void StartUsing()
    {
        m_StateMachine.TriggerEvent("StartUsing");
    }

    public void StopUsing()
    {
        m_StateMachine.TriggerEvent("StopUsing");
    }

    public void Reload()
    {
        m_StateMachine.TriggerEvent("Reload");
    }

    #region State Interfaces
    public void		LaunchProjectiles() {
		m_Shell.LaunchProjectiles( m_Spread, transform );
	}
	
	public void		UseAmmo() {
		m_Clip.Use();
	}
	
	public void		ReloadClip() {
		m_Clip.Reload();
	}
	
	public bool		IsOutOfAmmo() {
        return m_Clip.IsEmpty;
	}
	
	public float	ReloadDuration {
		get {
			return 1.0f;
		}
	}
	
	public float	UseDelay {
		get {
			return 1.0f / m_RoundsPerSec;
		}
	}
#endregion
}