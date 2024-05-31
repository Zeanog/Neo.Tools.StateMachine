using UnityEngine;
using System;

[System.Serializable]
public class Shell {
	[SerializeField]
	public int			NumProjectiles;

    [SerializeField]
    protected UnityEngine.GameObject m_LauncherObjectRef;//Store actual trace game object or projectile game object so serializer works

    [HideInInspector]
    public IProjectileLauncher Launcher {
        get; set;
    }

    public bool   CreateLauncher( Transform parent )
    {
        if(m_LauncherObjectRef == null)
        {
            Launcher = null;
            return false;
        }

        TraceLauncher launcher = m_LauncherObjectRef.GetComponent<TraceLauncher>();
        if (launcher != null)
        {
            GameObject traceInst = GameObject.Instantiate(m_LauncherObjectRef, Vector3.zero, Quaternion.identity, parent);
            traceInst.transform.localPosition = Vector3.zero;
            traceInst.transform.localRotation = Quaternion.identity;
            traceInst.transform.localScale = Vector3.one;
            traceInst.SetActive(false);
            Launcher = traceInst.GetComponent<TraceLauncher>();
            return true;
        }

        var projPrefab = m_LauncherObjectRef.GetComponent<AProjectile>();
        if (projPrefab != null)
        {
            var projLauncher = ScriptableObject.CreateInstance(typeof(ProjectileLauncher)) as ProjectileLauncher;
            projLauncher.ProjectilePrefab = m_LauncherObjectRef;
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
}