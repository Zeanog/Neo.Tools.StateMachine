using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Neo.Utility {
    public static class ReflectionUtils { 
    	public static bool	ContainsAttributes( Type type, params Type[] attributes ) {
    		Attribute[] typeAttribs = type.GetCustomAttributes( true ) as Attribute[];
    		foreach( Attribute typeAttrib in typeAttribs ) {
    			foreach( Type argAttrib in attributes ) {
    				if( typeAttrib.GetType() == argAttrib ) {
    					return true;
    				}
    			}
    		}
    
    		return false;
    	}
    	
    	//Returns true if a is a child of or is the same as b
    	public static bool CompareType(Type a, Type b)
    	{
    		bool subclass = a.IsSubclassOf(b);
    		bool same = (a == b);
    		return subclass || same;	
    	}
    	
    	public static Dictionary<string, System.Type> GetChildTypes<T>()
    	{
    		return GetChildTypes(typeof(T));
    	}
    	
    	public static Dictionary<string, System.Type> GetChildTypes(System.Type baseType)
    	{
    		List<Type> types = Assembly.GetAssembly(baseType).GetTypes().Where(type => type.IsSubclassOf(baseType)).ToList();
    		Dictionary<string, System.Type> output = new Dictionary<string, System.Type>();
    		foreach(Type child in types)
    		{
    			output.Add(child.ToString(), child);
    		}
    		return output;
    	}
    
    	public static MethodInfo	FindMethodInfo( Type objType, string methodName, Type[] typeArray, BindingFlags bindingFlags ) {
    		return objType.GetMethod( methodName, bindingFlags, null, typeArray, null );
    	}
    
    	public static MethodInfo	FindMethodInfo( Type objType, string methodName, Type[] typeArray ) {
    		return FindMethodInfo( objType, methodName, typeArray, BindingFlags.Instance|BindingFlags.Public );
    	}
    
    	public static MethodInfo	FindMethodInfo( Type objType, string methodName, BindingFlags bindingFlags ) {
    		return objType.GetMethod( methodName, bindingFlags );
    	}
    
    	public static MethodInfo	FindMethodInfo( Type objType, string methodName ) {
    		return FindMethodInfo( objType, methodName, BindingFlags.Instance|BindingFlags.Public );
    	}
    
    	public static string	BuildMethodInfoError( string methodName, Type[] typeArray ) {
    		System.Text.StringBuilder builder = new System.Text.StringBuilder();
    		if( typeArray.Length > 0 ) {
    			for( int ix = 0; ix < typeArray.Length - 1; ++ix ) {
    				builder.AppendFormat( "{0}, ", typeArray[ix] );
    			}
    			builder.Append( typeArray[typeArray.Length - 1] );
    		}
    		
    		return string.Format( "{0}({1}) not found!", methodName, builder );
    	}
    
    	public static string	BuildMethodInfoError( MethodInfo info ) {
    		System.Text.StringBuilder builder = new System.Text.StringBuilder();
    		ParameterInfo[] parameters = info.GetParameters();
    		if( parameters.Length > 0 ) {
    			for( int ix = 0; ix < parameters.Length - 1; ++ix ) {
    				builder.AppendFormat( "{0}, ", parameters[ix].ParameterType );
    			}
    			builder.Append( parameters[parameters.Length - 1].ParameterType );
    		}
    		
    		return string.Format( "{0}({1}) not found!", info.Name, builder );
    	}

        public static void  EnumerateFields<TField>( object inst, System.Action<string, TField> handler ) where TField : class {
            TField fieldVal;
    
            System.Reflection.FieldInfo[] fields = inst.GetType().GetFields(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance);
            foreach( System.Reflection.FieldInfo info in fields ) {
                if( info.FieldType == typeof(TField) ) {
                    fieldVal = info.GetValue(inst) as TField;
                    handler( info.Name, fieldVal );
                }
            }
        }
    }
}