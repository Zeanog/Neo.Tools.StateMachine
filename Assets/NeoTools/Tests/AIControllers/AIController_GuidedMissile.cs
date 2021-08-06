using UnityEngine;

public class AIController_GuidedMissile : AIController {
	protected Vector3	m_Waypoint;

	[SerializeField]
	protected float		m_DistanceToWaypoint = 5.0f;

	protected Plane		m_Plane;

	[SerializeField]
	protected float		m_Spread = 5.0f;

	protected override void Awake() {
		m_Plane = new Plane( transform.forward, transform.position );

		Vector3 dirToWaypoint = RandomEx.RandomVectorWithinCone( transform.forward, m_Spread );
		m_Waypoint = dirToWaypoint * m_DistanceToWaypoint;
	}

	protected override void	Update() {
		m_Plane.SetNormalAndPosition( transform.forward, transform.position );

		Vector3 dirToWaypoint;
		if( m_Plane.GetSide(m_Waypoint) ) {
			dirToWaypoint = RandomEx.RandomVectorWithinCone( transform.forward, m_Spread );
			m_Waypoint = transform.position + dirToWaypoint * m_DistanceToWaypoint;
		}

		dirToWaypoint = (m_Waypoint - transform.position).normalized;
		Quaternion q = Quaternion.RotateTowards( transform.rotation, Quaternion.LookRotation(dirToWaypoint), 0.1f );
	}

	protected virtual void OnDrawGizmos() {
		Gizmos.DrawLine( transform.position, m_Waypoint );
	}

	public static Vector3 Lerp( Vector3 from, Vector3 to, float frac ) {
		Vector3 res = new Vector3();

		for( int ix = 0; ix < 3; ++ix ) {
			res[ix] = Mathf.Lerp( from[ix], to[ix], frac );
		}

		return res;
	}
}