using System;
using System.Collections.Generic;
using System.Text;
using Neo.Utility;

namespace Neo.StateMachine.Internal {
    public class TransitionConditionalLibrary {
        public static TransitionConditionalLibrary Instance = new TransitionConditionalLibrary();

        public TransitionConditionalLibrary() {
            Register(typeof(TransitionConditional_OnEvent<>));
            //Register( typeof(TransitionConditional_OnEvent<,>) );
            //Register( typeof(TransitionConditional_OnEvent<,,>) );
            //Register( typeof(TransitionConditional_OnEvent<,,,>) );
            Register(typeof(TransitionConditional_OnDelay<>));
            Register(typeof(TransitionConditional_OnState<>));
            Register(typeof(TransitionConditional_OnExpression<>));
        }

        protected void Register( string name, string assemblyQualifiedName ) {
            if(qualifiedNames.ContainsKey(name)) {
                return;
            }
            qualifiedNames.Add(name, assemblyQualifiedName);
        }

        protected void Register( Type type ) {
            string[] parts = type.Name.Split('`');
            Register(parts[0] + BuildEmptyGenericArgs(int.Parse(parts[1])), type.AssemblyQualifiedName.ToString());
        }

        protected string BuildEmptyGenericArgs( int numArgs ) {
            var genericParmsSlip = DataStructureLibrary<StringBuilder>.Instance.CheckOut();
            genericParmsSlip.Value.Length = 0;

            try {
                for(int ix = 0; ix < numArgs; ++ix) {
                    genericParmsSlip.Value.Append(",");
                }
                return string.Format("<{0}>", genericParmsSlip.Value.ToString());
            }
            finally {
                genericParmsSlip.Dispose();
            }
        }

        public string FindAssemblyQualifiedName( string name, int numGenericArgs ) {
            try {
                string fullName = name + BuildEmptyGenericArgs(numGenericArgs);
                return qualifiedNames[fullName];
            }
            catch(KeyNotFoundException e) {
                Log.Error(e.Message);
                return "";
            }
        }

        protected Dictionary<string, string>    qualifiedNames = new Dictionary<string, string>();
    }
}