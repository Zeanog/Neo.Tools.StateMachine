using UnityEngine;

public abstract class ADamageDispatcher : MonoBehaviour {
	[SerializeField]
	protected GameObject	m_FxPrefab;

	[SerializeField]
	protected float			m_Damage;

    [SerializeField]
    protected float         m_Force = 10.0f;

    public abstract void Dispatch(AProjectile owner);
    public abstract void Dispatch(AProjectile owner, RaycastHit[] hits);
    public virtual void Dispatch(AProjectile owner, Collision collision) { }

    protected virtual void	CreateFx( Vector3 position, Quaternion rotation ) {
		if( m_FxPrefab == null ) {
			return;
		}
		
		GameObject.Instantiate( m_FxPrefab, position, rotation );
	}
}