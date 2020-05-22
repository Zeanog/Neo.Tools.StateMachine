using System;
using System.Collections.Generic;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using Neo.Utility;

namespace Neo.StateMachine.Internal {
public class ProgramCache {
    private static ProgramCache m_Instance= null;
    public static ProgramCache  Instance {
        get {
            if( m_Instance == null ) {
                m_Instance = new ProgramCache();
            }
            return m_Instance;
        }
    }

    protected Dictionary< StaticString, List<StaticString> > m_Cache = new Dictionary<StaticString, List<StaticString>>();

    public List<StaticString>   FindInstructions( StaticString program ) {
        try {
            return m_Cache[ program ];
        }
        catch( KeyNotFoundException ) {
            return null;
        }
    }

    public void         Register( StaticString program, List<StaticString> instructions ) {
        m_Cache.Add( program, instructions );
    }
}

public class TransitionProgram<OwnerType> where OwnerType : class {
	public TransitionProgram() {
	}
	
	public bool	LoadProgram( string expression ) {
        //List<StaticString> instructions = ProgramCache.Instance.FindInstructions( new StaticString(expression) );
        //if( instructions == null ) {
		    ANTLRStringStream input = new ANTLRStringStream( expression );
            StateMachineTransitionLexer lexer = new StateMachineTransitionLexer( input );
            CommonTokenStream tokens = new CommonTokenStream( lexer );
            StateMachineTransitionParser parser = new StateMachineTransitionParser( tokens );
		
		    CommonTree t = parser.process().Tree;
		    CommonTreeNodeStream nodes = new CommonTreeNodeStream( t );
		    StateMachineTransitionTree tree = new StateMachineTransitionTree( nodes );
		
		    tree.process();
		
		    if( tree.Instructions.Count <= 0 ) {
			    return false;
		    }
		
		    if( !LoadInstructions(tree.Instructions) ) {
			    return false;
		    }
        //} else {
        //    if( !LoadInstructions(instructions) ) {
		//	    return false;
		//    }
        //}
		
		return m_Interpreter.Execute();// Create all declared behaviors
	}
	
	public bool LoadInstructions( List<string> instructions ) {
		return m_Interpreter.InitFromInstructions( instructions );
	}

    public bool LoadInstructions( List<StaticString> instructions ) {
		return m_Interpreter.InitFromInstructions( instructions );
	}
	
	public bool LoadInstructions( string[] instructions ) {
		return m_Interpreter.InitFromInstructions( instructions );
	}
	
	public bool	Execute( ref bool result ) {
		if( !m_Interpreter.Execute() ) {
			return false;
		}
		result = m_Interpreter.DataStack.Pop();
		return true;
	}
	
	public Action<StaticString, StaticString, List<StaticString>>	DeclareConditional {
		get {
			return m_Interpreter.DeclareConditional;
		}
		
		set {
			m_Interpreter.DeclareConditional = value;
		}
	}

	public System.Func<StaticString, bool>	DereferenceConditional {
		get {
			return m_Interpreter.DereferenceConditional;
		}
		
		set {
			m_Interpreter.DereferenceConditional = value;
		}
	}
	
	protected TransitionInterpreter	m_Interpreter = new TransitionInterpreter();
}
}