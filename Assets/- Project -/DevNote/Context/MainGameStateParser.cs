using System.Collections.Generic;
using DevNote;


public static class GameState
{
    public static ReactiveValue<bool> NoAdsPurchased { get; set; }



}



public class MainGameStateParser : GameStateParser
{
    private const string NO_ADS_PURCHASED_KEY = "noAds";


    public override void Parse(Dictionary<string, string> data)
    {
        GameState.NoAdsPurchased = new(bool.Parse(data.GetValueOrDefault(NO_ADS_PURCHASED_KEY, "false")));
    }

    public override Dictionary<string, string> ToDictionary() => new()
    {
        { NO_ADS_PURCHASED_KEY, GameState.NoAdsPurchased.ToString() },
    };



    public override bool TransferParsingAvailable => false;
    public override Dictionary<string, string> TransferParse(string data)
    {
        throw new System.NotImplementedException();
    }
}
