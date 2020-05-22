//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 3.4
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// $ANTLR 3.4 C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g 2020-05-05 12:01:41

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 168, 219
// Unreachable code detected.
#pragma warning disable 162
// Missing XML comment for publicly visible type or member 'Type_or_Member'
#pragma warning disable 1591


#pragma warning disable 3021
#pragma warning disable 0414
using Neo.Utility;


using System.Collections.Generic;
using Antlr.Runtime;
using Antlr.Runtime.Misc;
using Antlr.Runtime.Tree;
using RewriteRuleITokenStream = Antlr.Runtime.Tree.RewriteRuleTokenStream;
using ConditionalAttribute = System.Diagnostics.ConditionalAttribute;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "3.4")]
[System.CLSCompliant(false)]
public partial class StateMachineTransitionTree : Antlr.Runtime.Tree.TreeParser
{
	internal static readonly string[] tokenNames = new string[] {
		"<invalid>", "<EOR>", "<DOWN>", "<UP>", "AND", "ARGTYPE_DELEGATE", "ARGTYPE_FLOAT", "ARGTYPE_INT", "FLOAT", "ID", "INT", "LEFT_CBRACKET", "LEFT_PAREN", "NEWLINE", "NOT", "OR", "PROG", "PROGEXPR", "PROGEXPR_RESTART", "RIGHT_CBRACKET", "RIGHT_PAREN", "VARARGS", "VARDECL", "VARDECLBLOCK", "VARNAME", "VARTYPE", "WS", "XOR", "','", "';'"
	};
	public const int EOF=-1;
	public const int T__28=28;
	public const int T__29=29;
	public const int AND=4;
	public const int ARGTYPE_DELEGATE=5;
	public const int ARGTYPE_FLOAT=6;
	public const int ARGTYPE_INT=7;
	public const int FLOAT=8;
	public const int ID=9;
	public const int INT=10;
	public const int LEFT_CBRACKET=11;
	public const int LEFT_PAREN=12;
	public const int NEWLINE=13;
	public const int NOT=14;
	public const int OR=15;
	public const int PROG=16;
	public const int PROGEXPR=17;
	public const int PROGEXPR_RESTART=18;
	public const int RIGHT_CBRACKET=19;
	public const int RIGHT_PAREN=20;
	public const int VARARGS=21;
	public const int VARDECL=22;
	public const int VARDECLBLOCK=23;
	public const int VARNAME=24;
	public const int VARTYPE=25;
	public const int WS=26;
	public const int XOR=27;

	#if ANTLR_DEBUG
		private static readonly bool[] decisionCanBacktrack =
			new bool[]
			{
				false, // invalid decision
				false, false, false, false, false, false
			};
	#else
		private static readonly bool[] decisionCanBacktrack = new bool[0];
	#endif
	public StateMachineTransitionTree(ITreeNodeStream input)
		: this(input, new RecognizerSharedState())
	{
	}
	public StateMachineTransitionTree(ITreeNodeStream input, RecognizerSharedState state)
		: base(input, state)
	{
		OnCreated();
	}

	public override string[] TokenNames { get { return StateMachineTransitionTree.tokenNames; } }
	public override string GrammarFileName { get { return "C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g"; } }


	public Dictionary<string, int> Labels = new Dictionary<string, int>();
	public List<string>	Instructions = new List<string>();

	public void process() {
		prog();
	}


