using UnityEngine;
using UnityEngine.Events;

namespace Neo.Utility {
    public class Timer : MonoBehaviour {
        public float        Duration;
    
        public   float   TimeRemaining {
            get;
            protected set;
        }
    
        public bool         IsRunning {
            get {
                return TimeRemaining > 0.0f;
            }
        }
    
        public UnityEvent   OnRestart = new UnityEvent();
        public UnityEvent   OnResume = new UnityEvent();
        public UnityEvent   OnStop = new UnityEvent();
        public UnityEvent   OnElapsed = new UnityEvent();
    
        protected System.Action     m_OnUpdate;
    
        protected void Awake() {
    
        }
    
        protected void Start() {
    
        }
    
        protected void Update() {
            if( m_OnUpdate != null ) {
                m_OnUpdate.Invoke();
            }
        }
    
        protected void OnDestroy() {
    
        }
    
        protected void  UpdateHandler() {
            TimeRemaining -= Time.deltaTime;
    
            if( !IsRunning ) {
                Stop();
                OnElapsed.Invoke();
            }
        }
    
        public void  Restart() {
            TimeRemaining = Duration;
    
            OnRestart.Invoke();
    
            m_OnUpdate = UpdateHandler;
        }
    
        public void Resume() {
            m_OnUpdate = UpdateHandler;
            OnResume.Invoke();
        }
    
        public void Stop() {
            m_OnUpdate = null;
            TimeRemaining = 0.0f;
            OnStop.Invoke();
        }
    
        protected static GameObject m_Manager = null;
        public static Timer Create( float duration ) {
            if( !m_Manager ) {
                m_Manager = GameObject.Find( "_TimerManager" );
                if( !m_Manager ) {
                    m_Manager = new GameObject( "_TimerManager" );
                }
            }
    
            Timer self = m_Manager.AddComponent<Timer>();
            self.Duration = duration;
            return self;
        }

        public override string ToString()
        {
            return IsRunning ? TimeRemaining.ToString() : "0.0f";
        }

        public virtual string ToString( string format )
        {
            return TimeRemaining.ToString(format);
        }

    }
}