using System.Collections.Generic;

namespace DevNote
{
    public static class GameStateParcer
    {
        public const string NO_ADS_PURCHASED = "noAdsPurchased";


        public static void Parse(Dictionary<string, string> data)
        {
            bool noAdsPurchased = bool.Parse(data.GetValueOrDefault(NO_ADS_PURCHASED, "false"));
            GameState.NoAdsPurchased = new (noAdsPurchased);

        }


        public static Dictionary<string, string> ToDataString()
        {
            var data = new Dictionary<string, string>
            {
                { NO_ADS_PURCHASED, GameState.NoAdsPurchased.ToString() }
            };

            return data;
        }


    }
}


