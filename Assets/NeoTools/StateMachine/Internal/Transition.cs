using System;
using System.Collections.Generic;
using Neo.Utility;
using Neo.Utility.Extensions;
using Neo.StateMachine.Internal;

namespace Neo.StateMachine {    
    public class Transition<TOwner> where TOwner : class, IStateMachineOwner {
        //[Save]
        protected 	List< State<TOwner> > m_NextStates = new List< State<TOwner> >();
        public List<State<TOwner>> NextStates {
            get {
                return m_NextStates;
            }
        }

        protected 	Dictionary< StaticString, ATransitionConditional<TOwner> >	m_Conditionals = new Dictionary< StaticString, ATransitionConditional<TOwner> >();
        protected   static Dictionary<StaticString, bool>   m_Literals = new Dictionary<StaticString, bool>();
	    protected	TransitionProgram<TOwner>	m_Program = new TransitionProgram<TOwner>();

        public 	TOwner	Owner {
            get;
            protected set;
        }

#pragma warning disable 0414
        private readonly string  Id = "";
#pragma warning restore 0414

        static Transition() {
            m_Literals.Add( new StaticString("true"), true );
            m_Literals.Add( new StaticString("True"), true );
            m_Literals.Add( new StaticString("false"), false );
            m_Literals.Add( new StaticString("False"), false );
        }

        public Transition( string id, TOwner o ) {
            Id = id;
		    Owner = o;
        }

        public Transition( string id, TOwner o, State<TOwner> state ) {
            Id = id;
		    Owner = o;
            AddState( state );
        }
	
	    public bool	InitFromProgram( string expression ) {
		    m_Program.DeclareConditional = DeclareConditional;
		    m_Program.DereferenceConditional = DereferenceConditional;
		    return m_Program.LoadProgram( expression );
	    }
	
	    public bool	InitFromInstructions( List<string> instructions ) {
		    m_Program.DeclareConditional = DeclareConditional;
		    m_Program.DereferenceConditional = DereferenceConditional;
		    return m_Program.LoadInstructions( instructions );
	    }

        public virtual void AddState( State<TOwner> state ) {
            m_NextStates.Add( state );
        }

        public virtual State<TOwner> GetState( int index ) {
		    if( m_NextStates.Count <= 0 ) {
			    return null;
		    }
            return m_NextStates[ index ];
        }
	
	    public virtual State<TOwner> GetRandomState() {
		    if( m_NextStates.Count == 0 ) {
			    return null;
		    }

            using(var randSlip = DataStructureLibrary<System.Random>.Instance.CheckOut()) {
                int index = randSlip.Value.Next(0, m_NextStates.Count - 1);
                return m_NextStates[index];
            }
        }
	
	    public virtual void	AddConditional( ATransitionConditional<TOwner> behavior ) {
		    m_Conditionals.Add( behavior.Name, behavior );
	    }

        public virtual void StateEntered( TOwner self ) {
            foreach (var key in m_Conditionals.Keys) {
                m_Conditionals[key].StateEntered(self);
            }
        }

        public virtual void StateExited( TOwner self ) {
		    foreach( var key in m_Conditionals.Keys ) {
                m_Conditionals[key].StateExited( self );
		    }
        }

        public virtual State<TOwner> TransitionTo( TOwner self ) {
		    bool result = false;
		    if( !m_Program.Execute(ref result) || !result ) {
			    return null;
		    }
		
		    State<TOwner> state = GetRandomState();
            foreach (var key in m_Conditionals.Keys)
            {
                m_Conditionals[key].SetStateParms(state);
            }

            //Log.Object( string.Format("Transition '{0}' fired!", Id) );

            return state;
	    }
	
	    public bool	DereferenceConditional( StaticString name ) {
		    try {
                if(!m_Conditionals.ContainsKey(name))
                {
                    return m_Literals[name];
                }
			    return m_Conditionals[ name ].Evaluate();
		    } catch( KeyNotFoundException ) {
                Log.Error(string.Format("Unable to dereference behavior '{0}'", name));
            }
		
		    return false;
	    }
	
	    public void DeclareConditional( StaticString behaviorType, StaticString name, List<StaticString> args ) {
		    TransitionConditionalFactory<TOwner>.Instance.DeclareConditional( this, behaviorType, name, args );
	    }

	    public void AddConditional( StaticString behaviorName, StaticString name, ref TransitionEventDelegate del ) {
		    ATransitionConditional<TOwner> behavior = TransitionConditionalFactory<TOwner>.AllocateAndAttachEventConditional( behaviorName, name, ref del, TypeExtensions.TypeOf(del) );
		    AddConditional( behavior );
	    }
	
	    //public void AddConditional<Parm1Type>( StaticString behaviorName, StaticString name, ref TransitionEventDelegate<Parm1Type> del ) {
	    //	Delegate d = del;
	    //	ATransitionConditional<TOwner> behavior = TransitionConditionalFactory<TOwner>.AllocateAndAttachEventConditional( behaviorName, name, ref d, TypeExtensions.TypeOf(del) );
	    //	del = d as TransitionEventDelegate<Parm1Type>;
	    //	AddConditional( behavior );
	    //}
	
	    //public void AddConditional<Parm1Type, Parm2Type>( StaticString behaviorName, StaticString name, ref TransitionEventDelegate<Parm1Type, Parm2Type> del ) {
	    //	Delegate d = del;
	    //	ATransitionConditional<TOwner> behavior = TransitionConditionalFactory<TOwner>.AllocateAndAttachEventConditional( behaviorName, name, ref d, TypeExtensions.TypeOf(del) );
	    //	del = d as TransitionEventDelegate<Parm1Type, Parm2Type>;
	    //	AddConditional( behavior );
	    //}
	
	    //public void AddConditional<Parm1Type, Parm2Type, Parm3Type>( StaticString behaviorName, StaticString name, ref TransitionEventDelegate<Parm1Type, Parm2Type, Parm3Type> del ) {
	    //	Delegate d = del;
	    //	ATransitionConditional<TOwner> behavior = TransitionConditionalFactory<TOwner>.AllocateAndAttachEventConditional( behaviorName, name, ref d, TypeExtensions.TypeOf(del) );
	    //	del = d as TransitionEventDelegate<Parm1Type, Parm2Type, Parm3Type>;
	    //	AddConditional( behavior );
	    //}
    }
}