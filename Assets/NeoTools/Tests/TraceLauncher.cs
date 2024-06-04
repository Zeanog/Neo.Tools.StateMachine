using UnityEngine;
using System;
using Neo.Utility;
using System.Collections.Generic;

public class ZDistanceSort : IComparer<RaycastHit> {
    public static ZDistanceSort Instance { get; } = new ZDistanceSort();

    public int Compare(RaycastHit lhs, RaycastHit rhs)
    {
        return lhs.distance < rhs.distance ? -1 : 1;
    }
}

[Serializable]
public class TraceLauncher : MonoBehaviour, IProjectileLauncher {
    [SerializeField]
    protected float m_Range = 100.0f;

    [HideInInspector]
    [NonSerialized]
    protected ADamageDispatcher[] m_Dispatchers = null;

    protected void Awake()
    {
        m_Dispatchers = gameObject.GetComponents<ADamageDispatcher>();
        ExceptionUtility.Verify<System.ArgumentOutOfRangeException>(m_Dispatchers.Length > 0);
    }

    public bool Launch(float spread, Transform startTransform)
    {
        ExceptionUtility.Verify<System.ArgumentNullException>(m_Range > 0.0f);
        ExceptionUtility.Verify<System.ArgumentNullException>(m_Dispatchers != null);

        Vector3 startDir = RandomEx.RandomVectorWithinCone(startTransform.forward, spread);
        RaycastHit[] hits = Physics.RaycastAll(startTransform.position, startDir, m_Range);
        if (hits == null || hits.Length <= 0)
        {
            return true;
        }

        System.Array.Sort(hits, ZDistanceSort.Instance);

        foreach (ADamageDispatcher dispatcher in m_Dispatchers)
        {
            dispatcher.Dispatch(gameObject, hits);
        }

        return true;
    }
}