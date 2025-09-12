using System.Collections.Generic;

namespace DevNote
{

    public static partial class GameState // DataParser
    {

        private static class DataParser
        {
            public const string NO_ADS_PURCHASED_KEY = "noAdsPurchased";


            public static void Parse(Dictionary<string, string> data)
            {
                bool noAdsPurchased = bool.Parse(data.GetValueOrDefault(NO_ADS_PURCHASED_KEY, "false"));
                NoAdsPurchased = new(noAdsPurchased);

            }


            public static Dictionary<string, string> ToDataString()
            {
                var data = new Dictionary<string, string>
                {
                    { NO_ADS_PURCHASED_KEY, NoAdsPurchased.ToString() }
                };

                return data;
            }


        }
    }

    
}


