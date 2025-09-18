using System.Collections.Generic;
using DevNote;


public partial class GameState // Data
{
    public static ReactiveValue<bool> NoAdsPurchased { get; private set; }




}


public partial class GameState : GameStateParser // Parsing
{
    private const string NO_ADS_PURCHASED_KEY = "noAds";

    public override void Parse(Dictionary<string, string> data)
    {
        NoAdsPurchased = new(bool.Parse(data.GetValueOrDefault(NO_ADS_PURCHASED_KEY, "false")));
    }

    public override Dictionary<string, string> ToDictionary() => new()
    {
        { NO_ADS_PURCHASED_KEY, NoAdsPurchased.ToString() },
    };


    public override bool TransferParsingAvailable => false;
    public override Dictionary<string, string> TransferParse(string data)
    {
        throw new System.NotImplementedException();
    }
}