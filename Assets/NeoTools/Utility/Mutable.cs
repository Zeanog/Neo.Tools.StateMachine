namespace Neo.Utility {
    public class Mutable<TData> {
        protected  TData   m_Current;
        protected  TData   m_Previous;
    
        public TData    Value {
            get {
                return m_Current;
            }
    
            set {
                m_Previous = m_Current;
                m_Current = value;
            }
        }
    
        public TData    PrevValue {
            get {
                return m_Previous;
            }
        }
    
        public bool     HasChanged {
            get {
                return !m_Current.Equals(m_Previous);
            }
        }
    
        public void     Clean() {
            m_Previous = m_Current;
        }
    
        public Mutable() : this( default(TData) ) {
        }
    
        public Mutable( TData val ) {
            m_Current = val;
            m_Previous = val;
        }
    }
}