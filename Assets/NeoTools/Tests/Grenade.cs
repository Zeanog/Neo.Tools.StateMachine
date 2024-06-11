using UnityEngine;
using Neo.Utility;

[RequireComponent(typeof(Rigidbody))]
[System.Serializable]
public class Grenade : AProjectile_Dynamic {
    [SerializeField]
    protected float m_CookoffDelay;

    [SerializeField]
    protected float m_Speed;

    protected InvocationManager m_InvocationManager = null;

    public override void ApplyForces()
    {
        m_InvocationManager = new InvocationManager(this);

        GetComponent<Rigidbody>().AddRelativeForce(0.0f, 0.0f, m_Speed / Time.deltaTime, ForceMode.VelocityChange);

        ExceptionUtility.Verify<System.ArgumentOutOfRangeException>(m_CookoffDelay > 0.0f);
        m_InvocationManager.Invoke("OnDetonate", m_CookoffDelay);
    }

    protected void OnDetonate()
    {
        ADamageDispatcher[] dispatchers = this.GetComponents<ADamageDispatcher>();
        foreach (ADamageDispatcher dispatcher in dispatchers)
        {
            dispatcher.Dispatch(gameObject);
        }

        Destroy(gameObject);
    }

    protected void OnDestroy()
    {
        m_InvocationManager?.CancelAll();
    }
}