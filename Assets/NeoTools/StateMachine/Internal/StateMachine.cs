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
    }

    public class StateMachine<OwnerType> where OwnerType : class, IStateMachineOwner {
        protected OwnerType         m_Owner;

        //[Save]
        public State<OwnerType>    CurrentState;
        //[Save]
        public State<OwnerType>    PreviousState;

        public Action<State<OwnerType>, Transition<OwnerType>, State<OwnerType>>   OnStateChange;

        protected Queue<string> m_QueuedEvents = new Queue<string>();

        public StateMachine( OwnerType owner ) {
            m_Owner = owner;
            CurrentState = null;
            PreviousState = null;

            AddAssociation(m_Owner);
        }
 
        public void ChangeState( State<OwnerType> nextState, Transition<OwnerType> transitionUsed, OwnerType self ) {
            CurrentState?.Exit(self, nextState);

            PreviousState = CurrentState;
            CurrentState = nextState;

            CurrentState?.Enter(self, PreviousState);

            OnStateChange?.Invoke(nextState, transitionUsed, PreviousState);
        }
    
        public void    Evaluate() {// Called by owner
            if( CurrentState == null ) {
                return;
            }

            while(m_QueuedEvents.Count > 0)
            {
                TriggerEvent(m_QueuedEvents.Dequeue(), false);
            }

            Transition<OwnerType> transitionUsed;
            State<OwnerType> nextState = CurrentState.AttemptStateChange( m_Owner, out transitionUsed );
            if( nextState != null ) {
                ChangeState( nextState, transitionUsed, m_Owner );
            }
        }

        protected Dictionary<string, TransitionEventDelegate>     m_DelegateMap = new Dictionary<string, TransitionEventDelegate>();
        public int  NumEventDelegates {
            get => m_DelegateMap.Count;
        }
        public void RegisterEvent(StaticString key, TransitionEventDelegate d)
        {
            if(!m_DelegateMap.ContainsKey(key.ToString()))
            {
                m_DelegateMap.Add(key.ToString(), d.Invoke);
            }
            else
            {
                m_DelegateMap[key.ToString()] += d.Invoke;
            }
        }

        public void QueueEvent(string name)
        {
            if (!m_DelegateMap.ContainsKey(name))
            {
                m_QueuedEvents.Enqueue(name);
            }
            else
            {
                TriggerEvent(name);
            }
        }

        public void TriggerEvent(string name)
        {
            TriggerEvent(name, true);
        }

        public void TriggerEvent(string name, bool requireListener)
        {
            if(!m_DelegateMap.ContainsKey(name))
            {
                if (requireListener)
                {
                    Log.Error(string.Format("Unable to find delegate event '{0}'", name));
                }
                return;
            }

            TransitionEventDelegate del = m_DelegateMap[name];
            del.Invoke();
            Evaluate();
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
    }
}