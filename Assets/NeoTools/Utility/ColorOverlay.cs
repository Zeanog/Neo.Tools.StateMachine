using UnityEngine;
using UnityEngine.Events;
using Neo.Utility;

namespace Neo.Utility {
    [RequireComponent(typeof(Camera))]
    public class ColorOverlay : MonoBehaviour {
        [SerializeField]
        protected Color     m_Color;
    
        [SerializeField]
        protected Shader    m_CopyShader;
        //[SerializeField]
        protected Material  m_CopyMaterial;
    
        protected Timer     m_Timer;
    
        public UnityEvent   OnFadeStart;
        public UnityEvent   OnFadeComplete;
    
        protected Vector2   m_FadeExtents;
    
        protected void Awake() {
            if( m_CopyMaterial == null ) {
                m_CopyMaterial = new Material( m_CopyShader );
            }
            m_CopyMaterial.SetColor( "_Color", m_Color );
    
            m_Timer = Timer.Create( 1.0f );
            m_Timer.OnElapsed.AddListener( OnFadeComplete.Invoke );
            m_Timer.Stop();
        }
    
        public void     FadeIn( float duration ) {
            OnFadeStart.Invoke();
    
            m_Timer.Duration = duration;
            m_Timer.Restart();
    
            m_FadeExtents = new Vector2( 1.0f, 0.0f );
        }
    
        public void     FadeOut( float duration ) {
            OnFadeStart.Invoke();
    
            m_Timer.Duration = duration;
            m_Timer.Restart();
    
            m_FadeExtents = new Vector2( 0.0f, 1.0f );
        }
    
        protected void OnRenderImage( RenderTexture source, RenderTexture destination ) {
            m_Color.a = Mathf.Lerp( m_FadeExtents.x, m_FadeExtents.y, 1.0f - (m_Timer.TimeRemaining / m_Timer.Duration) );
            m_CopyMaterial.SetColor( "_Color", m_Color );
            Graphics.Blit( source, destination, m_CopyMaterial );
        }
    }
}