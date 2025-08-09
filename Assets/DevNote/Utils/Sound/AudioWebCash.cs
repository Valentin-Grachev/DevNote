using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Networking;


namespace DevNote
{
    public class AudioWebCash : MonoBehaviour, IInitializable
    {
        private static Dictionary<string, AudioClip> cashedClips = new Dictionary<string, AudioClip>();

        [SerializeField] private List<string> _cashedClipNames;

        private int _loadedClips = 0;
        private bool _initialized = false;


        bool IInitializable.Initialized => _initialized;


        void IInitializable.Initialize()
        {
            LoadAllClips();
        }

        public AudioClip GetClip(string name) => cashedClips[name + ".mp3"];


        private void LoadAllClips()
        {
            if (_cashedClipNames.Count == 0 || _cashedClipNames == null)
                _initialized = true;

            else
            {
                cashedClips.Clear();
                _loadedClips = 0;

                foreach (var clipName in _cashedClipNames)
                    LoadClip(clipName).Forget();
            }
            
        }

        private async UniTask LoadClip(string name)
        {
            string url = Application.streamingAssetsPath + "/" + name;

            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
            await request.SendWebRequest();

            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
            
            cashedClips.Add(name, audioClip);
            _loadedClips++;

            if (_loadedClips == cashedClips.Count) 
                _initialized = true;
        }

        [Button("Load cash names")]
        private void LoadCashNames()
        {
            _cashedClipNames = new List<string>();

            DirectoryInfo directory = new DirectoryInfo(Application.streamingAssetsPath);
            SearchFilesInsideDirectory(directory);
        }


        private void SearchFilesInsideDirectory(DirectoryInfo directory)
        {
            FileInfo[] info = directory.GetFiles("*.mp3");
            foreach (var item in info) _cashedClipNames.Add(item.Name);

            foreach (var insideDirectory in directory.GetDirectories())
                SearchFilesInsideDirectory(insideDirectory);
        }

    }
}


