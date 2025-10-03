using System.Collections.Generic;
using DevNote;
using UnityEngine;



public class GameState : MonoBehaviour, IGameState // Parsing
{
    int IGameState.Version => 1;

    void IGameState.Parse(Dictionary<string, string> data)
    {
        IGameState.ParseState(data);
    }

    Dictionary<string, string> IGameState.ToDictionary()
    {
        var data = new Dictionary<string, string>()
        {
            IGameState.GetStateDictionary(),
        };

        return data;
    }


    bool IGameState.TransferParsingAvailable => false;
    Dictionary<string, string> IGameState.TransferParse(string data)
    {
        throw new System.NotImplementedException();
    }
}