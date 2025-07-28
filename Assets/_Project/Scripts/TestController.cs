using System;
using UnityEngine;
using VContainer.Unity;

public class TestController : ITickable, IStartable, IDisposable
{
    private int _testNumber = 0;


    public TestController(int testNumber)
    {
        _testNumber = testNumber;
        Debug.Log("constructor");
    }



    public void Dispose()
    {
        Debug.Log("dispose");
    }

    void IStartable.Start()
    {
        Debug.Log("Start");
    }

    void ITickable.Tick()
    {
        if (Input.GetKeyDown(KeyCode.T))
            Debug.Log($"Tick: {_testNumber}");
    }

    public void Call()
    {
        Debug.Log($"Call");
    }


}
