using Neo.Utility;

namespace Neo.StateMachine.Internal {
    public class NotOperation : AOperation {
        public static void Register(OperationLibrary library)
        {
            library.Register(new StaticString("Not"), delegate ()
            {
                return new NotOperation();
            });
        }

        public override bool Parse(string[] opDesc)
        {
            return true;
        }

        public override bool Execute(TransitionInterpreter interpreter)
        {
            bool res = interpreter.DataStack.Pop();
            interpreter.DataStack.Push(!res);
            return base.Execute(interpreter);
        }
    }
}