	[Conditional("ANTLR_TRACE")]
	protected virtual void OnCreated() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule(string ruleName, int ruleIndex) {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule(string ruleName, int ruleIndex) {}

	#region Rules

	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_prog() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_prog() {}

	// $ANTLR start "prog"
	// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:28:1: prog : ^( PROG ( declBlock )? expr ) ;
	[GrammarRule("prog")]
	private void prog()
	{
		EnterRule_prog();
		EnterRule("prog", 1);
		TraceIn("prog", 1);
		try { DebugEnterRule(GrammarFileName, "prog");
		DebugLocation(28, 1);
		try
		{
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:28:6: ( ^( PROG ( declBlock )? expr ) )
			DebugEnterAlt(1);
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:28:8: ^( PROG ( declBlock )? expr )
			{
			DebugLocation(28, 8);
			DebugLocation(28, 10);
			Match(input,PROG,Follow._PROG_in_prog68); 

			Match(input, TokenTypes.Down, null); 
			DebugLocation(28, 15);
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:28:15: ( declBlock )?
			int alt1=2;
			try { DebugEnterSubRule(1);
			try { DebugEnterDecision(1, decisionCanBacktrack[1]);
			int LA1_0 = input.LA(1);

			if ((LA1_0==VARDECLBLOCK))
			{
				alt1 = 1;
			}
			} finally { DebugExitDecision(1); }
			switch (alt1)
			{
			case 1:
				DebugEnterAlt(1);
				// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:28:15: declBlock
				{
				DebugLocation(28, 15);
				PushFollow(Follow._declBlock_in_prog70);
				declBlock();
				PopFollow();


				}
				break;

			}
			} finally { DebugExitSubRule(1); }

			DebugLocation(28, 26);
			PushFollow(Follow._expr_in_prog73);
			expr();
			PopFollow();


			Match(input, TokenTypes.Up, null); 


			}

		}
		catch (RecognitionException re)
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
			TraceOut("prog", 1);
			LeaveRule("prog", 1);
			LeaveRule_prog();
	    }
	 	DebugLocation(29, 1);
		} finally { DebugExitRule(GrammarFileName, "prog"); }
		return;

	}
	// $ANTLR end "prog"


	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_declBlock() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_declBlock() {}

