using System;
using UnityEngine;

namespace Nodes.Providers.Test
{
    public class TestCloudSavesProvider : MonoBehaviour, CloudSaves.IProvider
    {
        [SerializeField] private bool _showLogs = true;
        [SerializeField] private bool _simulateCommitError;

        void ICommonProvider.Initialize(Action onInitialized) 
            => onInitialized?.Invoke();


        void CloudSaves.IProvider.Commit(Action<bool> onHandled)
        {
            bool success = !_simulateCommitError;

            var logMessage = success ? "[Cloud Saves] Commit success." :
                "[Cloud Saves] Commit error.";

            if (_showLogs) Debug.Log(logMessage);

            onHandled?.Invoke(success);
        }

        bool CloudSaves.IProvider.TryGetValue(string key, out string value)
        {
            value = PlayerPrefs.HasKey(key) ? PlayerPrefs.GetString(key) : string.Empty;
            return PlayerPrefs.HasKey(key);
        }


        void CloudSaves.IProvider.SetValue(string key, string value)
            => PlayerPrefs.SetString(key, value);

        
    }
}

