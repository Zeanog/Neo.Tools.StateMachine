namespace Neo.StateMachine.Internal {
    public abstract class AOperation {
        public abstract bool Parse(string[] opDesc);

        public virtual bool Execute(TransitionInterpreter interpreter)
        {
            ++interpreter.InstructionIndex;
            return true;
        }
    }
}