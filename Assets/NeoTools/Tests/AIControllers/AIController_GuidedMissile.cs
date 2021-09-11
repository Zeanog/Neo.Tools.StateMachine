using UnityEngine;

public class AIController_GuidedMissile : AIController {
    protected float m_NoiseInput;
    protected float m_NoiseFrequency = 1.0f;
    protected Vector2 m_NoiseGain = Vector2.one;

	protected override void Awake() {
		
	}

	protected override void	Update() {
        m_NoiseInput += Time.deltaTime * m_NoiseFrequency;
        float noiseInput = m_NoiseInput;
        float xFrac = Mathf.PerlinNoise(noiseInput, 0);
        float yFrac = Mathf.PerlinNoise(0, noiseInput);

        //float scale = DetermineNoiseScale(self);

        Vector3 offsets;
        offsets.x = m_NoiseGain.x * xFrac;
        offsets.y = m_NoiseGain.y * yFrac;
        offsets.z = 0.0f;
 

        //FTransform transform(self->GetActorRotation());
        //FVector denoisedLocation = actorLoc - m_NoiseOffsets_WorldSpace;

        //if (self->ProjectileMovement->HomingTargetComponent == NULL)
        //{
        //    FRotator deltaRot = (self->TargetDirection.ToOrientationRotator() - self->GetActorRotation());
        //    self->SetActorRotation(self->GetActorRotation() + deltaRot * deltaTime * 15.0f);
        //    self->ProjectileMovement->SetVelocityInLocalSpace(FVector::ForwardVector * self->ProjectileMovement->Velocity.Size());

        //    FVector ptOnLine = ProjectPointOnLine(denoisedLocation, self->TargetDirection, self->StartPosition);
        //    denoisedLocation = ptOnLine + (denoisedLocation - ptOnLine) * scale;
        //}

        //m_NoiseOffsets_WorldSpace = transform.TransformVector(offsets);
        //self->SetActorLocation(denoisedLocation + m_NoiseOffsets_WorldSpace);
        //m_Plane.SetNormalAndPosition( transform.forward, transform.position );

        //Vector3 dirToWaypoint;
        //if( m_Plane.GetSide(m_Waypoint) ) {
        //	dirToWaypoint = RandomEx.RandomVectorWithinCone( transform.forward, m_Spread );
        //	m_Waypoint = transform.position + dirToWaypoint * m_DistanceToWaypoint;
        //}

        //dirToWaypoint = (m_Waypoint - transform.position).normalized;
        //Quaternion q = Quaternion.RotateTowards( transform.rotation, Quaternion.LookRotation(dirToWaypoint), 0.1f );
    }

	protected virtual void OnDrawGizmos() {
		//Gizmos.DrawLine( transform.position, m_Waypoint );
	}

	public static Vector3 Lerp( Vector3 from, Vector3 to, float frac ) {
		Vector3 res = new Vector3();

		for( int ix = 0; ix < 3; ++ix ) {
			res[ix] = Mathf.Lerp( from[ix], to[ix], frac );
		}

		return res;
	}
}