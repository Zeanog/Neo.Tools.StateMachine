using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;

public class AssetDownloader {
    private static AssetDownloader  m_Instance = null;
    public static AssetDownloader Instance {
        get {
            if(m_Instance == null) {
                m_Instance = new AssetDownloader();
            }
            return m_Instance;
        }
    }

    public class DownloadActions {
        public Func<string, UnityWebRequest>        CreateRequest;
        public Func<UnityWebRequest, System.Object> DecodeRequest;
    }
    public class DownloadActions_Texture2D : DownloadActions {
        public static readonly DownloadActions_Texture2D   Instance = new DownloadActions_Texture2D();

        public DownloadActions_Texture2D() {
            CreateRequest = delegate ( string url ) {
                UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
                return request;
            };

            DecodeRequest = delegate ( UnityWebRequest res ) {
                return DownloadHandlerTexture.GetContent(res);
            };
        }
    }

    public class DownloadActions_AudioClip : DownloadActions {
        public static readonly DownloadActions_AudioClip   Instance = new DownloadActions_AudioClip();

        protected static Dictionary<string, AudioType>  m_ExtensionToAudioTypeMap = new Dictionary<string, AudioType>();
        static DownloadActions_AudioClip() {
            m_ExtensionToAudioTypeMap.Add(".mp3", AudioType.MPEG);
            m_ExtensionToAudioTypeMap.Add(".wav", AudioType.WAV);
            m_ExtensionToAudioTypeMap.Add(".ogg", AudioType.OGGVORBIS);
        }

        public DownloadActions_AudioClip() {
            CreateRequest = delegate ( string url ) {
                try {
                    string ext = System.IO.Path.GetExtension(url);
                    AudioType t = m_ExtensionToAudioTypeMap[ext];
                    UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, t);
                    return request;
                }
                catch(Exception ex) {
                    Debug.LogException(ex);
                    return null;
                }
            };

            DecodeRequest = delegate ( UnityWebRequest res ) {
                return DownloadHandlerAudioClip.GetContent(res);
            };
        }
    }

    protected static Dictionary<Type, DownloadActions>  m_AssetLoadActions = new Dictionary<Type, DownloadActions>();

    static AssetDownloader() {
        m_AssetLoadActions.Add(typeof(Texture2D), DownloadActions_Texture2D.Instance );
        m_AssetLoadActions.Add(typeof(AudioClip), DownloadActions_AudioClip.Instance);
    }

    public void DownloadAsset<T>( string assetUrl, Action<T> onSuccess, Action<string> onError ) {
        DownloadAssetHandler<T>(assetUrl, onSuccess, onError);
    }

    protected async void DownloadAssetHandler<T>( string assetUrl, Action<T> onSuccess, Action<string> onError ) {
        UnityWebRequest request = null;
        DownloadActions actions = null;

        try {
            actions = m_AssetLoadActions[typeof(T)];
            request = actions.CreateRequest(assetUrl);
            await request.SendWebRequest();
        }
        catch(Exception ex) {
            onError?.Invoke(ex.Message);
            return;
        }

        try {
            var assetData = actions.DecodeRequest(request);
            onSuccess?.Invoke((T)assetData);
        }
        catch(Exception ex) {
            onError?.Invoke(ex.Message);
            return;
        }
    }
}