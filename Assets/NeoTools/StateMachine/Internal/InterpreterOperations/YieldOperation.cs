using Neo.Utility;

namespace Neo.StateMachine.Internal {

    public class YieldOperation : AOperation {
        public static void Register(OperationLibrary library)
        {
            library.Register(new StaticString("Yield"), delegate ()
            {
                return new YieldOperation();
            });
        }

        public override bool Parse(string[] opDesc)
        {
            return true;
        }

        public override bool Execute(TransitionInterpreter interpreter)
        {
            return base.Execute(interpreter) && interpreter.DataStack.Count <= 0;
        }
    }
}