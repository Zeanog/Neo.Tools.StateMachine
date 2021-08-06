using UnityEngine;

public abstract class ADamageDispatcher : MonoBehaviour {
	[SerializeField]
	protected GameObject	m_FxPrefab;

	[SerializeField]
	protected float			m_Damage;

	protected virtual void	CreateFx( Vector3 position, Quaternion rotation ) {
		if( m_FxPrefab == null ) {
			return;
		}
		
		GameObject.Instantiate( m_FxPrefab, position, rotation );
	}
}