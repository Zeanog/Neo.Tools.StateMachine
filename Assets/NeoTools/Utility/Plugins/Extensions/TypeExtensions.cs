using System;

namespace Neo.Utility.Extensions {
    public static class TypeExtensions {
        public static Type	TypeOf<T>( T t ) {// this lets us get the type for a null object
    		if( t != null ) {
    			return t.GetType();
    		}
    		return typeof(T);
    	}
    }
}