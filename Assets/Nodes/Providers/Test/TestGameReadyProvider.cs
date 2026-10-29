using System;
using UnityEngine;

namespace Nodes.Providers.Test
{
    public class TestGameReadyProvider : MonoBehaviour, GameReady.IProvider
    {
        [SerializeField] private bool _showLogs = true;


        void ICommonProvider.Initialize(Action onInitialized) => onInitialized?.Invoke();

        void GameReady.IProvider.GameplayPause()
        {
            if (_showLogs)
                Debug.Log("[Game Ready] Gameplay pause.");
        }

        void GameReady.IProvider.GameplayReady()
        {
            if (_showLogs)
                Debug.Log("[Game Ready] Gameplay ready.");
        }

        void GameReady.IProvider.GameplayStart()
        {
            if (_showLogs) 
                Debug.Log("[Game Ready] Gameplay start.");
        }


    }
}

