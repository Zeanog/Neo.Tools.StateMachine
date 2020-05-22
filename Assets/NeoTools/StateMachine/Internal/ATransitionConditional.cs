using Neo.Utility;

namespace Neo.StateMachine.Internal {
    public abstract class ATransitionConditional<OwnerType> where OwnerType : class, IStateMachineOwner {
	    public ATransitionConditional( StaticString name ) {
		    Name = name;
	    }
	
	    public virtual void StateEntered( OwnerType self ) {
	    }
	
	    public virtual void StateExited( OwnerType self ) {
	    }

	    public abstract bool Evaluate();
	
	    public abstract void SetStateParms( State<OwnerType> nextState );
	
	    public StaticString Name;
    }
}