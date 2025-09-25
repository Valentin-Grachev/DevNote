using System.Collections.Generic;
using DevNote;
using UnityEngine;


public partial class GameState // Data
{
    public static ReactiveValue<bool> NoAdsPurchased => IGameState.NoAdsPurchased;



}


public partial class GameState : MonoBehaviour, IGameState // Parsing
{
    int IGameState.Version => 1;



    private const string NO_ADS_PURCHASED_KEY = "noAds";

    void IGameState.Parse(Dictionary<string, string> data)
    {
        IGameState.NoAdsPurchased = new(bool.Parse(data.GetValueOrDefault(NO_ADS_PURCHASED_KEY, "False")));
    }

    Dictionary<string, string> IGameState.ToDictionary() => new()
    {
        { NO_ADS_PURCHASED_KEY, NoAdsPurchased.ToString() },
    };


    bool IGameState.TransferParsingAvailable => false;
    Dictionary<string, string> IGameState.TransferParse(string data)
    {
        throw new System.NotImplementedException();
    }
}