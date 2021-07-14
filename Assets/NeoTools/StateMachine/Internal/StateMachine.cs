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
        TransitionOnStateDelegate FindAssociatedOnStateMethod( string methodName );
        TransitionOnDelayDelegate FindAssociatedOnDelayMethod(string methodName);

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

        protected Dictionary<string, TransitionOnStateDelegate> m_OnStateMethodInfoCache = new Dictionary<string, TransitionOnStateDelegate>();

        public TransitionOnStateDelegate FindAssociatedOnStateMethod(string methodName)
        {
            MethodInfo info = null;

            try
            {
                if (m_OnStateMethodInfoCache.ContainsKey(methodName))
                {
                    return m_OnStateMethodInfoCache[methodName];
                }
                else
                {
                    foreach (System.Object obj in m_AssociatedObjects)
                    {
                        info = obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                        if (info != null)
                        {
                            TransitionOnStateDelegate del = Delegate.CreateDelegate(typeof(TransitionOnStateDelegate), obj, info) as TransitionOnStateDelegate;
                            m_OnStateMethodInfoCache.Add(methodName, del);
                            return del;
                        }
                    }
                }

                return null;
            }
            catch( Exception ex )
            {
                Log.Exception(ex);
                return null;
            }
        }

        protected Dictionary<string, TransitionOnDelayDelegate> m_OnDelayMethodInfoCache = new Dictionary<string, TransitionOnDelayDelegate>();

        //Wanna be able to handle both properties and methods with same signature
        public TransitionOnDelayDelegate FindAssociatedOnDelayMethod(string methodName)
        {
            Type retType = typeof(float);
            Type[] args = new Type[0];

            try
            {
                if (m_OnDelayMethodInfoCache.ContainsKey(methodName))
                {
                    return m_OnDelayMethodInfoCache[methodName];
                }
                else
                {
                    foreach (System.Object obj in m_AssociatedObjects)
                    {
                        PropertyInfo propInfo = obj.GetType().GetProperty(methodName, retType);
                        MethodInfo info = null;
                        if (propInfo != null)
                        {
                            var accessors = propInfo.GetAccessors(true);
                            
                            //Find the getter
                            foreach ( var accessor in accessors )
                            {
                                if( accessor.ReturnType == retType )
                                {
                                    info = accessor;
                                }
                            }

                            System.Diagnostics.Debug.Assert(info != null);
                            TransitionOnDelayDelegate del = Delegate.CreateDelegate(typeof(TransitionOnDelayDelegate), obj, info) as TransitionOnDelayDelegate;
                            m_OnDelayMethodInfoCache.Add(methodName, del);
                            return del;
                        }

                        MethodInfo methodInfo = obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                        if (methodInfo != null)
                        {
                            TransitionOnDelayDelegate del = Delegate.CreateDelegate(typeof(TransitionOnDelayDelegate), obj, methodInfo) as TransitionOnDelayDelegate;
                            m_OnDelayMethodInfoCache.Add(methodName, del);
                            return del;
                        }
                    }
                }

                return null;
            }
            catch( Exception ex ) {
                Log.Exception(ex);
                return null;
            }
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