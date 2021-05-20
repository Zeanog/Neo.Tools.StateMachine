using System;
using System.Collections.Generic;
using Neo.Utility;

namespace Neo.StateMachine.Internal {
    public abstract class Operation {
        public abstract bool Parse( string[] opDesc );

        public virtual bool Execute( TransitionInterpreter interpreter )
        {
            ++interpreter.InstructionIndex;
            return true;
        }
    }

    public class DeclareConditionalOperation : Operation {
        public static void Register( OperationLibrary library ) {
            library.Register(new StaticString("DeclareBehavior"), delegate () {
                return new DeclareConditionalOperation();
            });
        }

        public override bool Parse( string[] opDesc ) {
            m_ConditionalType = new StaticString(opDesc[1]);
            m_ConditionalName = new StaticString(opDesc[2]);

            for(int ix = 3; ix < opDesc.Length; ++ix) {
                m_ConditionalArgs.Add(new StaticString(opDesc[ix]));
            }
            return true;
        }

        public override bool Execute( TransitionInterpreter interpreter) {
            interpreter.DeclareConditional(m_ConditionalType, m_ConditionalName, m_ConditionalArgs);
            return base.Execute(interpreter);
        }

        protected StaticString      m_ConditionalType;
        protected StaticString      m_ConditionalName;
        protected List<StaticString>    m_ConditionalArgs = new List<StaticString>();
    }

    public class PushConditionalOperation : Operation {
        public static void Register( OperationLibrary library ) {
            library.Register(new StaticString("PushBehavior"), delegate () {
                return new PushConditionalOperation();
            });
        }

        public override bool Parse( string[] opDesc ) {
            m_ConditionalName = new StaticString(opDesc[1]);
            return true;
        }

        public override bool Execute( TransitionInterpreter interpreter) {
            interpreter.DataStack.Push(interpreter.DereferenceConditional(m_ConditionalName));
            return base.Execute(interpreter);
        }

        protected StaticString  m_ConditionalName;
    }

    public class NotOperation : Operation {
        public static void Register( OperationLibrary library ) {
            library.Register(new StaticString("Not"), delegate () {
                return new NotOperation();
            });
        }

        public override bool Parse( string[] opDesc ) {
            return true;
        }

        public override bool Execute( TransitionInterpreter interpreter) {
            bool res = interpreter.DataStack.Pop();
            interpreter.DataStack.Push(!res);
            return base.Execute(interpreter);
        }
    }

    public class AndOperation : Operation {
        public static void Register( OperationLibrary library ) {
            library.Register(new StaticString("And"), delegate () {
                return new AndOperation();
            });
        }

        public override bool Parse( string[] opDesc ) {
            return true;
        }

        public override bool Execute( TransitionInterpreter interpreter ) {
            bool lhs = interpreter.DataStack.Pop();
            bool rhs = interpreter.DataStack.Pop();
            bool res = lhs && rhs;
            interpreter.DataStack.Push(res);
            return base.Execute(interpreter);
        }
    }

    public class OrOperation : Operation {
        public static void Register( OperationLibrary library ) {
            library.Register(new StaticString("Or"), delegate () {
                return new OrOperation();
            });
        }

        public override bool Parse( string[] opDesc ) {
            return true;
        }

        public override bool Execute( TransitionInterpreter interpreter ) {
            bool lhs = interpreter.DataStack.Pop();
            bool rhs = interpreter.DataStack.Pop();
            bool res = lhs || rhs;
            interpreter.DataStack.Push(res);
            return base.Execute(interpreter);
        }
    }

    public class XOrOperation : Operation {
        public static void Register( OperationLibrary library ) {
            library.Register(new StaticString("XOr"), delegate () {
                return new XOrOperation();
            });
        }

        public override bool Parse( string[] opDesc ) {
            return true;
        }

        public override bool Execute( TransitionInterpreter interpreter ) {
            bool lhs = interpreter.DataStack.Pop();
            bool rhs = interpreter.DataStack.Pop();
            bool res = lhs ^ rhs;
            interpreter.DataStack.Push(res);
            return base.Execute(interpreter);
        }
    }

    public class YieldOperation : Operation {
        public static void Register( OperationLibrary library ) {
            library.Register(new StaticString("Yield"), delegate () {
                return new YieldOperation();
            });
        }

        public override bool Parse( string[] opDesc ) {
            return true;
        }

        public override bool Execute( TransitionInterpreter interpreter ) {
            return base.Execute(interpreter) && interpreter.DataStack.Count <= 0;
        }
    }

    // Currently we are assuming that there is only one Jump and its the last instruction.  When this changes please look thru the Execute code
    public class JumpOperation : Operation {
        public static void Register( OperationLibrary library ) {
            library.Register(new StaticString("Jump"), delegate () {
                return new JumpOperation();
            });
        }

        public override bool Parse( string[] opDesc ) {
            m_JumpDistance = int.Parse(opDesc[1]);
            return true;
        }

        public override bool Execute( TransitionInterpreter interpreter ) {
            interpreter.InstructionIndex += m_JumpDistance;
            return true;
        }

        protected int   m_JumpDistance = 0;
    }

    public class OperationLibrary {
        public static readonly OperationLibrary Instance = new OperationLibrary();

        public OperationLibrary() {
            RegisterConditionals();
        }

        protected void RegisterConditionals() {
            DeclareConditionalOperation.Register(this);
            PushConditionalOperation.Register(this);
            OrOperation.Register(this);
            XOrOperation.Register(this);
            AndOperation.Register(this);
            NotOperation.Register(this);
            JumpOperation.Register(this);
            YieldOperation.Register(this);
        }

        public delegate Operation AllocOperationDelegate();

        public void Register( StaticString name, AllocOperationDelegate alloc ) {
            allocators[name] = alloc;
        }

        public Operation Allocate( StaticString name ) {
            return allocators[name]();
        }

        public Operation Allocate( string name ) {
            return Allocate(new StaticString(name));
        }

        protected   Dictionary<StaticString, AllocOperationDelegate>    allocators = new Dictionary<StaticString, AllocOperationDelegate>();
    }

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

        protected static Operation ParseInstruction( string instruction ) {
            string[] comps = null;
            Operation operation = null;

            comps = instruction.Split(' ', '\t');
            operation = OperationLibrary.Instance.Allocate(comps[0]);
            operation.Parse(comps);
            return operation;
        }

        public bool Execute() {
            Operation op = null;

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

        protected List<Operation>   m_Operations = new List<Operation>();
        public Stack<bool>          DataStack = new Stack<bool>();

        public System.Action<StaticString, StaticString, List<StaticString>>    DeclareConditional;
        public System.Func<StaticString, bool>      DereferenceConditional;

        internal int                InstructionIndex;
    }
}