using UnityEngine;
using System;

[System.Serializable]
public class Shell {
	[SerializeField]
	public int			NumProjectiles;

    [SerializeField]
    protected GameObject m_ProjectilePrefab;

    [SerializeField]
    protected GameObject m_CasingPrefab;

    [HideInInspector]
    public IProjectileLauncher Launcher {
        get; set;
    }

    public bool   CreateLauncher( Transform parent )
    {
        if(m_ProjectilePrefab == null)
        {
            Launcher = null;
            return false;
        }

        TraceLauncher launcher = m_ProjectilePrefab.GetComponent<TraceLauncher>();
        if (launcher != null)
        {
            GameObject traceInst = GameObject.Instantiate(m_ProjectilePrefab, Vector3.zero, Quaternion.identity, parent);
            traceInst.transform.localPosition = Vector3.zero;
            traceInst.transform.localRotation = Quaternion.identity;
            traceInst.transform.localScale = Vector3.one;
            traceInst.SetActive(false);
            Launcher = traceInst.GetComponent<TraceLauncher>();
            return true;
        }

        var projPrefab = m_ProjectilePrefab.GetComponent<AProjectile>();
        if (projPrefab != null)
        {
            var projLauncher = ScriptableObject.CreateInstance(typeof(ProjectileLauncher)) as ProjectileLauncher;
            projLauncher.ProjectilePrefab = m_ProjectilePrefab;
            Launcher = projLauncher;
            return true;
        }

        Launcher = null;
        return false;
    }

	public virtual bool	LaunchProjectiles( float spread, Transform startTransform ) {
		for (int ix = 0; ix < NumProjectiles; ++ix) {
			if( !Launcher.Launch(spread, startTransform) ) {
				return false;
			}
		}

		return true;
	}

    public virtual void LaunchCasing(Transform launchPt, Vector3 launchDir)
    {
        if (m_CasingPrefab == null) {
            return;
        }

        var casingGO = GameObject.Instantiate(m_CasingPrefab, launchPt.position, launchPt.rotation);
        var rb = casingGO.GetComponent<Rigidbody>();
        rb.AddRelativeForce(launchDir);
    }
}