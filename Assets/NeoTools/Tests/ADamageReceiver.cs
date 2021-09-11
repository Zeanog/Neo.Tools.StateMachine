using UnityEngine;

public abstract class ADamageReceiver : MonoBehaviour {
	[SerializeField]
	protected float	m_MaxDurability;

	protected float	m_CurrentDurability = 0.0f;

	public	float	Durability {
		get {
			return m_CurrentDurability;
		}

		set {
			m_CurrentDurability = Mathf.Clamp( value, 0.0f, m_MaxDurability );
		}	
	}

	public abstract void		Damage( float amount );
    public abstract void        ApplyForce(Vector3 amount);

    public virtual void			Heal() {
		Durability = m_MaxDurability;
	}

	protected virtual void	Awake() {
		Heal();
	}
}