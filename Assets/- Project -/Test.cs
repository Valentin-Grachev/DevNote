using DevNote;
using UnityEngine;

public enum TestType { ABA, AbA, Biba }

public class Test : MonoBehaviour
{
    

    private readonly Holder<IRemote> remote = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Configs.AudioHub.Show.Play();
        }
        
    }




}
