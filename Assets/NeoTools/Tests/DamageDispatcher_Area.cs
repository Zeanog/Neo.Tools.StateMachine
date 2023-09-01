using UnityEngine;

public class DamageDispatcher_Area : ADamageDispatcher {
	[SerializeField]
	protected float	m_Radius = 10.0f;

    protected void	Start() {

	}
	
	public override void Dispatch(GameObject owner ) {
		CreateFx( owner.transform.position, owner.transform.rotation );

		Collider[] areaHits = Physics.OverlapSphere( owner.transform.position, m_Radius );
		foreach( Collider areaHit in areaHits ) {
			if( areaHit.gameObject == this.gameObject ) {
				continue;// Don't hit self
			}

			if( areaHit.gameObject == owner.gameObject ) {
				continue;
			}

            var forceDir = (areaHit.transform.position - owner.transform.position);
            var rayCasts = Physics.RaycastAll(owner.transform.position, forceDir);
            if (rayCasts.Length > 0)
            {
                var damageReceiver = areaHit.gameObject.GetComponent<DamageReceiver>();
                if (damageReceiver != null)
                {
                    damageReceiver.Damage(m_Damage);
                    //damageReceiver.ApplyForce(forceDir * m_Force);                    
                }
                var rigidBody = areaHit.gameObject.GetComponent<Rigidbody>();
                if (rigidBody != null && m_Force > 0.0f)
                {
                    rigidBody.AddExplosionForce(m_Force, owner.transform.position, m_Radius);
                }
            }
        }
	}
	
	public override void Dispatch(GameObject owner, Collision collision ) {
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

            var forceDir = (areaHit.transform.position - owner.transform.position);
            var rayCasts = Physics.RaycastAll(owner.transform.position, forceDir);
            if (rayCasts.Length > 0)
            {
                var damageReceiver = areaHit.gameObject.GetComponent<DamageReceiver>();
                if (damageReceiver != null)
                {
                    damageReceiver.Damage(m_Damage);
                    //damageReceiver.ApplyForce(forceDir * m_Force);                    
                }
                var rigidBody = areaHit.gameObject.GetComponent<Rigidbody>();
                if (rigidBody != null && m_Force > 0.0f)
                {
                    rigidBody.AddExplosionForce(m_Force, owner.transform.position, m_Radius);
                }
            }
        }
	}
	
	public override void Dispatch(GameObject owner, RaycastHit[] hits ) {
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

            var forceDir = (areaHit.transform.position - owner.transform.position);
            var rayCasts = Physics.RaycastAll(owner.transform.position, forceDir);
            if (rayCasts.Length > 0)
            {
                var damageReceiver = areaHit.gameObject.GetComponent<DamageReceiver>();
                if (damageReceiver != null)
                {
                    damageReceiver.Damage(m_Damage);
                    //damageReceiver.ApplyForce(forceDir * m_Force);                    
                }
                var rigidBody = areaHit.gameObject.GetComponent<Rigidbody>();
                if (rigidBody != null && m_Force > 0.0f)
                {
                    rigidBody.AddExplosionForce(m_Force, owner.transform.position, m_Radius);
                }
            }
        }
	}
}