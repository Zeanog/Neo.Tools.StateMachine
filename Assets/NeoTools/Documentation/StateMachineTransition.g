grammar StateMachineTransition;

options {
    language=CSharp2;
    output=AST;
    ASTLabelType=CommonTree;
}

tokens {
	PROG;
	PROGEXPR;
	PROGEXPR_RESTART;
	VARDECLBLOCK;
	VARDECL;
	VARTYPE;
	VARNAME;
	VARARGS;
	ARGTYPE_INT;
	ARGTYPE_FLOAT;
	ARGTYPE_DELEGATE;
}

@header {
#pragma warning disable 3021
#pragma warning disable 0414
}

@lexer::header {
#pragma warning disable 3021
#pragma warning disable 0414
}

@members {
public AstParserRuleReturnScope<CommonTree, IToken> process() {
	TreeAdaptor = new CommonTreeAdaptor();
	return prog();
}
}

@init {
}

prog	:	 (declBlock)? ignore* expr -> ^(PROG declBlock? expr)
	;

////////////////////////////////////////////////////

declBlock	:	LEFT_CBRACKET (ignore|decl)+ RIGHT_CBRACKET -> ^(VARDECLBLOCK decl+)
	;

decl	:	type ignore* name ignore* LEFT_PAREN ignore* args? RIGHT_PAREN ignore* ';' -> ^(VARDECL name type args?)
	;
	
type	:	ID -> ^(VARTYPE ID)
	;
	
name 
	:	ID -> ^(VARNAME ID)
	;
	
args	:	arg ignore* ( ',' ignore* arg ignore* )* -> ^(VARARGS arg+)
	;

arg	:	ID -> ^(ARGTYPE_DELEGATE ID)
	|	INT -> ^(ARGTYPE_INT INT)
	|	FLOAT -> ^(ARGTYPE_FLOAT FLOAT)
	;

////////////////////////////////////////////////////

expr	:	(orExpr ignore?)+ -> ^( PROGEXPR orExpr PROGEXPR_RESTART )
	; 

orExpr :	(lhs=xorExpr -> $lhs) ( ignore* OR ignore* rhs=xorExpr -> ^(OR $orExpr $rhs) )*
	;

xorExpr :	(lhs=andExpr -> $lhs) ( ignore* XOR ignore* rhs=andExpr -> ^(XOR $xorExpr $rhs) )*
	;
	
andExpr :	(lhs=term -> $lhs) ( ignore* AND ignore* rhs=term -> ^(AND $andExpr $rhs) )*
	;
	
term 	:	NOT ignore* atom -> ^( NOT atom )
	|	atom -> atom
	;
	
atom	:	ID -> ID
	| 	LEFT_PAREN ignore* orExpr RIGHT_PAREN -> orExpr
	;
	
ignore 	:	(WS|NEWLINE)
	;

LEFT_CBRACKET	:	'{';
RIGHT_CBRACKET	:	'}';
XOR	:	'^';
AND	:	'&';
OR	:	'|';
LEFT_PAREN 	:	'(';
RIGHT_PAREN 	:	')';
NOT		:	'!';
ID		:	('a'..'z'|'A'..'Z'|'_')+;
FLOAT	: 	INT '.' INT ;
INT  		: 	('0'..'9')+;
NEWLINE	:	'\r'? '\n';
WS		:	(' '|'\t')+;