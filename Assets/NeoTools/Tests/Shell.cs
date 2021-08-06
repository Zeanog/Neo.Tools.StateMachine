using UnityEngine;
using System;

[System.Serializable]
public class Shell {
	[SerializeField]
	protected int			m_NumProjectiles;

	[SerializeField]
	protected GameObject	m_ProjectilePrefab;

	private GameObject		m_Projectile = null;
    protected AProjectile m_ProjInterface;

    public void	Awake() {
		m_Projectile = GameObject.Instantiate( m_ProjectilePrefab, Vector3.zero, Quaternion.identity ) as GameObject;
        m_Projectile.SetActive(false);
        m_ProjInterface = m_Projectile.GetComponent<AProjectile>();
        m_ProjInterface.EnsureCache();
    }

	public virtual bool	LaunchProjectiles( float spread, Transform startTransform ) {
		if (m_ProjInterface == null) {
			return false;
		}
 
		for (int ix = 0; ix < m_NumProjectiles; ++ix) {
			if( !m_ProjInterface.Launch(spread, startTransform) ) {
				return false;
			}
		}

		return true;
	}
}