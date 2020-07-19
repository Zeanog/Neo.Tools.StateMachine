using System;
using System.Collections.Generic;
using System.Reflection;
using Neo.Utility;

namespace Neo.StateMachine.Internal
{
    public class TransitionConditionalFactory<TOwner> where TOwner : class, IStateMachineOwner
    {
        private static TransitionConditionalFactory<TOwner>    m_Instance = null;
        public static TransitionConditionalFactory<TOwner> Instance {
            get {
                if (m_Instance == null)
                {
                    m_Instance = new TransitionConditionalFactory<TOwner>();
                }
                return m_Instance;
            }
        }

        protected Dictionary<StaticString, StaticString>    m_Type2DeclTypeMap = new Dictionary<StaticString, StaticString>();
        protected Dictionary<StaticString, Action<Transition<TOwner>, StaticString, StaticString, List<StaticString>>>    m_ConditionalAllocators = new Dictionary<StaticString, Action<Transition<TOwner>, StaticString, StaticString, List<StaticString>>>();

        public delegate bool LiteralParserDelegate(string encodedVal, out object val);
        protected static List<LiteralParserDelegate>  m_LiteralParsers = new List<LiteralParserDelegate>();

        static TransitionConditionalFactory()
        {
            m_LiteralParsers.Add(delegate (string encodedVal, out object val)
            {
                try
                {
                    val = float.Parse(encodedVal);
                    return true;
                }
                catch (ArgumentException)
                {
                    val = null;
                    return false;
                }
            });

            m_LiteralParsers.Add(delegate (string encodedVal, out object val)
            {
                try
                {
                    val = int.Parse(encodedVal);
                    return true;
                }
                catch (ArgumentException)
                {
                    val = null;
                    return false;
                }
            });

            m_LiteralParsers.Add(delegate (string encodedVal, out object val)
            {
                try
                {
                    val = bool.Parse(encodedVal);
                    return true;
                }
                catch (ArgumentException)
                {
                    val = null;
                    return false;
                }
            });

            m_LiteralParsers.Add(delegate (string encodedVal, out object val)
            {
                try
                {
                    val = double.Parse(encodedVal);
                    return true;
                }
                catch (ArgumentException)
                {
                    val = null;
                    return false;
                }
            });
        }

