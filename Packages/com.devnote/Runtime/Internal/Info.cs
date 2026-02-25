
namespace DevNote
{
    public static class Info
    {
        public const string VERSION = "2.15.0";

        public static string Prefix
        {
            get => IEnvironment.IsEditor ? "<color=#DEA3FF>[DevNote]</color>" : "[DevNote]";
        }


    }
}


