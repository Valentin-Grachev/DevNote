using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace DevNote
{
    public abstract class SceneScope : LifetimeScope
    {
        protected IContainerBuilder Builder { get; private set; }


        protected sealed override void Configure(IContainerBuilder builder)
        {
            Builder = builder;
            if (!ProjectScope.Exists) CreateProjectScope();
            Configure();
        }

        protected abstract void Configure();


        private void CreateProjectScope()
        {
            var projectScopePrefab = Resources.Load<ProjectScope>("[ProjectScope]");
            Instantiate(projectScopePrefab);
            DontDestroyOnLoad(projectScopePrefab);
        }


    }
}


