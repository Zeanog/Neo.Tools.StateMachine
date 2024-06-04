using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using Neo.Utility;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Neo {
    public class AssetBundleManager : MonoBehaviour {
        protected static AssetBundleManager m_Instance = null;
        public static AssetBundleManager Instance {
            get {
                if(m_Instance == null) {
                    m_Instance = FindAnyObjectByType<AssetBundleManager>();
                    if(m_Instance == null) {
                        GameObject go = new GameObject("AssetBundleManager");
                        m_Instance = go.AddComponent<AssetBundleManager>();
                    }
                }
                return m_Instance;
            }
        }

        public bool DevelopmentMode = false;

        public AssetBundleManifest Manifest {
            get;
            protected set;
        }

        protected Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();

        [SerializeField]
        protected string[]      m_ActiveVariants = {  };

        public class AssetBundleRef {
            public AssetBundle  Bundle;
            public int          RefCount = 0;

            public AssetBundleRef() {
            }
        }

        protected Dictionary<string, AssetBundleRef>    m_LoadedBundles = new Dictionary<string, AssetBundleRef>();

        protected ProcessQueue                  m_ProcessQueue = new ProcessQueue();

        protected void Awake() {
            DontDestroyOnLoad(gameObject);
        }

        protected string BundleBaseLoadingURL {
            get {
                string baseUrl = "";
                if(UnityEngine.Application.isMobilePlatform || UnityEngine.Application.isConsolePlatform) {
                    baseUrl = UnityEngine.Application.streamingAssetsPath;
                } else {
                    baseUrl = UnityEngine.Application.streamingAssetsPath;
                }

                return string.Format("file://{0}", baseUrl.Replace("\\", "/"));
            }
        }

        protected void LoadManifestAsync( Action<AssetBundleManifest> onComplete, Action<string> onError ) {
            LoadAssetAsync(AssetBundles.Utility.GetPlatformName(), "AssetBundleManifest", delegate ( AssetBundleManifest m ) {
                Manifest = m;
                BuildVariantInfo();
                onComplete?.Invoke(m);
            }, onError);
        }

        protected void UnloadDependencies( string assetBundleName ) {
            try {
                string[] dependencies = m_Dependencies[assetBundleName];

                m_Dependencies.Remove(assetBundleName);

                foreach(var dependency in dependencies) {
                    UnloadAssetBundle(dependency);
                }
            }
            catch(KeyNotFoundException) { }
        }

        protected string BuildLoadURL( string assetBundleName ) {
            return System.IO.Path.Combine(BundleBaseLoadingURL, "AssetBundles", AssetBundles.Utility.GetPlatformName(), assetBundleName);
        }

        protected bool RequestAssetBundle( string assetBundleName, Action<AssetBundle> onComplete ) {
            AssetBundleRef assetBundleRef;
            if(m_LoadedBundles.TryGetValue(assetBundleName, out assetBundleRef)) {
                assetBundleRef.RefCount++;
                onComplete?.Invoke(assetBundleRef.Bundle);
                return true;
            }

            var loadAssetBundleProcess = m_ProcessQueue.Find<Process_LoadAssetBundle>(assetBundleName);
            if(loadAssetBundleProcess != null) {
                loadAssetBundleProcess.OnComplete -= onComplete;
                loadAssetBundleProcess.OnComplete += onComplete;
                return true;
            }

            return false;
        }

        protected void LoadAssetBundleInternal( string assetBundleName, Action<AssetBundle> onComplete, Action<string> onError ) {
            if(RequestAssetBundle(assetBundleName, onComplete)) {
                return;
            }

            UnityWebRequest request = null;

            Action<string> startAssetBundleLoad = delegate ( string assetBundleNameToLoad ) {
                string url = BuildLoadURL(assetBundleNameToLoad);
                request = UnityWebRequestAssetBundle.GetAssetBundle(url, Manifest.GetAssetBundleHash(assetBundleNameToLoad));

                m_ProcessQueue.Enqueue(new Process_LoadAssetBundle(assetBundleNameToLoad, request, delegate ( AssetBundle b ) {
                    assetBundleNameToLoad = RemapVariantName(assetBundleNameToLoad);
                    m_LoadedBundles.Add(assetBundleNameToLoad, new AssetBundleRef() { Bundle = b });

                    LoadDependencies(assetBundleNameToLoad, delegate () {
                        onComplete?.Invoke(b);
                    }, onError);
                }, onError));
            };

            if(Manifest == null) {// Load the manifest assetbundle
                if(!assetBundleName.Equals(AssetBundles.Utility.GetPlatformName())) {
                    LoadManifestAsync(delegate ( AssetBundleManifest manifest ) {
                        assetBundleName = RemapVariantName(assetBundleName);
                        if(!RequestAssetBundle(assetBundleName, onComplete)) {
                            startAssetBundleLoad(assetBundleName);
                        }

                    }, onError);
                    return;
                }

                //AOB: Loading the Manifest
                string url = BuildLoadURL(assetBundleName);
                request = UnityWebRequestAssetBundle.GetAssetBundle(url);

                m_ProcessQueue.Enqueue(new Process_LoadAssetBundle(assetBundleName, request, delegate ( AssetBundle b ) {
                    //assetBundleName = RemapVariantName(assetBundleName);
                    m_LoadedBundles.Add(assetBundleName, new AssetBundleRef() { Bundle = b });

                    LoadDependencies(assetBundleName, delegate () {
                        onComplete?.Invoke(b);
                    }, onError);
                }, onError));
                return;
            }

            startAssetBundleLoad(assetBundleName);
        }

        protected void LoadDependencies( string assetBundleName, Action onComplete, Action<string> onError ) {
            if(Manifest == null) {
                onComplete?.Invoke();
                return;
            }

            // Get dependecies from the AssetBundleManifest object..
            string[] dependencies = Manifest.GetAllDependencies(assetBundleName);
            if(dependencies.Length <= 0) {
                onComplete?.Invoke();
                return;
            }

            for(int i = 0; i < dependencies.Length; i++) {
                dependencies[i] = RemapVariantName(dependencies[i]);
            }

            int completionCount = 0;

            // Record and load all dependencies.
            m_Dependencies.Add(assetBundleName, dependencies);
            for(int i = 0; i < dependencies.Length; i++) {
                LoadAssetBundleInternal(dependencies[i], delegate ( AssetBundle b ) {
                    if(++completionCount >= dependencies.Length) {
                        onComplete?.Invoke();
                    }
                }, onError);
            }
        }

        protected struct VariantInfo {
            public string   Name;
            public string   AssetBundleName;
        }
        protected Dictionary<string, List<VariantInfo>>  m_VariantMap = new Dictionary<string, List<VariantInfo>>();

        protected void BuildVariantInfo() {
            if(Manifest == null) {
                return;
            }

            if(m_VariantMap.Count > 0) {
                return;
            }

            string[] bundlesWithVariant = Manifest.GetAllAssetBundlesWithVariant();

            for(int ix = 0; ix < bundlesWithVariant.Length; ++ix) {
                string variantName = bundlesWithVariant[ix];
                string[] curSplit = variantName.Split('.');
                string curBaseName = curSplit[0];
                string curVariant = curSplit[1];

                List<VariantInfo> variantList;
                try {
                    variantList = m_VariantMap[curBaseName];
                }
                catch(KeyNotFoundException) {
                    variantList = new List<VariantInfo>();
                    m_VariantMap.Add(curBaseName, variantList);
                }

                variantList.Add(new VariantInfo() { Name = curVariant, AssetBundleName= variantName });
            }
        }

        protected string RemapVariantName( string assetBundleName ) {
            if(Manifest == null) {
                return assetBundleName;
            }

            if(assetBundleName.IndexOf('.') > 0) {
                return assetBundleName;
            }

            List<VariantInfo> variantList;
            try {
                variantList = m_VariantMap[assetBundleName];
            }
            catch(KeyNotFoundException) {
                return assetBundleName;
            }

            foreach(VariantInfo variantInfo in variantList) {
                if(System.Array.IndexOf(m_ActiveVariants, variantInfo.Name) >= 0) {
                    return variantInfo.AssetBundleName;
                }
            }

            return assetBundleName;
        }

        protected void OnDestroy() {
            m_ProcessQueue.CancelAll();
            UnloadAllAssetBundles();
        }

        #region Public Methods
        public void LoadLevelAsync( string assetBundleName, string levelName, UnityEngine.SceneManagement.LoadSceneMode mode, Action onComplete, Action<string> onError ) {
#if UNITY_EDITOR
            if(DevelopmentMode) {
                m_ProcessQueue.Enqueue(new Process_LoadLevelFromBundleDevelopment(assetBundleName, levelName, mode, onComplete, onError));
                return;
            }
#endif
            LoadAssetBundleAsync(assetBundleName, delegate ( AssetBundle bundle ) {
                var loadAssetProcess = m_ProcessQueue.Find<Process_LoadLevelFromBundle>(levelName);
                if(loadAssetProcess == null) {
                    m_ProcessQueue.Enqueue(new Process_LoadLevelFromBundle(bundle, levelName, mode, onComplete, onError));
                }
            }, onError);
        }

        public void LoadAssetAsync<TAsset>( string assetBundleName, string assetName, Action<TAsset> onComplete, Action<string> onError ) where TAsset : UnityEngine.Object {
#if UNITY_EDITOR
            if(DevelopmentMode) {
                m_ProcessQueue.Enqueue(new Process_LoadAssetFromBundleDevelopment<TAsset>(assetBundleName, assetName, onComplete, onError));
                return;
            }
#endif

            LoadAssetBundleAsync(assetBundleName, delegate ( AssetBundle bundle ) {
                bundle.LoadAssetAsync<TAsset>(assetName).completed += delegate ( AsyncOperation op ) {
                    AssetBundleRequest req = op as AssetBundleRequest;
                    onComplete?.Invoke(req.asset as TAsset);
                };
            }, onError);
        }

        public void LoadAssetBundleAsync( string assetBundleName, Action<AssetBundle> onComplete, Action<string> onError ) {
#if UNITY_EDITOR
            if(DevelopmentMode) {
                return;
            }
#endif
            assetBundleName = RemapVariantName(assetBundleName);

            AssetBundleRef assetBundleRef;
            if(m_LoadedBundles.TryGetValue(assetBundleName, out assetBundleRef)) {
                onComplete?.Invoke(assetBundleRef.Bundle);
                return;
            }

            var loadAssetBundleProcess = m_ProcessQueue.Find<Process_LoadAssetBundle>(assetBundleName);
            if(loadAssetBundleProcess != null) {
                loadAssetBundleProcess.OnComplete += onComplete;
                return;
            }

            LoadAssetBundleInternal(assetBundleName, onComplete, onError);
        }

        public void UnloadAssetBundle( string assetBundleName ) {
#if UNITY_EDITOR
            if(DevelopmentMode)
                return;
#endif

            try {
                assetBundleName = RemapVariantName(assetBundleName);
                var bundle = m_LoadedBundles[assetBundleName];
                --bundle.RefCount;
                if(bundle.RefCount <= 0) {
                    UnloadDependencies(assetBundleName);
                    bundle.Bundle.Unload(true);
                    m_LoadedBundles.Remove(assetBundleName);
                }
            }
            catch(KeyNotFoundException) { }
        }

        public void UnloadAllAssetBundles() {
            using(var bundleNames = DataStructureLibrary<List<string>>.Instance.CheckOut()) {

                bundleNames.Value.Clear();
                bundleNames.Value.AddRange(m_LoadedBundles.Keys);

                foreach(var name in bundleNames.Value) {
                    UnloadAssetBundle(name);
                }

                m_LoadedBundles.Clear();
            }
        }
        #endregion
    }
}