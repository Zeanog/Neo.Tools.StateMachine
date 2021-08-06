using UnityEngine;
using System.Reflection;
using Neo.Utility;

public abstract class AProjectile : MonoBehaviour {
	public virtual void		EnsureCache() {}
	public abstract bool	Launch( float spread, Transform startTransform );
}

public abstract class AProjectile_Static : AProjectile {
}

public abstract class AProjectile_Dynamic : AProjectile {
}

public class InvocationManager {
	protected System.Collections.Generic.List<string>	m_Invocations = new System.Collections.Generic.List<string>();

	public MonoBehaviour	Owner {
		get;
		protected set;
	}

	public InvocationManager( MonoBehaviour owner ) {
		Owner = owner;
	}

	public void		Invoke( string methodName, float time ) {
		Owner.Invoke( methodName, time );
		m_Invocations.Add( methodName );// Should be AddUnique
	}

	public void		CancelAll() {
		foreach( string methodName in m_Invocations ) {
			Owner.CancelInvoke( methodName );
		}
	}
}

[RequireComponent(typeof(Rigidbody))]
[System.Serializable]
public class Projectile : AProjectile_Dynamic {
	[SerializeField]
	protected GameObject	m_FizzlePrefab;

	[SerializeField]
	protected float			m_TimeToFizzle;

	[SerializeField]
	protected float			m_Speed;

	protected InvocationManager	m_InvocationManager = null;

	public override bool	Launch( float spread, Transform startTransform ) {
		Vector3 startDir = RandomEx.RandomVectorWithinCone( startTransform.forward, spread );
		GameObject proj = UnityEngine.Object.Instantiate( gameObject, startTransform.position, Quaternion.LookRotation(startDir) ) as GameObject;
        proj.SetActive(true);
        var projComp = proj.GetComponent<Projectile>();
        projComp.ApplyForces();

        return proj != null;
	}

    public void ApplyForces()
    {
        m_InvocationManager = new InvocationManager(this);

        GetComponent<Rigidbody>().AddRelativeForce(0.0f, 0.0f, m_Speed / Time.deltaTime, ForceMode.VelocityChange);

        ExceptionUtility.Verify<System.ArgumentOutOfRangeException>(m_TimeToFizzle > 0.0f);
        m_InvocationManager.Invoke("OnFizzle", m_TimeToFizzle);
    }

    void	Awake() {
		
	}
	
	void	Start() {
		
	}

	protected void	CreateFizzle() {
		if( m_FizzlePrefab == null ) {
			return;
		}
		
		if( Camera.main == null ) {
			return;
		}
		
		Vector3 vecToEye = Camera.main.transform.position - transform.position;
		vecToEye.Normalize();
		
		m_FizzlePrefab.transform.localScale = transform.localScale;
		GameObject.Instantiate( m_FizzlePrefab, transform.position, Quaternion.LookRotation(vecToEye, Camera.main.transform.up) );
	}

	protected void	OnFizzle() {
		CreateFizzle();
		Destroy( gameObject );
	}

	protected void	OnDestroy() {
		m_InvocationManager?.CancelAll();
	}

	private System.Object[] args = new System.Object[2];
	private System.Type[] argTypes = new System.Type[2];
	protected void	OnCollisionEnter( Collision collision ) {
		args[0] = this;
		args[1] = collision;
		argTypes[0] = GetType();
		argTypes[1] = collision.GetType();

		gameObject.SetActive( false );

		ADamageDispatcher[] dispatchers = this.GetComponents<ADamageDispatcher>();
		foreach( ADamageDispatcher dispatcher in dispatchers ) {
			MethodInfo info = ReflectionUtils.FindMethodInfo( dispatcher.GetType(), "Dispatch", argTypes );
			if( info == null ) {
				UnityEngine.Debug.LogError( string.Format("Unable to find method {0}", ReflectionUtils.BuildMethodInfoError("Dispatch", argTypes)) );
				continue;
			}
			
			info.Invoke( dispatcher, args );
		}

		Destroy( gameObject );
	}

}