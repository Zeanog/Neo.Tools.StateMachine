//using UnityEngine;
using System;
using System.Collections.Generic;

namespace Neo.Utility {
    internal class StringPool {
    	protected Dictionary<int, string>	m_Pool = new Dictionary<int, string>();
    	
    	public int	Register( string str ) {
    		int hash = str.GetHashCode();
    		try {
    			m_Pool.Add( hash, str );
    		} catch( ArgumentException ) {
    			
    		}
    		return hash;
    	}
    
    	public string	Get( int hash ) {
    		try {
    			return m_Pool[ hash ];
    		}
    		catch( KeyNotFoundException ) {
    			//Debug.LogError(  );
    			return null;
    		}
    	}
    }
    
    // Need to inherit from all the same interfaces as String
    public class StaticString : IComparable, IComparable<string>, IComparable<StaticString>, IEquatable<string> {
    	internal static StringPool	m_StringPool = new StringPool();
    	protected int	m_Hash = 0;
    
        public readonly static StaticString  Null = (StaticString)null;
    
    	public StaticString() {
    	}
    
    	public StaticString( string str ) {
    		m_Hash = m_StringPool.Register( str );
    	}
    
    	public StaticString( StaticString str ) {
    		m_Hash = str.m_Hash;
    	}
    
    	public override string	ToString() {
    		return m_StringPool.Get( m_Hash );
    	}
    
    	public override bool	Equals( object rhs ) {
    		if( rhs == null ) {
    			return false;
    		}
    
    		Type rhsType = rhs.GetType();
    
    		if( rhsType == typeof(StaticString) ) {
    			return Equals( (StaticString)rhs );
    		}
    
    		if( rhsType == typeof(string) ) {
    			return Equals( (string)rhs );
    		}
    
    		return false;
    	}
    
    	public bool	Equals( StaticString rhs ) {
    		if( rhs == Null ) {
    			return false;
    		}
    		return m_Hash.Equals( rhs.m_Hash );
    	}
    
    	// May want to do an actual string compare when in debug to double check that this is correct
    	public bool	Equals( string rhs ) {
    		if( rhs == null ) {
    			return false;
    		}
    		return m_Hash.Equals( rhs.GetHashCode() );
    	}
    	
    	public static bool Equals( StaticString a, string b ) {
    		if( a == Null && b == (string)null ) { 
    			return true;
    		}
    		
    		if( a == Null && b != (string)null ) {
    			return false;
    		}
    		
    		if( a != Null && b == (string)null ) {
    			return false;
    		}
    
    		return a.ToString().Equals(b);
    	}
    
    	public static bool Equals( StaticString a, StaticString b ) {
    		//if( a == Null && b == Null ) {
    		//	return true;
    		//}
    		
    		//if( a == Null && b != Null ) {
    		//	return false;
    		//}
    		
    		//if( a != Null && b == Null ) {
    		//	return false;
    		//}
    
            int lhs = 0;
            int rhs = 0;
    
            try {
    		    lhs = a.m_Hash;     
            }
            catch( Exception ) {
            }
    
            try {
    		    rhs = b.m_Hash;     
            }
            catch( Exception ) {
            }
    
            return lhs == rhs;
    	}
    
    	public static bool Equals( string a, StaticString b ) {
    		if( a == (string)null && b == Null ) {
    			return true;
    		}
    		
    		if( a == (string)null && b != Null ) {
    			return false;
    		}
    		
    		if( a != (string)null && b == Null ) {
    			return false;
    		}

            return a.Equals(b.ToString());
    	}
    
    	public override int	GetHashCode() {
    		return m_Hash;
    	}
    
    	public int CompareTo( object rhs ) {
    		if( rhs == null ) {
    			return 1;
    		}
    
    		Type rhsType = rhs.GetType();
    		if( rhsType == typeof(StaticString) ) {
    			return CompareTo( (StaticString)rhs );
    		}
    
    		if( rhsType == typeof(string) ) {
    			return CompareTo( (string)rhs );
    		}
    
    		throw new ArgumentException("Object is not a String or StaticString");
    	}
    
    	public int CompareTo( string rhs ) {
    		return m_Hash.CompareTo( rhs.GetHashCode() );
    	}
    
    	public int CompareTo( StaticString rhs ) {
    		return m_Hash.CompareTo( rhs.m_Hash );
    	}
    
    	public TypeCode		GetTypeCode() {
    		return TypeCode.String;
    	}
    
    	protected char[]	ToCharArray() {
    		return ToString().ToCharArray();
    	}
    
    	public static bool operator==( StaticString lhs, StaticString rhs ) {
    		return StaticString.Equals( lhs, rhs );
    	}
    	
    	public static bool operator!=( StaticString lhs, StaticString rhs ) {
    		return !StaticString.Equals( lhs, rhs );
    	}
    
    	public static bool operator==( StaticString lhs, string rhs ) {
    		return StaticString.Equals( lhs, rhs );
    	}
    	
    	public static bool operator!=( StaticString lhs, string rhs ) {
    		return !StaticString.Equals( lhs, rhs );
    	}
    	
    	public static bool operator==( string a, StaticString b ) {
    		return StaticString.Equals( a, b );
    	}
    	
    	public static bool operator!=( string a, StaticString b ) {
    		return !StaticString.Equals( a, b );
    	}
    }
}