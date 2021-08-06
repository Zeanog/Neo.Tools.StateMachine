using UnityEngine;
using Neo.Utility;

public class DestroyAfterDelayOf : MonoBehaviour {
	[SerializeField]
	protected float	m_Delay = 1.0f;

	void	Awake() {
		ExceptionUtility.Verify<System.ArgumentOutOfRangeException>( m_Delay > 0.0f );
		Destroy( gameObject, m_Delay );
	}
}