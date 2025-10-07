using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace DevNote
{
    public class ProjectContext : MonoBehaviour
    {
        [Header("DevNote " + Info.VERSION), Space]
        [SerializeField] private bool _testVersion;
        [SerializeField] private EnvironmentKey _environmentKey;
        [Space(10)]
        [SerializeField] private MonoBehaviour _gameState;
        [SerializeField] private MonoBehaviour _purchaseHandler;
        [SerializeField] private ServiceSelector _serviceSelector;
        [SerializeField] private Sound _sound;
        [SerializeField] private GoogleTables _googleTables;
        [SerializeField] private Localization _localization;
        [SerializeField] private List<GameObject> _onlyBootstrapGameObject;

        private List<IInitializable> _initializables = new();

        public async void RegisterContext()
        {
            SetActiveRootGameObjects(false);

            IGameState.SetHandler(_gameState as IGameState);
            IPurchaseHandler.SetHandler(_purchaseHandler as IPurchaseHandler);

            IEnvironment.IsTest = _testVersion;
            IEnvironment.EnvironmentKey = _environmentKey;

            var environment = SelectAndRegisterService<IEnvironment>();
            var save = SelectAndRegisterService<ISave>();
            var purchase = SelectAndRegisterService<IPurchase>();
            var ads = SelectAndRegisterService<IAds>();
            var analytics = SelectAndRegisterService<IAnalytics>();
            var review = SelectAndRegisterService<IReview>();
            var leaderboards = SelectAndRegisterService<ILeaderboards>();
            var remote = SelectAndRegisterService<IRemote>();

            RunInitialization(environment);
            RunInitialization(remote);

            await UniTask.WaitUntil(() => remote.Initialized);

            RunInitialization(save);
            RunInitialization(ads);
            RunInitialization(purchase);
            RunInitialization(analytics);
            RunInitialization(review);
            RunInitialization(leaderboards);
            RunInitialization(_sound);
            RunInitialization(_googleTables);
            RunInitialization(_localization);

            Context.Register(new ScreenState());

            await WaitFullInitialization();

            SetActiveRootGameObjects(true);
            _onlyBootstrapGameObject.ForEach(gameObject => gameObject.SetActive(false));

            environment.GameReady();
        }


        private T SelectAndRegisterService<T>() where T : class
        {
            var service = _serviceSelector.GetServiceInterface<T>();
            Context.Register(service);
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



        private void RunInitialization(IInitializable initializable)
        {
            initializable.Initialize();
            _initializables.Add(initializable);
        }


        private void SetActiveRootGameObjects(bool active)
        {
            foreach (var rootObject in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (rootObject != gameObject)
                    rootObject.SetActive(active);
            }
                
        }


    }
}


