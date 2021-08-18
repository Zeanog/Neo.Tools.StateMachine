using System;
using System.Collections.Generic;
using Neo.Utility;

namespace Neo.StateMachine.Internal {
    public class OperationLibrary {
        public static readonly OperationLibrary Instance = new OperationLibrary();

        public OperationLibrary()
        {
            RegisterConditionals();
        }

        protected void RegisterConditionals()
        {
            DeclareConditionalOperation.Register(this);
            PushConditionalOperation.Register(this);
            OrOperation.Register(this);
            XOrOperation.Register(this);
            AndOperation.Register(this);
            NotOperation.Register(this);
            JumpOperation.Register(this);
            YieldOperation.Register(this);
        }

        public delegate AOperation AllocOperationDelegate();

        public void Register(StaticString name, AllocOperationDelegate alloc)
        {
            allocators[name] = alloc;
        }

        public AOperation Allocate(StaticString name)
        {
            return allocators[name]();
        }

        public AOperation Allocate(string name)
        {
            return Allocate(new StaticString(name));
        }

        protected Dictionary<StaticString, AllocOperationDelegate> allocators = new Dictionary<StaticString, AllocOperationDelegate>();
    }
}