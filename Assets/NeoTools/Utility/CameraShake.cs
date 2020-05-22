using UnityEngine;
using Neo.Utility.Extensions;

namespace Neo.Utility {
    [RequireComponent(typeof(Camera))]
    public class CameraShake : MonoBehaviour {
        //[SerializeField]
        //protected Camera    m_Camera;
    
        [SerializeField]
        protected Vector3       m_PositionMagnitude = Vector3.one;
    
        [SerializeField]
        protected Vector3       m_RotationMagnitude = Vector3.one;
    
        protected Vector3       m_TargetPos;
        protected Quaternion    m_TargetRot;
    
        protected void Awake() {
            End();
        }
    
        protected void OnPreRender() {
            m_TargetPos = transform.localPosition;
            m_TargetRot = transform.localRotation;
    
            Vector3 deltaPos = Vector3Extensions.Range( -m_PositionMagnitude, m_PositionMagnitude );
            transform.localPosition += deltaPos;
    
            Quaternion deltaRot = Quaternion.Euler( Vector3Extensions.Range(-m_RotationMagnitude, m_RotationMagnitude) );
            transform.localRotation *= deltaRot;
        }
    
        protected void OnPostRender() {
            transform.localPosition = m_TargetPos;
            transform.localRotation = m_TargetRot;
        }
    
        public void     Begin() {
            enabled = true;
        }
    
        public void     End() {
            enabled = false;
        }
    }
}