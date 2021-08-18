using Neo.Utility;

namespace Neo.StateMachine.Internal {
    public class PushConditionalOperation : AOperation {
        public static void Register(OperationLibrary library)
        {
            library.Register(new StaticString("PushBehavior"), delegate ()
            {
                return new PushConditionalOperation();
            });
        }

        public override bool Parse(string[] opDesc)
        {
            m_ConditionalName = new StaticString(opDesc[1]);
            return true;
        }

        public override bool Execute(TransitionInterpreter interpreter)
        {
            interpreter.DataStack.Push(interpreter.DereferenceConditional(m_ConditionalName));
            return base.Execute(interpreter);
        }

        protected StaticString m_ConditionalName;
    }
}