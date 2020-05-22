tree grammar StateMachineTransitionTree;

options {
    language=CSharp2;
    tokenVocab=StateMachineTransition;
    ASTLabelType=CommonTree;
    //output=AST;
}

@header {
#pragma warning disable 3021
#pragma warning disable 0414
using Neo.Utility;
}

@members {
public Dictionary<string, int> Labels = new Dictionary<string, int>();
public List<string>	Instructions = new List<string>();

public void process() {
	prog();
}
}

@init {
}

prog	:	^(PROG declBlock? expr)
	;
	
////////////////////////////////////////////////////

declBlock	:	^(VARDECLBLOCK decl+)
	;

decl	:	^(VARDECL n=name t=type a=args?) {
			Instructions.Add( "DeclareBehavior " + t + " " + n + " " + a );
		}
	;

type returns[ string type ]
	:	^(VARTYPE ID) {
			$type = $ID.text;
		}
	;
	
name returns[ string name ]
	:	^(VARNAME ID) {
			$name = $ID.text;
		}
	;

args returns[ string vals ]
@init {
	var builder = DataStructureLibrary<System.Text.StringBuilder>.Instance.CheckOut();
	builder.Value.Clear();
}
@after {
	vals = builder.Value.ToString().Trim();
	builder.Dispose();
}
	:	^(VARARGS (a=arg{builder.Value.AppendFormat("{0} ", a);})+ ) 
	;

arg returns[ string val ]
	:	^(ARGTYPE_DELEGATE ID) { $val = $ID.text; }
	|	^(ARGTYPE_INT INT) { $val = $INT.text; }
	|	^(ARGTYPE_FLOAT FLOAT) { $val = $FLOAT.text; }
	;

////////////////////////////////////////////////////

expr 	:
	^( PROGEXPR { Instructions.Add("Yield"); Labels.Add("Start", Instructions.Count); } opExpr PROGEXPR_RESTART { Instructions.Add( "Jump " + (Labels["Start"] - (Instructions.Count + 1)) ); } )
	;

opExpr	:	^(OR opExpr opExpr) { Instructions.Add( "Or" ); }
	|	^(XOR opExpr opExpr) { Instructions.Add( "XOr" ); }
	|	^(AND opExpr opExpr) { Instructions.Add( "And" ); }
	|	^( NOT atom ) { Instructions.Add( "Not" ); }
	|	atom
	;
	
atom	:	ID { Instructions.Add( "PushBehavior " + $ID.text ); }
	;