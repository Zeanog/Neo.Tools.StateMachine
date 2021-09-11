using UnityEngine;

public class DamageDispatcher_Point : ADamageDispatcher {
    public override void Dispatch( AProjectile owner ) {
		CreateFx( owner.transform.position, owner.transform.rotation );
	}
	
	public override void Dispatch( AProjectile owner, Collision collision ) {
		if( collision.contacts.Length <= 0 ) {
			return;
		}
		
		CreateFx( collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal, owner.transform.up) );

        var damageReceiver = collision.gameObject.GetComponent<DamageReceiver>();
        if (damageReceiver != null)
        {
            damageReceiver.Damage(m_Damage);
        }

        var rigidBody = collision.gameObject.GetComponent<Rigidbody>();
        if (rigidBody != null && m_Force > 0.0f)
        {
            rigidBody.AddForce(owner.transform.forward * m_Force, ForceMode.Force);
        }
    }

	public override void Dispatch( AProjectile owner, RaycastHit[] hits ) {
		if( hits.Length <= 0 ) {
			return;
		}
		
		CreateFx( hits[0].point, Quaternion.LookRotation(hits[0].normal, owner.transform.up) );

        var damageReceiver = hits[0].transform.gameObject.GetComponent<DamageReceiver>();
        if (damageReceiver != null)
        {
            damageReceiver.Damage(m_Damage);
            //damageReceiver.ApplyForce(owner.transform.forward * m_Force);
        }
        var rigidBody = hits[0].transform.GetComponent<Rigidbody>();
        if (rigidBody != null && m_Force > 0.0f)
        {
            rigidBody.AddForce(owner.transform.forward * m_Force, ForceMode.Force);
        }
    }
}