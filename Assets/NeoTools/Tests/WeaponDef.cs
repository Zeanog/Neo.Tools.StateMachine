using System;
using UnityEngine;

[Serializable]
public class Magazine {
    [SerializeField]
    protected int m_Capacity = 1;

    [SerializeField]
    protected int m_Count = 1;

    public int Count {
        get {
            return m_Count;
        }
    }

    public void Reload()
    {
        m_Count = m_Capacity;
    }

    public void Use()
    {
        m_Count = Mathf.Clamp(Count - 1, 0, m_Capacity);
    }

    public bool IsEmpty {
        get {
            return Count <= 0;
        }
    }

    public bool IsFull {
        get {
            return Count >= m_Capacity;
        }
    }
}

public class WeaponDef : MonoBehaviour {
    [SerializeField]
    public Shell Shell;

    [SerializeField]
    public Magazine Magazine;

    [SerializeField]
    public float RoundsPerSec;

    [SerializeField]
    public float Spread = 10.0f; // In Degrees

    public void LaunchProjectiles(Transform transform)
    {
        if (Shell.LaunchProjectiles(Spread, transform))
        {
            UseAmmo();
        }
    }

    public void UseAmmo()
    {
        Magazine.Use();
    }

    public void Reload()
    {
        Magazine.Reload();
    }

    public bool IsOutOfAmmo()
    {
        return Magazine.IsEmpty;
    }

    public float ReloadDuration {
        get {
            return 1.0f;
        }
    }

    public float UseDelay {
        get {
            return 1.0f / RoundsPerSec;
        }
    }
}