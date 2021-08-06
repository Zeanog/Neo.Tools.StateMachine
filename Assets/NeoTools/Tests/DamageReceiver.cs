using UnityEngine;

public class DamageReceiver : ADamageReceiver {
	public override void		Damage( float amount ) {
		Durability -= amount;
		
		if( Durability <= 0.0f ) {
			Debug.Log( "Destroyed!!!" );
			Destroy( gameObject );
		}
	}
}
