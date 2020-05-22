using Neo.Utility;
//using Neo.Extensions;
using Neo.StateMachine.Internal;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace Neo.StateMachine {
    public interface IStateMachineOwner {
        void  RegisterEvent( StaticString key, TransitionEventDelegate d );
        void  TriggerEvent( string name );

        void  AddAssociation( System.Object obj );
        void  RemoveAssociation(System.Object obj);
        TransitionOnStateDelegate FindAssociatedMethod( string methodName );

        void  RegisterPlug(StaticString name, TransitionPlug<float> plug);
        TransitionPlug<float> FindPlug(StaticString name);
    }

    public class StateMachine<OwnerType> where OwnerType : class, IStateMachineOwner {
        protected OwnerType         m_Owner;

        //[Save]
        public State<OwnerType>    CurrentState;
        //[Save]
        public State<OwnerType>    PreviousState;

        public Action<State<OwnerType>, State<OwnerType>>   OnStateChange;

        public StateMachine( OwnerType owner ) {
            m_Owner = owner;
            CurrentState = null;
            PreviousState = null;

            AddAssociation(m_Owner);
        }
 
        public void ChangeState( State<OwnerType> nextState, OwnerType self ) {
            if( CurrentState != null ) {
                CurrentState.Exit( self, nextState );
            }
    
            PreviousState = CurrentState;
            CurrentState = nextState;
    
            if( CurrentState != null ) {
                CurrentState.Enter( self, PreviousState );
            }

            if (OnStateChange != null)
            {
                OnStateChange.Invoke(nextState, PreviousState);
            }
        }
    
        public void    Evaluate() {// Called by owner
            if( CurrentState == null ) {
                return;
            }
    
            State<OwnerType> nextState = CurrentState.AttemptStateChange( m_Owner );
            if( nextState != null ) {
                ChangeState( nextState, m_Owner );
            }
        }

        protected Dictionary<string, TransitionEventDelegate>     m_DelegateMap = new Dictionary<string, TransitionEventDelegate>();
        public void RegisterEvent(StaticString key, TransitionEventDelegate d)
        {
            try
            {
                m_DelegateMap[key.ToString()] += d.Invoke;
            }
            catch (KeyNotFoundException)
            {
                m_DelegateMap.Add(key.ToString(), d.Invoke);
            }
        }

        public void TriggerEvent(string name)
        {
            try
            {
                TransitionEventDelegate del = m_DelegateMap[ name ];
                del.Invoke();
                Evaluate();
            }
            catch (KeyNotFoundException)
            {
                Log.Error(string.Format("Unable to find delegate event '{0}'", name));
            }
        }

        protected List<object>     m_AssociatedObjects = new List<object>();
        public void AddAssociation(object obj)
        {
            m_AssociatedObjects.AddUnique(obj);
        }

        public void RemoveAssociation(object obj)
        {
            m_AssociatedObjects.Remove(obj);
        }

        public TransitionOnStateDelegate FindAssociatedMethod(string methodName)
        {
            MethodInfo info;

            foreach (System.Object obj in m_AssociatedObjects)
            {
                info = obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (info != null)
                {
                    return Delegate.CreateDelegate(typeof(TransitionOnStateDelegate), obj, info) as TransitionOnStateDelegate;
                }
            }

            return null;
        }

        protected Dictionary<StaticString, TransitionPlug<float>>   m_Plugs = new Dictionary<StaticString, TransitionPlug<float>>();

        public void RegisterPlug(StaticString name, TransitionPlug<float> plug)
        {
            m_Plugs.Add(name, plug);
        }

        public TransitionPlug<float> FindPlug(StaticString name)
        {
            try
            {
                return m_Plugs[name];
            }
            catch (KeyNotFoundException ex)
            {
                Log.Exception(ex);
                return null;
            }
        }
    }
}