using Neo.Utility;
using System;

namespace Neo.StateMachine.Internal {
    public delegate float TransitionOnDelayDelegate();

    public class TransitionConditional_OnDelay<OwnerType> : ATransitionConditional<OwnerType> where OwnerType : class, IStateMachineOwner {
        protected TransitionOnDelayDelegate m_Delay;
        protected float m_StartTime = 0.0f;

        public TransitionConditional_OnDelay(StaticString name, TransitionOnDelayDelegate onDelay) : base(name)
        {
            m_Delay = onDelay;
        }

        public TransitionConditional_OnDelay(StaticString name, float delay) : base(name)
        {
            m_Delay = delegate ()
            {
                return delay;
            };
        }

        //public TransitionConditional_OnDelay(StaticString name, float delay, TransitionPlug<float> delayPlug) : this(name, delay)
        //{
        //    delayPlug.OnSet += delegate (float d) { m_Delay = d; };
        //    delayPlug.OnGet += delegate () { return m_Delay; };
        //}

        public override void StateEntered(OwnerType self)
        {
            m_StartTime = Clock.Time();
        }

        public override void StateExited(OwnerType self)
        {
        }

        public override bool Evaluate()
        {
            float d = m_Delay == null ? 0 : m_Delay();

            System.Diagnostics.Debug.Assert(d >= 0.0f, "Can not have a negative delay value");

            return Clock.Time() >= (m_StartTime + d);
        }

        public override void SetStateParms(State<OwnerType> nextState)
        {
        }
    }
}