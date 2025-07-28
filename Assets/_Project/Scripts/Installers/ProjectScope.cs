using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace DevNote
{
    public class ProjectScope : LifetimeScope
    {
        public static IContainerBuilder Builder { get; private set; }
        public static bool Exists { get; private set; } = false;

        [SerializeField] private bool _testVersion;
        [SerializeField] private EnvironmentType _environmentType;
        [Space(10)]
        [SerializeField] private ServiceSelector _serviceSelector;
        [SerializeField] private Sound _sound;
        [SerializeField] private GoogleTables _googleTables;
        [SerializeField] private Localization _localization;
        [SerializeField] private List<GameObject> _onlyBootstrapGameObject;

        private List<IProjectInitializable> _initializables = new();


        protected async override void Configure(IContainerBuilder builder)
        {
            Exists = true;
            Builder = builder;
            SetActiveRootGameObjects(false);

            IEnvironment.IsTest = _testVersion;
            IEnvironment.EnvironmentType = _environmentType;

            var environment = SelectAndBindService<IEnvironment>();
            var save = SelectAndBindService<ISave>();
            var purchase = SelectAndBindService<IPurchase>();
            var ads = SelectAndBindService<IAds>();
            var analytics = SelectAndBindService<IAnalytics>();
            var review = SelectAndBindService<IReview>();

            RunInitialization(environment);
            RunInitialization(save);
            RunInitialization(ads);
            RunInitialization(purchase);
            RunInitialization(analytics);
            RunInitialization(review);
            RunInitialization(_sound);
            RunInitialization(_googleTables);
            RunInitialization(_localization);

            await WaitFullInitialization();

            SetActiveRootGameObjects(true);
            _onlyBootstrapGameObject.ForEach(gameObject => gameObject.SetActive(false));

            environment.GameReady();
        }

        private T SelectAndBindService<T>() where T : class
        {
            var service = _serviceSelector.GetServiceInterface<T>();
            Builder.RegisterInstance(service).AsImplementedInterfaces().AsSelf();
            return service;
        }



        private UniTask WaitFullInitialization() => UniTask.WaitUntil(() =>
        {
            for (int i = 0; i < _initializables.Count; i++)
            {
                if (_initializables[i].Initialized == false)
                    return false;
            }

            return true;
        });

        private T RunServiceInitialization<T>() where T : class
        {
            var service = _serviceSelector.GetServiceInterface<T>();
            var initializable = service as DevNote.IProjectInitializable;
            _initializables.Add(initializable);
            initializable.Initialize();
            return service;
        }

        private void RunInitialization(DevNote.IProjectInitializable initializable)
        {
            initializable.Initialize();
            _initializables.Add(initializable);
        }


        private void SetActiveRootGameObjects(bool active)
        {
            foreach (var rootObject in SceneManager.GetActiveScene().GetRootGameObjects())
                rootObject.SetActive(active);
        }



    }
}

