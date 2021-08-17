using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Inventory {
    public class InventoryItem {
        public int Index;
        public FPSGunExample Instance;

        public bool IsValid {
            get {
                return Index >= 0;
            }
        }

        public void SwitchTo( int index, GameObject prefab, GameObject parent )
        {
            if (Instance != null)
            {
                GameObject.Destroy(Instance.gameObject);
            }

            Index = index;
            GameObject go = GameObject.Instantiate(prefab, parent.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            
            Instance = go.GetComponent<FPSGunExample>();
            go.SetActive(true);
        }
    }

    [SerializeField]
    protected List<GameObject> m_FirearmPrefabs;

    [SerializeField]
    protected int m_InitialWeaponIndex;

    public InventoryItem CurrentInventoryItem {
        get;
        protected set;
    }

    public Inventory()
    {
        CurrentInventoryItem = new InventoryItem() { Index = -1, Instance = null };
    }

    public void CycleWeapon(int direction, GameObject owner)
    {
        if (CurrentInventoryItem.IsValid)
        {
            CurrentInventoryItem.Instance.Lower();
        }

        int nextIndex = (CurrentInventoryItem.Index + direction) % m_FirearmPrefabs.Count;
        if(nextIndex < 0)
        {
            nextIndex = m_FirearmPrefabs.Count - 1;
        }

        CurrentInventoryItem.SwitchTo(nextIndex, m_FirearmPrefabs[nextIndex], owner);

        if (CurrentInventoryItem.IsValid)
        {
            CurrentInventoryItem.Instance.Raise();
        }
    }
}

public class Player : MonoBehaviour {
    //[SerializeField]
    //protected	FPSGunExample	m_Gun;

    [SerializeField]
    protected Inventory m_Inventory;

    [SerializeField]
	protected float				m_Speed;

	[SerializeField]
	protected float				m_RotationalSpeed;
	
	void	Awake() {
	}
	
	void	Start() {
        
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

		translation *= m_Speed * Time.deltaTime;
        transform.Translate(translation[0], translation[1], translation[2]);

        if (Input.GetMouseButton(1))
        {
            rotation.x = -Input.GetAxis("Mouse Y");
            rotation.y = Input.GetAxis("Mouse X");
            rotation *= m_RotationalSpeed * Time.deltaTime;

            transform.Rotate(rotation[0], 0.0f, 0.0f);
            transform.Rotate(0.0f, rotation[1], 0.0f);
        }

        if ( Input.GetMouseButtonDown(0) ) {
            if (m_Inventory.CurrentInventoryItem.IsValid)
            {
                m_Inventory.CurrentInventoryItem.Instance.StartUsing();
            }
		} else if( Input.GetMouseButtonUp(0) ) {
            if (m_Inventory.CurrentInventoryItem.IsValid)
            {
                m_Inventory.CurrentInventoryItem.Instance.StopUsing();
            }
		}

		if( Input.GetKeyDown(KeyCode.R) ) {
            if (m_Inventory.CurrentInventoryItem.IsValid)
            {
                m_Inventory.CurrentInventoryItem.Instance.Reload();
            }
		}

        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        if(mouseWheel > 0.0f)
        {
            m_Inventory.CycleWeapon(1, gameObject);
        } else if(mouseWheel < 0.0f)
        {
            m_Inventory.CycleWeapon(-1, gameObject);
        }
    }
}