using DevNote;

public class MainSceneContext : SceneContext
{

    public override void RegisterContext()
    {

        IRemote.SetDefaultHandler(RemoteKey.Test, () => TestType.ABA.ToString());



    }


}
