using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    public static partial class GameState // Handlers
    {
        public static string GetEncodedData() => Encoder.Encode(DataParser.ToDataString());
        public static void RestoreFromEncodedData(string data)
        {
            if (Encoder.DataIsSupported(data))
                DataParser.Parse(Encoder.Decode(data));

            else if (TransferParser.Available)
            {
                Debug.Log($"{Info.Prefix} Encoder \"{Info.ENCODER_VERSION}\": " +
                    $"Using {nameof(TransferParser)} for transfer old saves to current version of the encoder.");

                DataParser.Parse(TransferParser.Parse(data));
            }
            else
            {
                Debug.Log($"{Info.Prefix} Encoder \"{Info.ENCODER_VERSION}\": Current data format is not supported.\n" +
               $"Please write realisation for {nameof(TransferParser)} to transfer old saves to current version of the encoder. " +
               $"Now all player saves are deleted.\nData: {data}" );

                var emptyDictionary = new Dictionary<string, string>();
                DataParser.Parse(emptyDictionary);
            }

        }


    }

}

