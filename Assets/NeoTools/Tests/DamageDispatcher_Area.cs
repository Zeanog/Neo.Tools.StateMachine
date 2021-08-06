using UnityEngine;

public class DamageDispatcher_Area : ADamageDispatcher {
	[SerializeField]
	protected float	m_Radius = 10.0f;
	
	protected void	Start() {

	}
	
	public void Dispatch( AProjectile owner ) {
		CreateFx( owner.transform.position, owner.transform.rotation );

		Collider[] areaHits = Physics.OverlapSphere( owner.transform.position, m_Radius );
		foreach( Collider areaHit in areaHits ) {
			if( areaHit.gameObject == this.gameObject ) {
				continue;// Don't hit self
			}

			if( areaHit.gameObject == owner.gameObject ) {
				continue;
			}

			Debug.Log( areaHit.name );
		}
	}
	
	public void Dispatch( AProjectile owner, Collision collision ) {
		if( collision.contacts.Length <= 0 ) {
			return;
		}
		
		CreateFx( collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal, owner.transform.up) );

		Collider[] areaHits = Physics.OverlapSphere( collision.contacts[0].point, m_Radius );
		foreach( Collider areaHit in areaHits ) {
			if( areaHit.gameObject == this.gameObject ) {
				continue;// Don't hit self
			}

			if( areaHit.gameObject == owner.gameObject ) {
				continue;
			}

			areaHit.gameObject.SendMessage( "Damage", m_Damage, SendMessageOptions.DontRequireReceiver );
		}
	}
	
	public void Dispatch( AProjectile owner, RaycastHit[] hits ) {
		if( hits.Length <= 0 ) {
			return;
		}
		
		CreateFx( hits[0].point, Quaternion.LookRotation(hits[0].normal, owner.transform.up) );

		Collider[] areaHits = Physics.OverlapSphere( hits[0].point, m_Radius );
		foreach( Collider areaHit in areaHits ) {
			if( areaHit.gameObject == this.gameObject ) {
				continue;// Don't hit self
			}

			if( areaHit.gameObject == owner.gameObject ) {
				continue;
			}

			areaHit.gameObject.SendMessage( "Damage", m_Damage, SendMessageOptions.DontRequireReceiver );
		}
	}
}