
namespace DevNote
{
    public static class Const
    {
        public const string VERSION = "v.2.0.0";

        public static string LOG_PREFIX
        {
            get => IEnvironment.IsEditor ? "<color=#DEA3FF>[DevNote]</color>" : "[DevNote]";
        }


    }
}