	// $ANTLR start "declBlock"
	// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:33:1: declBlock : ^( VARDECLBLOCK ( decl )+ ) ;
	[GrammarRule("declBlock")]
	private void declBlock()
	{
		EnterRule_declBlock();
		EnterRule("declBlock", 2);
		TraceIn("declBlock", 2);
		try { DebugEnterRule(GrammarFileName, "declBlock");
		DebugLocation(33, 1);
		try
		{
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:33:11: ( ^( VARDECLBLOCK ( decl )+ ) )
			DebugEnterAlt(1);
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:33:13: ^( VARDECLBLOCK ( decl )+ )
			{
			DebugLocation(33, 13);
			DebugLocation(33, 15);
			Match(input,VARDECLBLOCK,Follow._VARDECLBLOCK_in_declBlock88); 

			Match(input, TokenTypes.Down, null); 
			DebugLocation(33, 28);
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:33:28: ( decl )+
			int cnt2=0;
			try { DebugEnterSubRule(2);
			while (true)
			{
				int alt2=2;
				try { DebugEnterDecision(2, decisionCanBacktrack[2]);
				int LA2_0 = input.LA(1);

				if ((LA2_0==VARDECL))
				{
					alt2 = 1;
				}


				} finally { DebugExitDecision(2); }
				switch (alt2)
				{
				case 1:
					DebugEnterAlt(1);
					// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:33:28: decl
					{
					DebugLocation(33, 28);
					PushFollow(Follow._decl_in_declBlock90);
					decl();
					PopFollow();


					}
					break;

				default:
					if (cnt2 >= 1)
						goto loop2;

					EarlyExitException eee2 = new EarlyExitException( 2, input );
					DebugRecognitionException(eee2);
					throw eee2;
				}
				cnt2++;
			}
			loop2:
				;

			} finally { DebugExitSubRule(2); }


			Match(input, TokenTypes.Up, null); 


			}

		}
		catch (RecognitionException re)
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
			TraceOut("declBlock", 2);
			LeaveRule("declBlock", 2);
			LeaveRule_declBlock();
	    }
	 	DebugLocation(34, 1);
		} finally { DebugExitRule(GrammarFileName, "declBlock"); }
		return;

	}
	// $ANTLR end "declBlock"


	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_decl() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_decl() {}

	// $ANTLR start "decl"
	// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:36:1: decl : ^( VARDECL n= name t= type (a= args )? ) ;
	[GrammarRule("decl")]
	private void decl()
	{
		EnterRule_decl();
		EnterRule("decl", 3);
		TraceIn("decl", 3);
	    string n = default(string);
	    string t = default(string);
	    string a = default(string);

		try { DebugEnterRule(GrammarFileName, "decl");
		DebugLocation(36, 1);
		try
		{
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:36:6: ( ^( VARDECL n= name t= type (a= args )? ) )
			DebugEnterAlt(1);
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:36:8: ^( VARDECL n= name t= type (a= args )? )
			{
			DebugLocation(36, 8);
			DebugLocation(36, 10);
			Match(input,VARDECL,Follow._VARDECL_in_decl103); 

			Match(input, TokenTypes.Down, null); 
			DebugLocation(36, 19);
			PushFollow(Follow._name_in_decl107);
			n=name();
			PopFollow();

			DebugLocation(36, 26);
			PushFollow(Follow._type_in_decl111);
			t=type();
			PopFollow();

			DebugLocation(36, 33);
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:36:33: (a= args )?
			int alt3=2;
			try { DebugEnterSubRule(3);
			try { DebugEnterDecision(3, decisionCanBacktrack[3]);
			int LA3_0 = input.LA(1);

			if ((LA3_0==VARARGS))
			{
				alt3 = 1;
			}
			} finally { DebugExitDecision(3); }
			switch (alt3)
			{
			case 1:
				DebugEnterAlt(1);
				// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:36:33: a= args
				{
				DebugLocation(36, 33);
				PushFollow(Follow._args_in_decl115);
				a=args();
				PopFollow();


				}
				break;

			}
			} finally { DebugExitSubRule(3); }


			Match(input, TokenTypes.Up, null); 

			DebugLocation(36, 41);

						Instructions.Add( "DeclareBehavior " + t + " " + n + " " + a );
					

			}

		}
		catch (RecognitionException re)
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
			TraceOut("decl", 3);
			LeaveRule("decl", 3);
			LeaveRule_decl();
	    }
	 	DebugLocation(39, 1);
		} finally { DebugExitRule(GrammarFileName, "decl"); }
		return;

	}
	// $ANTLR end "decl"


	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_type() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_type() {}

	// $ANTLR start "type"
	// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:41:1: type returns [ string type ] : ^( VARTYPE ID ) ;
	[GrammarRule("type")]
	private string type()
	{
		EnterRule_type();
		EnterRule("type", 4);
		TraceIn("type", 4);
	    string type = default(string);


	    CommonTree ID1 = default(CommonTree);

		try { DebugEnterRule(GrammarFileName, "type");
		DebugLocation(41, 1);
		try
		{
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:42:2: ( ^( VARTYPE ID ) )
			DebugEnterAlt(1);
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:42:4: ^( VARTYPE ID )
			{
			DebugLocation(42, 4);
			DebugLocation(42, 6);
			Match(input,VARTYPE,Follow._VARTYPE_in_type134); 

			Match(input, TokenTypes.Down, null); 
			DebugLocation(42, 14);
			ID1=(CommonTree)Match(input,ID,Follow._ID_in_type136); 

			Match(input, TokenTypes.Up, null); 

			DebugLocation(42, 18);

						type = (ID1!=null?ID1.Text:null);
					

			}

		}
		catch (RecognitionException re)
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
			TraceOut("type", 4);
			LeaveRule("type", 4);
			LeaveRule_type();
	    }
	 	DebugLocation(45, 1);
		} finally { DebugExitRule(GrammarFileName, "type"); }
		return type;

	}
	// $ANTLR end "type"


	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_name() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_name() {}

	// $ANTLR start "name"
	// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:47:1: name returns [ string name ] : ^( VARNAME ID ) ;
	[GrammarRule("name")]
	private string name()
	{
		EnterRule_name();
		EnterRule("name", 5);
		TraceIn("name", 5);
	    string name = default(string);


	    CommonTree ID2 = default(CommonTree);

		try { DebugEnterRule(GrammarFileName, "name");
		DebugLocation(47, 1);
		try
		{
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:48:2: ( ^( VARNAME ID ) )
			DebugEnterAlt(1);
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:48:4: ^( VARNAME ID )
			{
			DebugLocation(48, 4);
			DebugLocation(48, 6);
			Match(input,VARNAME,Follow._VARNAME_in_name155); 

			Match(input, TokenTypes.Down, null); 
			DebugLocation(48, 14);
			ID2=(CommonTree)Match(input,ID,Follow._ID_in_name157); 

			Match(input, TokenTypes.Up, null); 

			DebugLocation(48, 18);

						name = (ID2!=null?ID2.Text:null);
					

			}

		}
		catch (RecognitionException re)
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
			TraceOut("name", 5);
			LeaveRule("name", 5);
			LeaveRule_name();
	    }
	 	DebugLocation(51, 1);
		} finally { DebugExitRule(GrammarFileName, "name"); }
		return name;

	}
	// $ANTLR end "name"


	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_args() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_args() {}

	// $ANTLR start "args"
	// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:53:1: args returns [ string vals ] : ^( VARARGS (a= arg )+ ) ;
	[GrammarRule("args")]
	private string args()
	{
		EnterRule_args();
		EnterRule("args", 6);
		TraceIn("args", 6);
	    string vals = default(string);


	    string a = default(string);


	    	var builder = DataStructureLibrary<System.Text.StringBuilder>.Instance.CheckOut();
	    	builder.Value.Clear();

		try { DebugEnterRule(GrammarFileName, "args");
		DebugLocation(53, 1);
		try
		{
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:62:2: ( ^( VARARGS (a= arg )+ ) )
			DebugEnterAlt(1);
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:62:4: ^( VARARGS (a= arg )+ )
			{
			DebugLocation(62, 4);
			DebugLocation(62, 6);
			Match(input,VARARGS,Follow._VARARGS_in_args185); 

			Match(input, TokenTypes.Down, null); 
			DebugLocation(62, 14);
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:62:14: (a= arg )+
			int cnt4=0;
			try { DebugEnterSubRule(4);
			while (true)
			{
				int alt4=2;
				try { DebugEnterDecision(4, decisionCanBacktrack[4]);
				int LA4_0 = input.LA(1);

				if (((LA4_0>=ARGTYPE_DELEGATE && LA4_0<=ARGTYPE_INT)))
				{
					alt4 = 1;
				}


				} finally { DebugExitDecision(4); }
				switch (alt4)
				{
				case 1:
					DebugEnterAlt(1);
					// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:62:15: a= arg
					{
					DebugLocation(62, 16);
					PushFollow(Follow._arg_in_args190);
					a=arg();
					PopFollow();

					DebugLocation(62, 20);
					builder.Value.AppendFormat("{0} ", a);

					}
					break;

				default:
					if (cnt4 >= 1)
						goto loop4;

					EarlyExitException eee4 = new EarlyExitException( 4, input );
					DebugRecognitionException(eee4);
					throw eee4;
				}
				cnt4++;
			}
			loop4:
				;

			} finally { DebugExitSubRule(4); }


			Match(input, TokenTypes.Up, null); 


			}


				vals = builder.Value.ToString().Trim();
				builder.Dispose();

		}
		catch (RecognitionException re)
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
			TraceOut("args", 6);
			LeaveRule("args", 6);
			LeaveRule_args();
	    }
	 	DebugLocation(63, 1);
		} finally { DebugExitRule(GrammarFileName, "args"); }
		return vals;

	}
	// $ANTLR end "args"


	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_arg() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_arg() {}

	// $ANTLR start "arg"
	// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:65:1: arg returns [ string val ] : ( ^( ARGTYPE_DELEGATE ID ) | ^( ARGTYPE_INT INT ) | ^( ARGTYPE_FLOAT FLOAT ) );
	[GrammarRule("arg")]
	private string arg()
	{
		EnterRule_arg();
		EnterRule("arg", 7);
		TraceIn("arg", 7);
	    string val = default(string);


	    CommonTree ID3 = default(CommonTree);
	    CommonTree INT4 = default(CommonTree);
	    CommonTree FLOAT5 = default(CommonTree);

		try { DebugEnterRule(GrammarFileName, "arg");
		DebugLocation(65, 1);
		try
		{
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:66:2: ( ^( ARGTYPE_DELEGATE ID ) | ^( ARGTYPE_INT INT ) | ^( ARGTYPE_FLOAT FLOAT ) )
			int alt5=3;
			try { DebugEnterDecision(5, decisionCanBacktrack[5]);
			switch (input.LA(1))
			{
			case ARGTYPE_DELEGATE:
				{
				alt5 = 1;
				}
				break;
			case ARGTYPE_INT:
				{
				alt5 = 2;
				}
				break;
			case ARGTYPE_FLOAT:
				{
				alt5 = 3;
				}
				break;
			default:
				{
					NoViableAltException nvae = new NoViableAltException("", 5, 0, input);
					DebugRecognitionException(nvae);
					throw nvae;
				}
			}

			} finally { DebugExitDecision(5); }
			switch (alt5)
			{
			case 1:
				DebugEnterAlt(1);
				// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:66:4: ^( ARGTYPE_DELEGATE ID )
				{
				DebugLocation(66, 4);
				DebugLocation(66, 6);
				Match(input,ARGTYPE_DELEGATE,Follow._ARGTYPE_DELEGATE_in_arg211); 

				Match(input, TokenTypes.Down, null); 
				DebugLocation(66, 23);
				ID3=(CommonTree)Match(input,ID,Follow._ID_in_arg213); 

				Match(input, TokenTypes.Up, null); 

				DebugLocation(66, 27);
				 val = (ID3!=null?ID3.Text:null); 

				}
				break;
			case 2:
				DebugEnterAlt(2);
				// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:67:4: ^( ARGTYPE_INT INT )
				{
				DebugLocation(67, 4);
				DebugLocation(67, 6);
				Match(input,ARGTYPE_INT,Follow._ARGTYPE_INT_in_arg222); 

				Match(input, TokenTypes.Down, null); 
				DebugLocation(67, 18);
				INT4=(CommonTree)Match(input,INT,Follow._INT_in_arg224); 

				Match(input, TokenTypes.Up, null); 

				DebugLocation(67, 23);
				 val = (INT4!=null?INT4.Text:null); 

				}
				break;
			case 3:
				DebugEnterAlt(3);
				// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:68:4: ^( ARGTYPE_FLOAT FLOAT )
				{
				DebugLocation(68, 4);
				DebugLocation(68, 6);
				Match(input,ARGTYPE_FLOAT,Follow._ARGTYPE_FLOAT_in_arg233); 

				Match(input, TokenTypes.Down, null); 
				DebugLocation(68, 20);
				FLOAT5=(CommonTree)Match(input,FLOAT,Follow._FLOAT_in_arg235); 

				Match(input, TokenTypes.Up, null); 

				DebugLocation(68, 27);
				 val = (FLOAT5!=null?FLOAT5.Text:null); 

				}
				break;

			}
		}
		catch (RecognitionException re)
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
			TraceOut("arg", 7);
			LeaveRule("arg", 7);
			LeaveRule_arg();
	    }
	 	DebugLocation(69, 1);
		} finally { DebugExitRule(GrammarFileName, "arg"); }
		return val;

	}
	// $ANTLR end "arg"


	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_expr() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_expr() {}

	// $ANTLR start "expr"
	// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:73:1: expr : ^( PROGEXPR opExpr PROGEXPR_RESTART ) ;
	[GrammarRule("expr")]
	private void expr()
	{
		EnterRule_expr();
		EnterRule("expr", 8);
		TraceIn("expr", 8);
		try { DebugEnterRule(GrammarFileName, "expr");
		DebugLocation(73, 1);
		try
		{
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:73:7: ( ^( PROGEXPR opExpr PROGEXPR_RESTART ) )
			DebugEnterAlt(1);
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:74:2: ^( PROGEXPR opExpr PROGEXPR_RESTART )
			{
			DebugLocation(74, 2);
			DebugLocation(74, 5);
			Match(input,PROGEXPR,Follow._PROGEXPR_in_expr254); 

			DebugLocation(74, 14);
			 Instructions.Add("Yield"); Labels.Add("Start", Instructions.Count); 

			Match(input, TokenTypes.Down, null); 
			DebugLocation(74, 86);
			PushFollow(Follow._opExpr_in_expr258);
			opExpr();
			PopFollow();

			DebugLocation(74, 93);
			Match(input,PROGEXPR_RESTART,Follow._PROGEXPR_RESTART_in_expr260); 
			DebugLocation(74, 110);
			 Instructions.Add( "Jump " + (Labels["Start"] - (Instructions.Count + 1)) ); 

			Match(input, TokenTypes.Up, null); 


			}

		}
		catch (RecognitionException re)
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
			TraceOut("expr", 8);
			LeaveRule("expr", 8);
			LeaveRule_expr();
	    }
	 	DebugLocation(75, 1);
		} finally { DebugExitRule(GrammarFileName, "expr"); }
		return;

	}
	// $ANTLR end "expr"


	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_opExpr() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_opExpr() {}

	// $ANTLR start "opExpr"
	// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:77:1: opExpr : ( ^( OR opExpr opExpr ) | ^( XOR opExpr opExpr ) | ^( AND opExpr opExpr ) | ^( NOT atom ) | atom );
	[GrammarRule("opExpr")]
	private void opExpr()
	{
		EnterRule_opExpr();
		EnterRule("opExpr", 9);
		TraceIn("opExpr", 9);
		try { DebugEnterRule(GrammarFileName, "opExpr");
		DebugLocation(77, 1);
		try
		{
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:77:8: ( ^( OR opExpr opExpr ) | ^( XOR opExpr opExpr ) | ^( AND opExpr opExpr ) | ^( NOT atom ) | atom )
			int alt6=5;
			try { DebugEnterDecision(6, decisionCanBacktrack[6]);
			switch (input.LA(1))
			{
			case OR:
				{
				alt6 = 1;
				}
				break;
			case XOR:
				{
				alt6 = 2;
				}
				break;
			case AND:
				{
				alt6 = 3;
				}
				break;
			case NOT:
				{
				alt6 = 4;
				}
				break;
			case ID:
				{
				alt6 = 5;
				}
				break;
			default:
				{
					NoViableAltException nvae = new NoViableAltException("", 6, 0, input);
					DebugRecognitionException(nvae);
					throw nvae;
				}
			}

			} finally { DebugExitDecision(6); }
			switch (alt6)
			{
			case 1:
				DebugEnterAlt(1);
				// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:77:10: ^( OR opExpr opExpr )
				{
				DebugLocation(77, 10);
				DebugLocation(77, 12);
				Match(input,OR,Follow._OR_in_opExpr275); 

				Match(input, TokenTypes.Down, null); 
				DebugLocation(77, 15);
				PushFollow(Follow._opExpr_in_opExpr277);
				opExpr();
				PopFollow();

				DebugLocation(77, 22);
				PushFollow(Follow._opExpr_in_opExpr279);
				opExpr();
				PopFollow();


				Match(input, TokenTypes.Up, null); 

				DebugLocation(77, 30);
				 Instructions.Add( "Or" ); 

				}
				break;
			case 2:
				DebugEnterAlt(2);
				// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:78:4: ^( XOR opExpr opExpr )
				{
				DebugLocation(78, 4);
				DebugLocation(78, 6);
				Match(input,XOR,Follow._XOR_in_opExpr288); 

				Match(input, TokenTypes.Down, null); 
				DebugLocation(78, 10);
				PushFollow(Follow._opExpr_in_opExpr290);
				opExpr();
				PopFollow();

				DebugLocation(78, 17);
				PushFollow(Follow._opExpr_in_opExpr292);
				opExpr();
				PopFollow();


				Match(input, TokenTypes.Up, null); 

				DebugLocation(78, 25);
				 Instructions.Add( "XOr" ); 

				}
				break;
			case 3:
				DebugEnterAlt(3);
				// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:79:4: ^( AND opExpr opExpr )
				{
				DebugLocation(79, 4);
				DebugLocation(79, 6);
				Match(input,AND,Follow._AND_in_opExpr301); 

				Match(input, TokenTypes.Down, null); 
				DebugLocation(79, 10);
				PushFollow(Follow._opExpr_in_opExpr303);
				opExpr();
				PopFollow();

				DebugLocation(79, 17);
				PushFollow(Follow._opExpr_in_opExpr305);
				opExpr();
				PopFollow();


				Match(input, TokenTypes.Up, null); 

				DebugLocation(79, 25);
				 Instructions.Add( "And" ); 

				}
				break;
			case 4:
				DebugEnterAlt(4);
				// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:80:4: ^( NOT atom )
				{
				DebugLocation(80, 4);
				DebugLocation(80, 7);
				Match(input,NOT,Follow._NOT_in_opExpr315); 

				Match(input, TokenTypes.Down, null); 
				DebugLocation(80, 11);
				PushFollow(Follow._atom_in_opExpr317);
				atom();
				PopFollow();


				Match(input, TokenTypes.Up, null); 

				DebugLocation(80, 18);
				 Instructions.Add( "Not" ); 

				}
				break;
			case 5:
				DebugEnterAlt(5);
				// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:81:4: atom
				{
				DebugLocation(81, 4);
				PushFollow(Follow._atom_in_opExpr326);
				atom();
				PopFollow();


				}
				break;

			}
		}
		catch (RecognitionException re)
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
			TraceOut("opExpr", 9);
			LeaveRule("opExpr", 9);
			LeaveRule_opExpr();
	    }
	 	DebugLocation(82, 1);
		} finally { DebugExitRule(GrammarFileName, "opExpr"); }
		return;

	}
	// $ANTLR end "opExpr"


	[Conditional("ANTLR_TRACE")]
	protected virtual void EnterRule_atom() {}
	[Conditional("ANTLR_TRACE")]
	protected virtual void LeaveRule_atom() {}

	// $ANTLR start "atom"
	// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:84:1: atom : ID ;
	[GrammarRule("atom")]
	private void atom()
	{
		EnterRule_atom();
		EnterRule("atom", 10);
		TraceIn("atom", 10);
	    CommonTree ID6 = default(CommonTree);

		try { DebugEnterRule(GrammarFileName, "atom");
		DebugLocation(84, 1);
		try
		{
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:84:6: ( ID )
			DebugEnterAlt(1);
			// C:\\Users\\abahr\\Dropbox\\Neo\\StateMachine\\Internal\\StateMachineTransitionTree.g:84:8: ID
			{
			DebugLocation(84, 8);
			ID6=(CommonTree)Match(input,ID,Follow._ID_in_atom337); 
			DebugLocation(84, 11);
			 Instructions.Add( "PushBehavior " + (ID6!=null?ID6.Text:null) ); 

			}

		}
		catch (RecognitionException re)
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
			TraceOut("atom", 10);
			LeaveRule("atom", 10);
			LeaveRule_atom();
	    }
	 	DebugLocation(85, 1);
		} finally { DebugExitRule(GrammarFileName, "atom"); }
		return;

	}
	// $ANTLR end "atom"
	#endregion Rules


	#region Follow sets
	private static class Follow
	{
		public static readonly BitSet _PROG_in_prog68 = new BitSet(new ulong[]{0x4UL});
		public static readonly BitSet _declBlock_in_prog70 = new BitSet(new ulong[]{0x20000UL});
		public static readonly BitSet _expr_in_prog73 = new BitSet(new ulong[]{0x8UL});
		public static readonly BitSet _VARDECLBLOCK_in_declBlock88 = new BitSet(new ulong[]{0x4UL});
		public static readonly BitSet _decl_in_declBlock90 = new BitSet(new ulong[]{0x400008UL});
		public static readonly BitSet _VARDECL_in_decl103 = new BitSet(new ulong[]{0x4UL});
		public static readonly BitSet _name_in_decl107 = new BitSet(new ulong[]{0x2000000UL});
		public static readonly BitSet _type_in_decl111 = new BitSet(new ulong[]{0x200008UL});
		public static readonly BitSet _args_in_decl115 = new BitSet(new ulong[]{0x8UL});
		public static readonly BitSet _VARTYPE_in_type134 = new BitSet(new ulong[]{0x4UL});
		public static readonly BitSet _ID_in_type136 = new BitSet(new ulong[]{0x8UL});
		public static readonly BitSet _VARNAME_in_name155 = new BitSet(new ulong[]{0x4UL});
		public static readonly BitSet _ID_in_name157 = new BitSet(new ulong[]{0x8UL});
		public static readonly BitSet _VARARGS_in_args185 = new BitSet(new ulong[]{0x4UL});
		public static readonly BitSet _arg_in_args190 = new BitSet(new ulong[]{0xE8UL});
		public static readonly BitSet _ARGTYPE_DELEGATE_in_arg211 = new BitSet(new ulong[]{0x4UL});
		public static readonly BitSet _ID_in_arg213 = new BitSet(new ulong[]{0x8UL});
		public static readonly BitSet _ARGTYPE_INT_in_arg222 = new BitSet(new ulong[]{0x4UL});
		public static readonly BitSet _INT_in_arg224 = new BitSet(new ulong[]{0x8UL});
		public static readonly BitSet _ARGTYPE_FLOAT_in_arg233 = new BitSet(new ulong[]{0x4UL});
		public static readonly BitSet _FLOAT_in_arg235 = new BitSet(new ulong[]{0x8UL});
		public static readonly BitSet _PROGEXPR_in_expr254 = new BitSet(new ulong[]{0x4UL});
		public static readonly BitSet _opExpr_in_expr258 = new BitSet(new ulong[]{0x40000UL});
		public static readonly BitSet _PROGEXPR_RESTART_in_expr260 = new BitSet(new ulong[]{0x8UL});
		public static readonly BitSet _OR_in_opExpr275 = new BitSet(new ulong[]{0x4UL});
		public static readonly BitSet _opExpr_in_opExpr277 = new BitSet(new ulong[]{0x800C210UL});
		public static readonly BitSet _opExpr_in_opExpr279 = new BitSet(new ulong[]{0x8UL});
		public static readonly BitSet _XOR_in_opExpr288 = new BitSet(new ulong[]{0x4UL});
		public static readonly BitSet _opExpr_in_opExpr290 = new BitSet(new ulong[]{0x800C210UL});
		public static readonly BitSet _opExpr_in_opExpr292 = new BitSet(new ulong[]{0x8UL});
		public static readonly BitSet _AND_in_opExpr301 = new BitSet(new ulong[]{0x4UL});
		public static readonly BitSet _opExpr_in_opExpr303 = new BitSet(new ulong[]{0x800C210UL});
		public static readonly BitSet _opExpr_in_opExpr305 = new BitSet(new ulong[]{0x8UL});
		public static readonly BitSet _NOT_in_opExpr315 = new BitSet(new ulong[]{0x4UL});
		public static readonly BitSet _atom_in_opExpr317 = new BitSet(new ulong[]{0x8UL});
		public static readonly BitSet _atom_in_opExpr326 = new BitSet(new ulong[]{0x2UL});
		public static readonly BitSet _ID_in_atom337 = new BitSet(new ulong[]{0x2UL});
	}
	#endregion Follow sets
}
