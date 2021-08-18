using System;
using System.Collections.Generic;
using Neo.Utility;

namespace Neo.StateMachine.Internal {
    public class TransitionInterpreter {
        public TransitionInterpreter() {
        }

        public bool InitFromInstructions( List<string> lines ) {
            return InitFromInstructions(lines.ToArray());
        }

        public bool InitFromInstructions( List<StaticString> lines ) {
            foreach(StaticString line in lines) {
                m_Operations.Add(ParseInstruction(line.ToString()));
            }
            return true;
        }

        public bool InitFromInstructions( string[] lines ) {
            foreach(string line in lines) {
                m_Operations.Add(ParseInstruction(line));
            }
            return true;
        }

        protected static AOperation ParseInstruction( string instruction ) {
            string[] comps = null;
            AOperation operation = null;

            comps = instruction.Split(' ', '\t');
            operation = OperationLibrary.Instance.Allocate(comps[0]);
            operation.Parse(comps);
            return operation;
        }

        public bool Execute() {
            AOperation op = null;

            try {
                while(InstructionIndex < m_Operations.Count) {
                    op = m_Operations[InstructionIndex];
                    if (!op.Execute(this)) {
                        return true;
                    }
                }

                return true;
            }
            catch(Exception ex) {
                Log.Exception(ex);
                return false;
            }
        }

        protected List<AOperation>   m_Operations = new List<AOperation>();
        public Stack<bool>          DataStack = new Stack<bool>();

        public System.Action<StaticString, StaticString, List<StaticString>>    DeclareConditional;
        public System.Func<StaticString, bool>      DereferenceConditional;

        internal int                InstructionIndex;
    }
}