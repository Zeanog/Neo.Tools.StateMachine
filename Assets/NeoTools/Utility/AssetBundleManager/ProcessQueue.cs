using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class AProcess {
    public Action<string>   OnError;

    public virtual string Id {
        get;
        protected set;
    }

    public abstract IEnumerator Evaluate();
}

public abstract class AProcess<TResult> : AProcess {
    public Action<TResult>  OnComplete;
}

public class Process_UnityWebRequest : AProcess<UnityWebRequest> {
    public string URL {
        get {
            return Request.url;
        }
    }

    public UnityWebRequest Request {
        get;
        protected set;
    }

    public Process_UnityWebRequest( string url, Action<UnityWebRequest> onComplete, Action<string> onError ) {
        Id = url;
        OnComplete += onComplete;
        OnError += onError;
    }

    public Process_UnityWebRequest( string url, string method, Action<UnityWebRequest> onComplete, Action<string> onError ) : this(url, onComplete, onError) {
        Request = new UnityWebRequest(url, method);
    }

    public Process_UnityWebRequest( UnityWebRequest request, Action<UnityWebRequest> onComplete, Action<string> onError ) {
        Id = request.url;
        Request = request;
        OnComplete += onComplete;
        OnError += onError;
    }

    public void SetRequestHeader( string name, string val ) {
        Request.SetRequestHeader(name, val);
    }

    public override IEnumerator Evaluate() {
        yield return Request.SendWebRequest();

        if(Request.result == UnityWebRequest.Result.ProtocolError) {
            OnError?.Invoke("");
            Request.Dispose();
            yield break;
        } else if(Request.result != UnityWebRequest.Result.Success) {
            OnError?.Invoke("");
            Request.Dispose();
            yield break;
        }

        OnComplete?.Invoke(Request);
        Request.Dispose();
    }
}

public class Process_DownloadAssetBundle : Process_UnityWebRequest {
    public Process_DownloadAssetBundle( string url, Action<UnityWebRequest> onComplete, Action<string> onError ) : base(url, "GET", onComplete, onError) {
        //Request.disposeCertificateHandlerOnDispose = true;
        //Request.disposeDownloadHandlerOnDispose = true;
        //Request.disposeUploadHandlerOnDispose = true;
        //Request.timeout = 60;

        //Request.SetRequestHeader("Authorization", m_EncodedAuthorization);
    }
}

public class Process_LoadAssetBundle : AProcess<AssetBundle> {
    public string URL {
        get {
            return Request.url;
        }
    }

    public UnityWebRequest Request {
        get;
        protected set;
    }

    public Process_LoadAssetBundle( string assetBundleName, UnityWebRequest request, Action<AssetBundle> onComplete, Action<string> onError ) {
        OnComplete += onComplete;
        OnError = onError;
        
        Id = assetBundleName;
        Request = request;
    }

    public override IEnumerator Evaluate() {
        yield return Request.SendWebRequest();

        if(Request.result == UnityWebRequest.Result.ProtocolError) {
            OnError?.Invoke(Request.error);
            Request.Dispose();
            yield break;
        } else if(Request.result != UnityWebRequest.Result.Success) {
            OnError?.Invoke(Request.error);
            Request.Dispose();
            yield break;
        }

        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(Request);
        OnComplete?.Invoke(bundle);
        Request.Dispose();
    }
}

public class Process_LoadLevelFromBundle : AProcess {
    public Action                                       OnComplete;

    protected string                                    m_AssetBundleName;
    protected string                                    m_LevelName;
    protected UnityEngine.SceneManagement.LoadSceneMode m_Mode;

    public Process_LoadLevelFromBundle( AssetBundle bundle, string levelName, UnityEngine.SceneManagement.LoadSceneMode mode, Action onComplete, Action<string> onError ) : this(bundle.name, levelName, mode, onComplete, onError) {
        
    }

    public Process_LoadLevelFromBundle( string assetBundleName, string levelName, UnityEngine.SceneManagement.LoadSceneMode mode, Action onComplete, Action<string> onError ) {
        OnComplete += onComplete;
        OnError += onError;
        m_AssetBundleName = assetBundleName;
        m_LevelName = levelName;
        m_Mode = mode;
    }

    public override IEnumerator Evaluate() {
        yield return LoadScene();
    }

