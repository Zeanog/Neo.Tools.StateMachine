using UnityEngine;

public static class ObjectExtensions
{
    private static System.Func<int, UnityEngine.Object> m_FindObjectFromInstanceID = null;
    static ObjectExtensions()
    {
        var methodInfo = typeof(UnityEngine.Object).GetMethod("FindObjectFromInstanceID", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        if (methodInfo == null)
            UnityEngine.Debug.LogError("FindObjectFromInstanceID was not found in UnityEngine.Object");
        else
            m_FindObjectFromInstanceID = (System.Func<int, UnityEngine.Object>)System.Delegate.CreateDelegate(typeof(System.Func<int, UnityEngine.Object>), methodInfo);
    }

    // Should put this in an ObjectExtensions
    public static Object Find(int aObjectID)
    {
        if (m_FindObjectFromInstanceID == null)
        {
            throw new System.NullReferenceException("m_FindObjectFromInstanceID == null");
        }
        return m_FindObjectFromInstanceID(aObjectID);
    }

    public static void  Destroy( Object obj )
    {
        if( Application.isEditor && !Application.isPlaying )
        {
            Object.DestroyImmediate(obj);
        } else
        {
            Object.Destroy(obj);
        }
    }
}