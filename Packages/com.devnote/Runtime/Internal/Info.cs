
namespace DevNote
{
    public static class Info
    {

        public const string ENCODER_VERSION = "DN1";
        public const string VERSION = "v.2.5.2";

        public static string Prefix
        {
            get => IEnvironment.IsEditor ? "<color=#DEA3FF>[DevNote]</color>" : "[DevNote]";
        }


    }
}


