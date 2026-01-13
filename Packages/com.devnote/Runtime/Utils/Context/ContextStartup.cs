using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    public class ContextStartup : MonoBehaviour
    {
        private static bool _projectContextExists = false;


        [SerializeField] private List<SceneContext> _contexts;

        private void Awake()
        {
            if (!_projectContextExists)
            {
                RegisterProjectContext();
                _projectContextExists = true;
            }

            _contexts.ForEach(context => context.RegisterContext());
            gameObject.AddComponent<Context>();
        }


        private void RegisterProjectContext()
        {
            var projectContext = Instantiate(Resources.Load<ProjectContext>("- ProjectContext -"));
            DontDestroyOnLoad(projectContext.gameObject);
            projectContext.RegisterContext();
            projectContext.name = "ProjectContext";
            
        }

    }
}


