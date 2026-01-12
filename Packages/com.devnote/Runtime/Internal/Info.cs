
namespace DevNote
{
    public static class Info
    {
        public const string VERSION = "v.2.10.1";

        public static string Prefix
        {
            get => IEnvironment.IsEditor ? "<color=#DEA3FF>[DevNote]</color>" : "[DevNote]";
        }


    }
}


