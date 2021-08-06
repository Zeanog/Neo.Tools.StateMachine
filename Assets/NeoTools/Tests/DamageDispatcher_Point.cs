using UnityEngine;

public class DamageDispatcher_Point : ADamageDispatcher {
	public void Dispatch( AProjectile owner ) {
		CreateFx( owner.transform.position, owner.transform.rotation );
	}
	
	public void Dispatch( AProjectile owner, Collision collision ) {
		if( collision.contacts.Length <= 0 ) {
			return;
		}
		
		CreateFx( collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal, owner.transform.up) );
		
		collision.gameObject.SendMessage( "Damage", m_Damage, SendMessageOptions.DontRequireReceiver );
	}

	public void Dispatch( AProjectile owner, RaycastHit[] hits ) {
		if( hits.Length <= 0 ) {
			return;
		}
		
		CreateFx( hits[0].point, Quaternion.LookRotation(hits[0].normal, owner.transform.up) );
		
		hits[0].transform.gameObject.SendMessage( "Damage", m_Damage, SendMessageOptions.DontRequireReceiver );
	}
}