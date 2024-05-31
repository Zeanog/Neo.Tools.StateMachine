using UnityEngine;
using System;

[Serializable]
public class ProjectileLauncher : UnityEngine.ScriptableObject, IProjectileLauncher {
    [SerializeField]
    public GameObject ProjectilePrefab {
        get;
        set;
    }

    public bool Launch(float spread, Transform startTransform)
    {
        Vector3 startDir = RandomEx.RandomVectorWithinCone(startTransform.forward, spread);
        GameObject proj = GameObject.Instantiate(ProjectilePrefab, startTransform.position, Quaternion.LookRotation(startDir)) as GameObject;
        proj.SetActive(true);
        var projComp = proj.GetComponent<AProjectile>();
        projComp.ApplyForces();

        return proj != null;
    }
}