        public TransitionConditionalFactory()
        {
            StaticString declType_OnDelay = new StaticString("TransitionConditional_OnDelay");
            StaticString declType_OnEvent = new StaticString("TransitionConditional_OnEvent");
            StaticString declType_OnState = new StaticString("TransitionConditional_OnState");

            m_Type2DeclTypeMap.Add(declType_OnDelay, declType_OnDelay);
            m_Type2DeclTypeMap.Add(declType_OnEvent, declType_OnEvent);
            m_Type2DeclTypeMap.Add(declType_OnState, declType_OnState);
            m_Type2DeclTypeMap.Add(new StaticString("OnDelay"), declType_OnDelay);
            m_Type2DeclTypeMap.Add(new StaticString("OnEvent"), declType_OnEvent);
            m_Type2DeclTypeMap.Add(new StaticString("OnState"), declType_OnState);

            Action<Transition<TOwner>, StaticString, StaticString, List<StaticString>> handler = delegate(Transition<TOwner> transition, StaticString behaviorDeclType, StaticString name, List<StaticString> args)
            {
                try
                {
                    Type eventType = typeof(TransitionEventDelegate);
                    TransitionEventDelegate del = null;
                    // If we want to only have one behavior per delegate we should create and cache it here
                    // And then only register it once
                    transition.AddConditional( AllocateAndAttachEventConditional(behaviorDeclType, name, ref del, eventType) );
                    //Type TOwner = transition.Owner.GetType();
                    //MethodInfo methodInfo = typeof(TOwner).GetMethod( "RegisterDelegate", BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public );
                    //methodInfo.Invoke( transition.Owner, new object[] { args[0], del } );
                    transition.Owner.RegisterEvent( args[0], del );
                }
                catch( Exception ex )
                {
                    Log.Exception( ex );
                }
            };
            m_ConditionalAllocators.Add(declType_OnEvent, handler);

            handler = delegate (Transition<TOwner> transition, StaticString behaviorDeclType, StaticString name, List<StaticString> args)
            {
                var argInstsSlip = DataStructureLibrary< List<System.Object> >.Instance.CheckOut();
                argInstsSlip.Value.Clear();

                argInstsSlip.Value.Add(name);

                try
                {
                    string behaviorQualifiedName = TransitionConditionalLibrary.Instance.FindAssemblyQualifiedName( behaviorDeclType.ToString(), 1 );

                    argInstsSlip.Value.Add(GetValue(transition.Owner, args[0].ToString()));

                    if (args.Count == 2)
                    {
                        TransitionPlug<float> plug = new TransitionPlug<float>();
                        transition.Owner.RegisterPlug(args[1], plug);
                        argInstsSlip.Value.Add(plug);
                    }
                    transition.AddConditional(AllocateNonGenericConditional(behaviorQualifiedName, argInstsSlip.Value));
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                }
                finally
                {
                    argInstsSlip.Dispose();
                }
            };
            m_ConditionalAllocators.Add(declType_OnDelay, handler);

            handler = delegate (Transition<TOwner> transition, StaticString behaviorDeclType, StaticString name, List<StaticString> args)
            {
                var argInstsSlip = DataStructureLibrary< List<System.Object> >.Instance.CheckOut();
                argInstsSlip.Value.Clear();

                argInstsSlip.Value.Add(name);

                TransitionOnStateDelegate del = transition.Owner.FindAssociatedMethod( args[0].ToString() );
                ExceptionUtility.Verify<ArgumentException>( del != null, string.Format("Unable to find method {0} for OnState {1}( {0} );", args[0].ToString(), name.ToString()) );

                argInstsSlip.Value.Add(del);

                try
                {
                    string behaviorQualifiedName = TransitionConditionalLibrary.Instance.FindAssemblyQualifiedName( behaviorDeclType.ToString(), 1 );
                    transition.AddConditional( AllocateNonGenericConditional(behaviorQualifiedName, argInstsSlip.Value) );
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                }
                finally
                {
                    argInstsSlip.Dispose();
                }
            };
            m_ConditionalAllocators.Add(declType_OnState, handler);
        }

        #region Find Conditional Args 
        public void DeclareConditional(Transition<TOwner> transition, StaticString behaviorType, StaticString name, List<StaticString> args)
        {
            StaticString behaviorDeclType = null;

            try
            {
                behaviorDeclType = m_Type2DeclTypeMap[behaviorType];
            }
            catch (KeyNotFoundException ex)
            {
                Log.Exception(ex);
                return;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return;
            }

            try
            {
                m_ConditionalAllocators[behaviorDeclType].Invoke(transition, behaviorDeclType, name, args);
            }
            catch (KeyNotFoundException ex)
            {
                Log.Exception(ex);
                return;
            }
        }

        public static System.Object GetValue(TOwner inst, string argName)
        {
            Type TOwner = inst.GetType();

            FieldInfo fieldInfo = TOwner.GetField( argName, BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance );
            if (fieldInfo != null)
            {
                return GetValue(inst, fieldInfo);
            }

            PropertyInfo propInfo = TOwner.GetProperty( argName, BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance );
            if (propInfo != null)
            {
                return GetValue(inst, propInfo);
            }

            object val = null;
            for (int ix = 0; ix < m_LiteralParsers.Count; ++ix)
            {
                if (m_LiteralParsers[ix].Invoke(argName, out val))
                {
                    return val;
                }
            }

            return null;
        }

        public static System.Object GetValue(TOwner inst, FieldInfo info)
        {
            return info.GetValue(inst);
        }

        public static System.Object GetValue(TOwner inst, PropertyInfo info)
        {
            if (!info.CanRead)
            {
                return null;
            }
            return info.GetValue(inst, null);
        }
        #endregion

