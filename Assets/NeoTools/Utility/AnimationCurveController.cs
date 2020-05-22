using UnityEngine;

namespace Neo.Utility {
    [System.Serializable]
    public class AnimationCurveController {
    	public AnimationCurve	Curve;
    	public float			Duration;
    	
    	public float			StartTime {
    		get;
    		protected set;
    	}
    	
    	public float			Evaluate() {
    		if( IsDone() ) {
    			return Curve.Evaluate( 1.0f );
    		}
    		return Curve.Evaluate( Progress );
    	}
    	
    	public bool				IsDone() {
    		if( StartTime <= 0.0f ) {
    			return true;
    		}
    		
    		if( (StartTime + Duration) <= Time.time ) {
    			return true;
    		}
    		
    		return false;
    	}
    	
    	public float			Progress {
    		get {
    			return Mathf.Clamp01( (Time.time - StartTime) / Duration );
    		}
    	}
    	
    	public void				Start() {
    		Start( Time.time );
    	}
    	
    	public void				Start( float time ) {
    		StartTime = time;
    	}
    	
    	public void				Stop() {
    		StartTime = 0.0f;
    	}
    }
}