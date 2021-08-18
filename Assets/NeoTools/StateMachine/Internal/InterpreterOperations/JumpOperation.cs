using Neo.Utility;

namespace Neo.StateMachine.Internal {
    // Currently we are assuming that there is only one Jump and its the last instruction.  When this changes please look thru the Execute code
    public class JumpOperation : AOperation {
        public static void Register(OperationLibrary library)
        {
            library.Register(new StaticString("Jump"), delegate ()
            {
                return new JumpOperation();
            });
        }

        public override bool Parse(string[] opDesc)
        {
            m_JumpDistance = int.Parse(opDesc[1]);
            return true;
        }

        public override bool Execute(TransitionInterpreter interpreter)
        {
            interpreter.InstructionIndex += m_JumpDistance;
            return true;
        }

        protected int m_JumpDistance = 0;
    }
}