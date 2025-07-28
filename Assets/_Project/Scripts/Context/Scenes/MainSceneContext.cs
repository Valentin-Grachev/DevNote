using DevNote;

public class MainSceneContext : SceneContext
{
    protected override void RegisterContext()
    {

        var test = Register(new TestController(3));


    }


}
