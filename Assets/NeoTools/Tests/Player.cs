using UnityEngine;

public class Player : MonoBehaviour {
	protected	FPSGunExample	m_Gun;

	[SerializeField]
	protected float				m_Speed;

	[SerializeField]
	protected float				m_RotationalSpeed;
	
	void	Awake() {
	}
	
	void	Start() {
		m_Gun = GetComponentInChildren<FPSGunExample>();
	}
	
	void	Update() {
		Vector3 translation = Vector3.zero;
		Vector2 rotation = Vector2.zero;

		translation[0] = Input.GetAxisRaw("Horizontal");
		translation[2] = Input.GetAxisRaw("Vertical");

		if( Input.GetKey(KeyCode.C) ) {
			translation[1] = -1.0f;
		} else if( Input.GetKey(KeyCode.Space) ) {
			translation[1] = 1.0f;
		}

		rotation.x = -Input.GetAxis("Mouse Y");
		rotation.y = Input.GetAxis("Mouse X");

		translation *= m_Speed * Time.deltaTime;
		rotation *= m_RotationalSpeed * Time.deltaTime;

		transform.Translate( translation[0], translation[1], translation[2] );
		
		transform.Rotate( rotation[0], 0.0f, 0.0f);
		transform.Rotate( 0.0f, rotation[1], 0.0f);

		if( Input.GetMouseButtonDown(0) ) {
			m_Gun.StartUsing();
		} else if( Input.GetMouseButtonUp(0) ) {
			m_Gun.StopUsing();
		}

		if( Input.GetKeyDown(KeyCode.R) ) {
			m_Gun.Reload();
		}
	}
}