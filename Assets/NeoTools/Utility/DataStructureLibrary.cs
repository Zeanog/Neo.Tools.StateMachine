using System;
using System.Collections.Generic;

namespace Neo.Utility {
    public class DataStructureLibrary<T> where T : class, new() {
        public class CheckoutSlip : IDisposable {
            public T Value {
                get;
                protected set;
            }

            public CheckoutSlip( T data ) {
                Value = data;
            }

            public static implicit operator T( CheckoutSlip slip ) => slip.Value;

            public void Dispose() {
                Instance.Return(this);
            }

            public override string ToString() {
                return Value.ToString();
            }
        }

        public readonly static DataStructureLibrary<T>  Instance = new DataStructureLibrary<T>();

        protected LinkedList<T> available = new LinkedList<T>();
        protected LinkedList<T> checkedOut = new LinkedList<T>();

        public CheckoutSlip CheckOut() {
            if(available.Count <= 0) {
                available.AddLast(new T());
            }

            LinkedListNode<T> node = available.Last;
            available.Remove(node.Value);

            checkedOut.AddLast(node);

            return new CheckoutSlip(node.Value);
        }

        protected void Return( CheckoutSlip slip ) {
            LinkedListNode<T> node = checkedOut.Find(slip.Value);
            if(node == null) {
                slip = null;
                return;
            }

            checkedOut.Remove(node);
            available.AddLast(node);
        }

        // Hiding the contructor
        protected DataStructureLibrary() {
        }
    }
}