    protected IEnumerator LoadScene() {
        yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(m_LevelName, m_Mode);
        OnComplete?.Invoke();
    }
}

#if UNITY_EDITOR
public class Process_LoadLevelFromBundleDevelopment : Process_LoadLevelFromBundle {
    public Process_LoadLevelFromBundleDevelopment( string assetBundleName, string levelName, UnityEngine.SceneManagement.LoadSceneMode mode, Action onComplete, Action<string> onError ) : base(assetBundleName, levelName, mode, onComplete, onError) {
        
    }

    public override IEnumerator Evaluate() {
        string[] levelPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(m_AssetBundleName, m_LevelName);
        if(levelPaths.Length == 0) {
            OnError?.Invoke("There is no scene with name \"" + m_LevelName + "\" in " + m_AssetBundleName);
            yield break;
        }
        yield return LoadScene();
    }
}

public class Process_LoadAssetFromBundleDevelopment<TAsset> : AProcess<TAsset> where TAsset : UnityEngine.Object {
    protected string        m_AssetBundleName;

    public Process_LoadAssetFromBundleDevelopment( string assetBundleName, string assetName, Action<TAsset> onComplete, Action<string> onError ) {
        OnComplete += onComplete;
        OnError += onError;
        Id = assetName;
        m_AssetBundleName = assetBundleName;
    }

    public override IEnumerator Evaluate() {
        string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(m_AssetBundleName, Id);
        if(assetPaths.Length == 0) {
            OnError?.Invoke("There is no asset with name \"" + Id + "\" in " + m_AssetBundleName);
            yield break;
        }

        yield return 0;

        UnityEngine.Object target = AssetDatabase.LoadMainAssetAtPath(assetPaths[0]);
        OnComplete?.Invoke(target as TAsset);
        yield break;
    }
}
#endif

public class ProcessQueue {
    public struct PendingProcess {
        public BeauRoutine.Routine  Routine;
        public AProcess             Process;
    }

    protected int           m_MaxConcurrentProcesses = 2;
    protected LinkedList<AProcess> m_Queue = new LinkedList<AProcess>();
    protected LinkedList<PendingProcess> m_PendingList = new LinkedList<PendingProcess>();

    public void Enqueue( AProcess process ) {
        m_Queue.AddLast(process);

        if(m_PendingList.Count < m_MaxConcurrentProcesses) {
            StartNextProcess();
        }
    }

    protected AProcess Dequeue() {
        AProcess p = m_Queue.Count > 0 ? m_Queue.First.Value : null;
        if(p != null) {
            m_Queue.Remove(p);
        }

        return p;
    }

    protected void StartNextProcess() {
        AProcess process = Dequeue();
        if(process != null) {
            PendingProcess pending = new PendingProcess() { Process = process };
            pending.Routine = BeauRoutine.Routine.Start(process.Evaluate()).OnComplete(delegate() {
                var node = m_PendingList.Find(delegate ( PendingProcess p ) {
                    return process.Equals(p.Process);
                });
                m_PendingList.Remove(node);
                StartNextProcess();
            });
            m_PendingList.AddLast(pending);
        }
    }

    public TProcess Find<TProcess>( string id ) where TProcess : AProcess {
        PendingProcess pending;
        if(Find(id, m_PendingList, out pending)) {
            return pending.Process as TProcess;
        }

        TProcess process = Find<TProcess>(id, m_Queue);
        if(process != null) {
            return process;
        }

        return null;
    }

    protected static TProcess Find<TProcess>( string id, LinkedList<AProcess> list ) where TProcess : AProcess {
        foreach(var p in list) {
            if(p.Id.Equals(id, StringComparison.OrdinalIgnoreCase)) {
                TProcess process = p as TProcess;
                if(process != null) {
                    return process;
                }
            }
        }

        return null;
    }

    protected static bool Find( string id, LinkedList<PendingProcess> list, out PendingProcess process ) {
        foreach(var p in list) {
            if(p.Process.Id.Equals(id, StringComparison.OrdinalIgnoreCase)) {
                process = p;
                return true;
            }
        }

        process = default(PendingProcess);
        return false;
    }

    public void CancelAll() {
        foreach(var pending in m_PendingList) {
            pending.Routine.Stop();
        }

        m_PendingList.Clear();
        m_Queue.Clear();
    }
}