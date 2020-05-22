using System;

namespace Neo.Utility {
    public static class ExceptionUtility {
    	public static void	Verify<TException>( bool exp, string comment ) where TException : Exception {
    		if( exp ) {
    			return;
    		}
    		
    		Object[] args = new object[] { comment };
    		TException exception = Activator.CreateInstance( typeof(TException), args ) as TException;
    	
    		throw exception;
    	}
    
    	public static void	Verify<TException>( bool exp ) where TException : Exception {
    		Verify<TException>( exp, "" );
    	}
    }
}