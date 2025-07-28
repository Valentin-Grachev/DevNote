using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace DevNote
{
    public class ProjectContext : MonoBehaviour
    {
        public static bool Exists { get; private set; } = false;

        public static bool Initialized { get; private set; } = false;


        [SerializeField] private bool _testVersion;
        [SerializeField] private EnvironmentType _environmentType;
        [Space(10)]
        [SerializeField] private Context _context;
        [SerializeField] private ServiceSelector _serviceSelector;
        [SerializeField] private Sound _sound;
        [SerializeField] private GoogleTables _googleTables;
        [SerializeField] private Localization _localization;
        [SerializeField] private List<GameObject> _onlyBootstrapGameObject;

        private List<IProjectInitializable> _initializables = new();

        private async void Awake()
        {
            _context.Initialize();
            Exists = true;

            SetActiveRootGameObjects(false);

            IEnvironment.IsTest = _testVersion;
            IEnvironment.EnvironmentType = _environmentType;

            var environment = SelectAndRegisterService<IEnvironment>();
            var save = SelectAndRegisterService<ISave>();
            var purchase = SelectAndRegisterService<IPurchase>();
            var ads = SelectAndRegisterService<IAds>();
            var analytics = SelectAndRegisterService<IAnalytics>();
            var review = SelectAndRegisterService<IReview>();

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

            Initialized = true;

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
            _context.gameObject.SetActive(active);

            foreach (var rootObject in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (rootObject != gameObject)
                    rootObject.SetActive(active);
            }
                
        }


    }
}


