using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionTestHandlers : MonoBehaviour
{
    protected float m_TestOnDelayProp = 3f;
    public float    TestOnDelayProp {
        get {
            return m_TestOnDelayProp;
        }

        protected set {
            m_TestOnDelayProp = value;
        }
    }

    public float TestOnDelayMethod() {
        return -349.0f;
    }
}
