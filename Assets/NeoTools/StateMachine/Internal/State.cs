using System;
using System.Collections.Generic;
using Neo.Utility;

namespace Neo.StateMachine {
    public class State<OwnerType> where OwnerType : class, IStateMachineOwner {
        //[Save]
        protected List< Transition<OwnerType> >     m_ExitTransitionList = new List< Transition<OwnerType> >();
    	//[Save]
        protected float         m_TimeEnterState = 0.0f;
        protected float         m_DurationInState {
    		get {
    			return Clock.Time() - m_TimeEnterState;
    		}
    	}

        public Action<OwnerType, State<OwnerType>>   OnEnter;
        public Action<OwnerType, State<OwnerType>>   OnExit;
    	
    	protected Dictionary<string, TransitionPlug<float>>	m_TransitionPlugs = new Dictionary<string, TransitionPlug<float>>();

        public virtual void Enter( OwnerType self, State<OwnerType> prevState ) {
            m_TimeEnterState = Clock.Time();
    
            //Transition<OwnerType> transition = null;
            //for( int ix = 0; ix < exitTransitionList.Count; ++ix ) {
            //    transition = exitTransitionList[ix];
            //    transition.StateEntered( self );
            //}

            foreach( Transition<OwnerType> transition in m_ExitTransitionList ) {
                transition.StateEntered( self );
            }

            if (OnEnter != null)
            {
                OnEnter.Invoke(self, prevState);
            }
        }
    
        public virtual void Exit( OwnerType self, State<OwnerType> nextState ) {
            //Transition<OwnerType> transition = null;
            //for( int ix = 0; ix < exitTransitionList.Count; ++ix ) {
            //    transition = exitTransitionList[ix];
            //    transition.StateExited( self );
            //}
            foreach( Transition<OwnerType> transition in m_ExitTransitionList ) {
                transition.StateExited( self );
            }

            if (OnExit != null)
            {
                OnExit.Invoke(self, nextState);
            }
        }
    
        public State<OwnerType> AttemptStateChange( OwnerType self ) {
            State<OwnerType>    nextState = null;
    
            Transition<OwnerType> transition = null;
            for( int ix = 0; ix < m_ExitTransitionList.Count; ++ix ) {
                transition = m_ExitTransitionList[ix];
            //foreach( Transition<OwnerType> transition in exitTransitionList ) {
                nextState = transition.TransitionTo( self );
                if( nextState != null ) {
                    return nextState;
                }
            }
    
            return null;
        }
    
        public  void        AddTransition( Transition<OwnerType> transition ) {
            m_ExitTransitionList.Add( transition );
        }
    	
    	public void			AddPlug( string name, TransitionPlug<float> plug ) {
    		m_TransitionPlugs.Add( name, plug );
    	}
    	
    	public void			SetTransitionValue( string name, float val ) {
    		try {
    			m_TransitionPlugs[ name ].Value = val;
    		} catch( KeyNotFoundException e ) {
    			Log.Error( e );
    		}
    	}
    }
    
    //public class State<OwnerType, Parm1Type> : State<OwnerType> where OwnerType : class {
    //	//[Save]
    //	public Parm1Type	Parm1;
    //}
    
    //public class State<OwnerType, Parm1Type, Parm2Type> : State<OwnerType, Parm1Type> where OwnerType : class {
    //	//[Save]
    //	public Parm2Type	Parm2;
    //}
    
    //public class State<OwnerType, Parm1Type, Parm2Type, Parm3Type> : State<OwnerType, Parm1Type, Parm2Type> where OwnerType : class {
    //	//[Save]
    //	public Parm3Type	Parm3;
    //}
    
    //public class State<OwnerType, Parm1Type, Parm2Type, Parm3Type, Parm4Type> : State<OwnerType, Parm1Type, Parm2Type, Parm3Type> where OwnerType : class {
    //	//[Save]
    //	public Parm4Type	Parm4;
    //}
}