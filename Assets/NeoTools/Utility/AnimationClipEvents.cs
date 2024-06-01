using Animancer;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationClipEvents : MonoBehaviour
{
    [SerializeField]
    protected List<ClipTransition> clips = new List<ClipTransition>();

    protected void Start()
    {
        ApplyEvents();
    }

    protected void ApplyEvents()
    {
        for (int ix = 0; ix < clips.Count; ++ix)
        {
            ApplyClipEvents(ix);
        }
    }

    protected int EncodeParameter(int clipIndex, int evtIndex)
    {
        return clipIndex << 16 | evtIndex;
    }

    protected void DecodeParameter(int encodedArg, out int clipIndex, out int evtIndex)
    {
        clipIndex = encodedArg >> 16;
        evtIndex = encodedArg & 0xFFFF;
    }

    protected void ApplyClipEvents( int clipIndex )
    {
        ClipTransition ct = clips[clipIndex];
        for (int ix = 0; ix < ct.Events.Count; ++ix)
        {
            var evt = ct.Events[ix];
            AnimationEvent av = new AnimationEvent();
            av.time = evt.normalizedTime * ct.Clip.length;
            av.intParameter = EncodeParameter( clipIndex, ix );
            av.functionName = "ExecuteEvent";
            ct.Clip.AddEvent(av);
        }
    }

    protected void ExecuteEvent(int arg)
    {
        DecodeParameter(arg, out int clipIndex, out int evtIndex);
        ClipTransition ct = clips[clipIndex];
        ct.Events[evtIndex].callback?.Invoke();
    }

    protected T LoadAsset<T>( RuntimeAnimatorController ct ) where T : RuntimeAnimatorController
    {
        return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GetAssetPath(ct));
    }

    public void Clear()
    {
        clips.Clear();
    }

    public void Rebuild()
    {
#if UNITY_EDITOR
        var animator = GetComponent<Animator>();

        var runtimeController = animator.runtimeAnimatorController;
        var runtimeControllerAsset = LoadAsset<RuntimeAnimatorController>(runtimeController);

        clips.Clear();

        AnimatorOverrideController oct = runtimeControllerAsset as AnimatorOverrideController;
        if (oct != null)
        {
            List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            oct.GetOverrides(overrides);
            foreach (var kv in overrides)
            {
                var ct = new ClipTransition() { Clip = kv.Value };
                clips.Add(ct);
            }
        }
        else
        {
            foreach (var clip in runtimeControllerAsset.animationClips)
            {
                var ct = new ClipTransition() { Clip = clip };
                clips.Add(ct);
            }
        }
    }
#endif
}