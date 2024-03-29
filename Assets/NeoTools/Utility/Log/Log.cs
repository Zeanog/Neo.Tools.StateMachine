﻿using System;

namespace Neo.Utility {
    public class Log {
        protected static Action<Exception> m_ExceptionHandler = Console.Write;
        public static void SetExceptionHandler( Action<Exception> handler ) {
            m_ExceptionHandler = handler;
        }
        public static Action<Exception> Exception {
            get {                
                return m_ExceptionHandler;
            }
        }
    
        protected static Action<Object> m_ErrorHandler = Console.Write;
        public static void SetErrorHandler( Action<Object> handler ) {
            m_ErrorHandler = handler;
        }
        public static Action<Object> Error {
            get {
                return m_ErrorHandler;
            }
        }
    
        protected static Action<Object> m_WarningHandler = Console.Write;
        public static void SetWarningHandler( Action<Object> handler ) {
            m_WarningHandler = handler;
        }
        public static Action<Object> Warning {
            get {
                return m_WarningHandler;
            }
        }
    
        protected static Action<Object> m_ObjectHandler = Console.Write;
        public static void SetLogHandler( Action<Object> handler ) {
            m_ObjectHandler = handler;
        }
        public static Action<Object> Object {
            get {
                return m_ObjectHandler;
            }
        }

        public static void  FormatObject(string format, params object[] objects)
        {
            Object(string.Format(format, objects));
        }
    }
}