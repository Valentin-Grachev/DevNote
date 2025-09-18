using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    public abstract class GameStateParser : MonoBehaviour
    {
        public static GameStateParser Handler { get; private set; }
        public static void SetHandler(GameStateParser handler) => Handler = handler;


        public abstract void Parse(Dictionary<string, string> data);
        public abstract Dictionary<string, string> ToDictionary();


        public abstract bool TransferParsingAvailable { get; }
        public abstract Dictionary<string, string> TransferParse(string data);





        public static string GetEncodedData() => GameStateEncoder.Encode(Handler.ToDictionary());
        public static void RestoreFromEncodedData(string data)
        {
            if (GameStateEncoder.DataIsSupported(data))
                Handler.Parse(GameStateEncoder.Decode(data));

            else if (Handler.TransferParsingAvailable)
            {
                Debug.Log($"{Info.Prefix} Encoder \"{Info.ENCODER_VERSION}\": " +
                    $"Using transfer parsing old saves to current version of the encoder.");

                Handler.TransferParse(data);
            }
            else
            {
                Debug.Log($"{Info.Prefix} Encoder \"{Info.ENCODER_VERSION}\": Current data format is not supported.\n" +
               $"Please write realisation for TransferParse() to transfer old saves to current version of the encoder. " +
               $"Now all player saves are deleted.\nData: {data}");

                var emptyDictionary = new Dictionary<string, string>();
                Handler.Parse(emptyDictionary);
            }

        }




    }
}

