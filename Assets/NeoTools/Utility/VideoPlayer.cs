using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

namespace Neo.Utility
{
    [RequireComponent(typeof(UnityEngine.Video.VideoPlayer))]
    public class VideoPlayer : MonoBehaviour
    {
        protected UnityEngine.Video.VideoPlayer m_Self;

        protected Timer         m_DurationTimer;

        public UnityEvent       OnStart;
        //public UnityEvent       OnResume;
        public UnityEvent       OnPause;
        public UnityEvent       OnStop;
        public UnityEvent       OnComplete;

        protected void Awake()
        {
            m_Self = GetComponent<UnityEngine.Video.VideoPlayer>();
            m_DurationTimer = Timer.Create(2f);
            m_DurationTimer.OnElapsed.AddListener(OnComplete.Invoke);
        }

        public void    Play( VideoClip clip )
        {
            m_DurationTimer.Stop();

            m_Self.Stop();
            m_Self.clip = null;
            m_Self.clip = clip;
            m_Self.Play();
            m_DurationTimer.Duration = (float)m_Self.clip.length;
            m_DurationTimer.Restart();

            OnStart.Invoke();
        }

        //public void Play()
        //{
        //    m_Self.Play();
        //    m_DurationTimer.Duration = (float)m_Self.clip.length;
        //    m_DurationTimer.Restart();
        //    OnStart.Invoke();
        //}

        public void Pause()
        {
            m_Self.Pause();
            m_DurationTimer.Stop();

            OnPause.Invoke();
        }

        public void Stop()
        {
            m_Self.Stop();
            m_DurationTimer.Stop();

            OnStop.Invoke();
        }

        public bool IsPlaying {
            get {
                return m_Self.isPlaying;
            }
        }
    }
}
