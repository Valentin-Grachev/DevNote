using DevNote;
using UnityEngine;


public class TestController : IStartable, IUpdatable, IContextDisposable
{
    private int _testNumber = 0;


    public TestController(int testNumber)
    {
        _testNumber = testNumber;
        Debug.Log("constructor");
    }

    void IStartable.Start()
    {
        Debug.Log("Start");
    }

    public void Call()
    {
        Debug.Log($"Call");
    }

    void IUpdatable.Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            Debug.Log($"Tick: {_testNumber}");
    }

    void IContextDisposable.Dispose()
    {
        Debug.Log("dispose");
    }
}
