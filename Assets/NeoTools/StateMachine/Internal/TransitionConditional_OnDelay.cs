using Neo.Utility;

namespace Neo.StateMachine.Internal {
public class TransitionConditional_OnDelay<OwnerType> : ATransitionConditional<OwnerType> where OwnerType : class, IStateMachineOwner {
    protected float	m_Delay = 0.0f;
	protected float	m_StartTime = 0.0f;

	public TransitionConditional_OnDelay( StaticString name ) : base( name ) {
	}

	public TransitionConditional_OnDelay( StaticString name, float delay ) : base( name ) {
		m_Delay = delay;
	}
	
	public TransitionConditional_OnDelay( StaticString name, float delay, TransitionPlug<float> delayPlug ) : this( name, delay ) {
		delayPlug.OnSet += delegate( float d ) { m_Delay = d; };
		delayPlug.OnGet += delegate() { return m_Delay; };
	}
	
	public override void StateEntered( OwnerType self ) {
		m_StartTime = Clock.Time();
	}
	
	public override void StateExited( OwnerType self ) {
	}

	public override bool Evaluate() {
		return Clock.Time() >= m_StartTime + m_Delay;
	}
	
	public override void SetStateParms( State<OwnerType> nextState ) {
	}
}
}