using DevNote;
using UnityEngine;
using VContainer;

public class MainSceneScope : SceneScope
{
    [SerializeField] private int _testNumber;


    protected override void Configure()
    {
        var test = Register(new TestController(_testNumber));


    }

    private T Register<T>(T controller) where T : class
    {
        Builder.RegisterInstance(controller).AsImplementedInterfaces().AsSelf();
        return controller;
    }

}
