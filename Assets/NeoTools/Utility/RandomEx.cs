using UnityEngine;

public class RandomEx {
	public static Vector3	Range( Vector3 min, Vector3 max ) {
		return new Vector3( Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z) );
	}
	
	public static Vector3	RandomVector() {
		return Random.onUnitSphere;
	}
	
	public static Vector3	RandomVectorWithinCone( Vector3 forwardVec, float maxAngle ) {
		float ang = Mathf.Sin( maxAngle * Random.value * Mathf.Deg2Rad );
		float spin = 360.0f * Mathf.Deg2Rad * Random.value;
		
		// THis first cross product could fail in two instances.  If the difference between these two vectors is 0 or 180
		Vector3 crossVec1 = Vector3.Cross( RandomVector(), forwardVec ).normalized;
		Vector3 crossVec2 = Vector3.Cross( forwardVec, crossVec1 ).normalized;
			
		Vector3 randVec = forwardVec + crossVec1 * (ang * Mathf.Sin(spin)) - crossVec2 * (ang * Mathf.Cos(spin));
		return randVec.normalized;
	}
}