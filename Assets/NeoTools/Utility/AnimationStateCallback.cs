using UnityEngine;
using System.Collections.Generic;
using System;

public interface IAnimatorEventHandler
{
    void OnAnimEvent(string evtName, Animator aniamtor, int layerIndex);
}

public abstract class AAnimatorEventHander : MonoBehaviour, IAnimatorEventHandler
{
    protected Dictionary<string, Action<Animator, int>> animCallbackHandlers = new Dictionary<string, Action<Animator, int>>();

    protected virtual void Awake()
    {
    }

    public virtual void AddHandler( string evtName, Action<Animator, int> handler )
    {
        animCallbackHandlers.Add( evtName, handler );
    }

    public virtual void RemoveHandler(string evtName, Action<Animator, int> handler )
    {
        try
        {
            if (!animCallbackHandlers.TryGetValue(evtName, out Action<Animator, int> del))
            {
                return;
            }

            del -= handler;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public virtual void OnAnimEvent(string evtName, Animator animator, int layerIndex )
    {
        try
        {
            if(!animCallbackHandlers.TryGetValue(evtName, out Action<Animator, int> handler))
            {
                return;
            }

            handler?.Invoke(animator, layerIndex);
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }
    }
} 

public class AnimationStateCallback : StateMachineBehaviour
{
    [SerializeField]
    protected string stateName;

    [SerializeField]
    protected bool onEnter;
    protected string stateEnterEventName = "Enter";

    [SerializeField]
    protected bool onUpdate;
    protected string stateUpdateEventName = "Update";

    //[SerializeField]
    //protected ClipTransition animClip;

    [SerializeField]
    protected bool onExit;
    protected string stateExitEventName = "Exit";

    [SerializeField]
    protected bool onMove;
    protected string stateMoveEventName = "Move";

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        base.OnStateMachineEnter(animator, stateMachinePathHash);
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        base.OnStateMachineExit(animator, stateMachinePathHash);
    }

    protected void BroadcastEvent(string evtName, Animator animator, int layerIndex)
    {
        if(string.IsNullOrEmpty(evtName) || string.IsNullOrEmpty(stateName))
        {
            Debug.LogErrorFormat("StateName property for {0} can not be empty!", animator.name);
            return; 
        }

        using (var slip = Neo.Utility.DataStructureLibrary<System.Text.StringBuilder>.Instance.CheckOut())
        {
            slip.Value.Clear();

            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(stateName));
            slip.Value.AppendFormat("{0}.{1}", stateName, evtName);

            var animHandler = animator.transform.parent.gameObject.GetComponent<AAnimatorEventHander>();
            animHandler?.OnAnimEvent(slip.Value.ToString(), animator, layerIndex);
        }
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (onEnter)
        {
            BroadcastEvent(stateEnterEventName, animator, layerIndex);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (onUpdate)
        {
            BroadcastEvent(stateUpdateEventName, animator, layerIndex);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (onExit)
        {
            BroadcastEvent(stateExitEventName, animator, layerIndex);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (onMove)
        {
            BroadcastEvent(stateMoveEventName, animator, layerIndex);
        }
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
