using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using Neo.Utility;

public class ZDistanceSort : IComparer<RaycastHit> {
	private static ZDistanceSort	m_Instance = new ZDistanceSort();
	public static ZDistanceSort		Instance {
		get {
			return m_Instance;
		}
	}

	public int Compare( RaycastHit lhs, RaycastHit rhs ) {
		return lhs.distance < rhs.distance ? -1 : 1;
	}
}

[System.Serializable]
public class Projectile_Trace : AProjectile_Static {
	[SerializeField]
	protected float		m_Range = 100.0f;

	protected ADamageDispatcher[] m_Dispatchers = null;

	public override void	EnsureCache() {
		base.EnsureCache();

		if( m_Dispatchers == null ) {
			m_Dispatchers = gameObject.GetComponents<ADamageDispatcher>();
			ExceptionUtility.Verify<System.ArgumentOutOfRangeException>( m_Dispatchers.Length > 0 );
		}
	}

	private System.Object[] args = new System.Object[2];
	private System.Type[] argTypes = new System.Type[2];
	public override bool	Launch( float spread, Transform startTransform ) {
		ExceptionUtility.Verify<System.ArgumentNullException>( m_Range > 0.0f );
		ExceptionUtility.Verify<System.ArgumentNullException>( m_Dispatchers != null );
		
		Vector3 startDir = RandomEx.RandomVectorWithinCone( startTransform.forward, spread );
		RaycastHit[] hits = Physics.RaycastAll( startTransform.position, startDir, m_Range );
		if( hits == null || hits.Length <= 0 ) {
			return false;
		}

		System.Array.Sort( hits, ZDistanceSort.Instance );

		args[0] = this;
		args[1] = hits;
		argTypes[0] = GetType();
		argTypes[1] = hits.GetType();
		
		foreach( ADamageDispatcher dispatcher in m_Dispatchers ) {
			MethodInfo info = ReflectionUtils.FindMethodInfo( dispatcher.GetType(), "Dispatch", argTypes );
			if( info == null ) {
				UnityEngine.Debug.LogError( string.Format("Unable to find method {0}", ReflectionUtils.BuildMethodInfoError("Dispatch", argTypes)) );
				continue;
			}

			info.Invoke( dispatcher, args );
		}

		return true;
	}
}