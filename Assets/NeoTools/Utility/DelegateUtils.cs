using System;

namespace Neo.Utility {
    public static class DelegateUtils {
    	public static void	Call( Action action ) {
    		if( action == null ) {
    			return;
    		}
    		
    		action();
    	}
    	
    	public static void	Call<TParam>( Action<TParam> action, TParam param ) {
    		if( action == null ) {
    			return;
    		}
    		
    		action( param );
    	}
    	
    	public static void	Call<TParam1, TParam2>( Action<TParam1, TParam2> action, TParam1 param1, TParam2 param2 ) {
    		if( action == null ) {
    			return;
    		}
    		
    		action( param1, param2 );
    	}
    	
    	public static void	Call<TParam1, TParam2, TParam3>( Action<TParam1, TParam2, TParam3> action, TParam1 param1, TParam2 param2, TParam3 param3 ) {
    		if( action == null ) {
    			return;
    		}
    		
    		action( param1, param2, param3 );
    	}
    	
    	public static TResult	Call<TResult>( Func<TResult> func ) {
    		if( func == null ) {
    			return default(TResult);
    		}
    		
    		return func();
    	}
    	
    	public static TResult	Call<TResult, TParam1>( Func<TParam1, TResult> func, TParam1 param1 ) {
    		if( func == null ) {
    			return default(TResult);
    		}
    		
    		return func( param1 );
    	}
    	
    	public static TResult	Call<TResult, TParam1, TParam2>( Func<TParam1, TParam2, TResult> func, TParam1 param1, TParam2 param2 ) {
    		if( func == null ) {
    			return default(TResult);
    		}
    		
    		return func( param1, param2 );
    	}
    	
    	public static TResult	Call<TResult, TParam1, TParam2, TParam3>( Func<TParam1, TParam2, TParam3, TResult> func, TParam1 param1, TParam2 param2, TParam3 param3 ) {
    		if( func == null ) {
    			return default(TResult);
    		}
    		
    		return func( param1, param2, param3 );
    	}
    }
}