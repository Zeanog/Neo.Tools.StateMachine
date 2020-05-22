using Neo.Utility;

namespace Neo.StateMachine.Internal {
public delegate bool    TransitionOnStateDelegate();

public class TransitionConditional_OnState<OwnerType> : ATransitionConditional<OwnerType> where OwnerType : class, IStateMachineOwner {
	public TransitionConditional_OnState( StaticString name ) : base(name) {
    }
	
	public TransitionConditional_OnState( StaticString name, TransitionOnStateDelegate d ) : base(name) {
		AttachHandler( d );
    }
	
	public void	AttachHandler( TransitionOnStateDelegate d ) {
		CanTransition = d;
	}
	
	public override bool Evaluate() {
		return CanTransition();
	}
	
	public override void SetStateParms( State<OwnerType> nextState ) {
	}
	
	protected TransitionOnStateDelegate	CanTransition;
}
}