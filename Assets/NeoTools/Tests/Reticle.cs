using UnityEngine;

public class Reticle : MonoBehaviour {
	[SerializeField]
	protected Texture	m_Image;

	[SerializeField]
	protected Rect		m_Rect;

	protected void	Start() {
		m_Rect = new Rect();

		m_Rect.width = 25.0f;
		m_Rect.height = 25.0f;

		m_Rect.x = (Screen.width - m_Rect.width) * 0.5f;
		m_Rect.y = (Screen.height - m_Rect.height) * 0.5f;
	}
	
	protected void	OnGUI() {
		GUI.DrawTexture( m_Rect, m_Image );
	}
}