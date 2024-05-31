using Neo.Utility;

namespace Neo.StateMachine.Internal {
    //public delegate bool TransitionOnStateDelegate();

    public class TransitionConditional_OnExpression<OwnerType> : ATransitionConditional<OwnerType> where OwnerType : class, IStateMachineOwner {
        public TransitionConditional_OnExpression(StaticString name) : base(name)
        {
        }

        //public TransitionConditional_OnExpression(StaticString name, TransitionOnStateDelegate d) : base(name)
        //{
        //    AttachHandler(d);
        //}

        //public void AttachHandler(TransitionOnStateDelegate d)
        //{
        //    CanTransition = d;
        //}

        public override bool Evaluate()
        {
            return true;
        }

        public override void SetStateParms(State<OwnerType> nextState)
        {
        }
    }
}