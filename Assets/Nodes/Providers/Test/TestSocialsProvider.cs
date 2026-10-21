using System;
using UnityEngine;

namespace Nodes.Providers.Test
{
    public class TestSocialsProvider : MonoBehaviour, Socials.IProvider
    {
        [SerializeField] private bool _isSupportInvite = true;
        [SerializeField] private bool _isSupportRateGame = true;

        bool Socials.IProvider.IsSupportInvite => _isSupportInvite;
        bool Socials.IProvider.IsSupportRateGame => _isSupportRateGame;


        void ICommonProvider.Initialize(Action onInitialized) => onInitialized?.Invoke();
        void Socials.IProvider.Invite() { }
        void Socials.IProvider.RateGame() { }
    }
}

