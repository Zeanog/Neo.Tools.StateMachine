using System;

namespace Neo.Utility {
    public class Log {
        protected static Action<Exception> m_ExceptionHandler = null;
        public static void SetExceptionHandler( Action<Exception> handler ) {
            m_ExceptionHandler = handler;
        }
        public static Action<Exception> Exception {
            get {
                if( m_ExceptionHandler == null ) {
                    m_ExceptionHandler = Console.Write;
                }
                return m_ExceptionHandler;
            }
        }
    
        protected static Action<Object> m_ErrorHandler = null;
        public static void SetErrorHandler( Action<Object> handler ) {
            m_ErrorHandler = handler;
        }
        public static Action<Object> Error {
            get {
                if( m_ErrorHandler == null ) {
                    m_ErrorHandler = Console.Write;
                }
                return m_ErrorHandler;
            }
        }
    
        protected static Action<Object> m_WarningHandler = null;
        public static void SetWarningHandler( Action<Object> handler ) {
            m_WarningHandler = handler;
        }
        public static Action<Object> Warning {
            get {
                if( m_WarningHandler == null ) {
                    m_WarningHandler = Console.Write;
                }
                return m_WarningHandler;
            }
        }
    
        protected static Action<Object> m_ObjectHandler = null;
        public static void SetLogHandler( Action<Object> handler ) {
            m_ObjectHandler = handler;
        }
        public static Action<Object> Object {
            get {
                if( m_ObjectHandler == null ) {
                    m_ObjectHandler = Console.Write;
                }
                return m_ObjectHandler;
            }
        }
    }
}