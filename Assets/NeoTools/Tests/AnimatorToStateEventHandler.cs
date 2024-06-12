using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EventMapPair
{
    [SerializeField]
    public string Key;

    [SerializeField]
    public string Value;
}

[UnityEngine.ExecuteAlways]
public class AnimatorToStateEventHandler : AAnimatorEventHander
{
    [SerializeField]
    protected List<EventMapPair> eventMap;

    public List<EventMapPair> EventMap => eventMap;
}

[UnityEngine.ExecuteAlways]
public class WeaponAnimatorEventHandler : AnimatorToStateEventHandler
{
    //[SerializeField]
    protected ProjectileWeapon weapon;

    protected virtual void Reset()
    {
        weapon = GetComponent<ProjectileWeapon>();

        //AddHandler("Shoot.Exit", "OnUseComplete");
        //AddHandler("Hide.Exit", "OnHideComplete");
        //AddHandler("Raise.Exit", "OnRaiseComplete");
    }

    protected void RemoveHandler(string animEvtName)
    {
        for( int ix = eventMap.Count - 1; ix >= 0; ++ix)
        {
            var kvp = eventMap[ix];
            if( kvp.Key.Equals(animEvtName, StringComparison.OrdinalIgnoreCase)) {
                eventMap.RemoveAt(ix);
                break;
            }
        }
    }

    protected void AddHandler(string animEvtName, string stateEvtName)
    {
        if (Application.isPlaying)
        {
            return;
        }
        eventMap.Add(new EventMapPair() { Key = animEvtName, Value = stateEvtName });
    }    

    protected override void Awake()
    {
        base.Awake();

        weapon = GetComponent<ProjectileWeapon>();

        foreach( var kvp in eventMap )
        {
            AddHandler(kvp.Key, (Animator animator, int layerIndex) =>
            {
                weapon.TriggerStateEvent(kvp.Value, layerIndex);
            });
        }
    }
}