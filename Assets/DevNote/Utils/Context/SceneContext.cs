using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DevNote
{
    public abstract class SceneContext : MonoBehaviour
    {
        private List<Type> _registeredTypes = new();

        private async void Awake()
        {
            if (!ProjectContext.Exists) CreateProjectContext();

            await UniTask.WaitUntil(() => ProjectContext.Initialized);
            RegisterContext();
        }

        private void OnDestroy() => UnregisterContext();


        private void CreateProjectContext()
        {
            var projectContext = Resources.Load<ProjectContext>("[ProjectContext]");
            Instantiate(projectContext);
            DontDestroyOnLoad(projectContext.gameObject);
        }


        protected abstract void RegisterContext();


        protected T Register<T>(T controller) where T : class
        {
            Context.Register(controller);
            _registeredTypes.Add(typeof(T));
            return controller;
        }

        private void UnregisterContext()
        {
            foreach (var type in _registeredTypes)
                Context.Unregister(type);
        }


    }
}


