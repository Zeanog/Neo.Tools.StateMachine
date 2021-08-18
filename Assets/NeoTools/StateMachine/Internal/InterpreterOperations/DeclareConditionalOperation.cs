using System.Collections.Generic;
using Neo.Utility;

namespace Neo.StateMachine.Internal {
    public class DeclareConditionalOperation : AOperation {
        public static void Register(OperationLibrary library)
        {
            library.Register(new StaticString("DeclareBehavior"), delegate ()
            {
                return new DeclareConditionalOperation();
            });
        }

        public override bool Parse(string[] opDesc)
        {
            m_ConditionalType = new StaticString(opDesc[1]);
            m_ConditionalName = new StaticString(opDesc[2]);

            for (int ix = 3; ix < opDesc.Length; ++ix)
            {
                m_ConditionalArgs.Add(new StaticString(opDesc[ix]));
            }
            return true;
        }

        public override bool Execute(TransitionInterpreter interpreter)
        {
            interpreter.DeclareConditional(m_ConditionalType, m_ConditionalName, m_ConditionalArgs);
            return base.Execute(interpreter);
        }

        protected StaticString          m_ConditionalType;
        protected StaticString          m_ConditionalName;
        protected List<StaticString>    m_ConditionalArgs = new List<StaticString>();
    }
}