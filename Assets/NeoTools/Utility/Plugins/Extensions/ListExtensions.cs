using System.Collections.Generic;

//namespace Neo.Extensions {
    public static class ListExtensions {
    	public static int	AddUnique<T>( this List<T> self, T val ) {
            int index = self.IndexOf( val );
            if( index >= 0 ) {
                return index;
            }
            
            self.Add( val );
            return self.Count - 1;
    	}
    }
//}