        public static ATransitionConditional<TOwner> AllocateAndAttachEventConditional(StaticString behaviorName, StaticString name, ref TransitionEventDelegate delegateInst, Type delegateType)
        {
            string behaviorQualifiedName = TransitionConditionalLibrary.Instance.FindAssemblyQualifiedName( behaviorName.ToString(), delegateType.GetGenericArguments().Length + 1 );
            ATransitionConditional<TOwner> behavior = AllocateEventConditional( behaviorQualifiedName, name, delegateType );

            object[] argArray = new object[] { delegateInst };

            try
            {
                Type t = behavior.GetType();
                t.InvokeMember("AttachHandler", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, behavior, argArray);
            }
            catch (Exception e)
            {
                Log.Error("User Caught Exception(t.InvokeMember): " + e.Message);
            }

            delegateInst = argArray[0] as TransitionEventDelegate;
            return behavior;
        }

        public static ATransitionConditional<TOwner> AllocateEventConditional(string qualifiedTypeName, StaticString name, Type delegateType)
        {
            var behaviorGenericArgsSlip = DataStructureLibrary< List<Type> >.Instance.CheckOut();
            behaviorGenericArgsSlip.Value.Clear();

            try
            {
                Type t = Type.GetType( qualifiedTypeName );

                Type[] genericArgs = delegateType.GetGenericArguments();

                behaviorGenericArgsSlip.Value.Add(typeof(TOwner));
                foreach (Type type in genericArgs)
                {
                    behaviorGenericArgsSlip.Value.Add(type);
                }

                Type behaviorType = null;
                try
                {
                    behaviorType = t.MakeGenericType(behaviorGenericArgsSlip.Value.ToArray());
                }
                catch (Exception e)
                {
                    Log.Error("User Caught Exception(t.MakeGenericType): " + e.Message);
                    return null;
                }

                try
                {
                    object obj = Activator.CreateInstance( behaviorType, name );
                    return obj as ATransitionConditional<TOwner>;
                }
                catch (TargetInvocationException e)
                {
                    Log.Error("User Caught TargetInvocationException(Activator.CreateInstance - " + e.InnerException + "): " + e.Message);
                }
                catch (Exception e)
                {
                    Log.Error("User Caught Exception(Activator.CreateInstance'" + behaviorType.ToString() + "): " + e.Message);
                }
            }
            catch (Exception e)
            {
                Log.Error("User Caught Exception(1): " + e.Message);
            }
            finally
            {
                behaviorGenericArgsSlip.Dispose();
            }

            return null;
        }

        public static ATransitionConditional<TOwner> AllocateNonGenericConditional(string qualifiedTypeName, List<object> args)
        {
            var behaviorGenericArgsSlip = DataStructureLibrary< List<Type> >.Instance.CheckOut();
            behaviorGenericArgsSlip.Value.Clear();

            try
            {
                Type t = Type.GetType( qualifiedTypeName );

                behaviorGenericArgsSlip.Value.Add(typeof(TOwner));

                Type behaviorType = null;
                try
                {
                    behaviorType = t.MakeGenericType(behaviorGenericArgsSlip.Value.ToArray());
                }
                catch (Exception e)
                {
                    Log.Error("User Caught Exception(t.MakeGenericType): " + e.Message);
                    return null;
                }

                try
                {
                    object obj = Activator.CreateInstance( behaviorType, args.ToArray() );
                    return obj as ATransitionConditional<TOwner>;
                }
                catch (TargetInvocationException e)
                {
                    Log.Error("User Caught TargetInvocationException(Activator.CreateInstance): " + e.Message);
                }
                catch (Exception e)
                {
                    Log.Error("User Caught Exception(Activator.CreateInstance - Delay): " + e.Message);
                }
            }
            catch (Exception e)
            {
                Log.Error("User Caught Exception(2): " + e.Message);
            }
            finally
            {
                behaviorGenericArgsSlip.Dispose();
            }

            return null;
        }
    }
}