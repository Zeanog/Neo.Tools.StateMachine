using System;

namespace Neo.Utility {
    public class SystemInfo {
        private static SystemInfo   m_Instance = null;
        public static SystemInfo    Instance {
            get {
                if( m_Instance == null ) {
                    m_Instance = new SystemInfo();
                }
                return m_Instance;
            }
        }

        protected Guid    m_UUID;
        public Guid   UUID {
            get {
                return m_UUID;
            }
        }

        protected SystemInfo() {
            m_UUID = System.Guid.NewGuid();
            //IntPtr hKey;
            //if( Registry.OpenKey(Registry.HKEY_LOCAL_MACHINE, @"SOFTWARE\Microsoft\Cryptography", out hKey) ) {
            //    Registry.QueryValue( hKey, "MachineGuid", out m_UUID );
            //    Registry.CloseKey( ref hKey );
            //}
        }
    }
}