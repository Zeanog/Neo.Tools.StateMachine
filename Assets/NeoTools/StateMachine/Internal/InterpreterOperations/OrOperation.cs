using Neo.Utility;

namespace Neo.StateMachine.Internal {
    public class OrOperation : AOperation {
        public static void Register(OperationLibrary library)
        {
            library.Register(new StaticString("Or"), delegate ()
            {
                return new OrOperation();
            });
        }

        public override bool Parse(string[] opDesc)
        {
            return true;
        }

        public override bool Execute(TransitionInterpreter interpreter)
        {
            bool lhs = interpreter.DataStack.Pop();
            bool rhs = interpreter.DataStack.Pop();
            bool res = lhs || rhs;
            interpreter.DataStack.Push(res);
            return base.Execute(interpreter);
        }
    }
}