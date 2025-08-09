using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    public static partial class GameState // Handlers
    {
        public static string GetEncodedData() => GameStateEncoder.Encode(GameStateParcer.ToDataString());
        public static void RestoreFromEncodedData(string data)
        {
            if (GameStateEncoder.DataIsSupported(data))
                GameStateParcer.Parse(GameStateEncoder.Decode(data));

            else if (GameStateTransferParser.Available)
            {
                Debug.Log($"{Info.Prefix} Encoder \"{GameStateEncoder.VERSION}\": " +
                    $"Using {nameof(GameStateTransferParser)} for transfer old saves to current version of the encoder.");

                GameStateParcer.Parse(GameStateTransferParser.Parse(data));
            }
            else
            {
                Debug.Log($"{Info.Prefix} Encoder \"{GameStateEncoder.VERSION}\": Current data format is not supported.\n" +
               $"Please write realisation for {nameof(GameStateTransferParser)} to transfer old saves to current version of the encoder. " +
               $"Now all player saves are deleted.\nData: {data}" );

                var emptyDictionary = new Dictionary<string, string>();
                GameStateParcer.Parse(emptyDictionary);
            }

        }


    }

}

