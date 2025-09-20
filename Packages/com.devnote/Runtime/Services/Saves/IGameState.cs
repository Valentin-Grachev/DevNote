using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DevNote
{
    public interface IGameState
    {
        public static ReactiveValue<bool> NoAdsPurchased { get; protected set; }


        private static IGameState _handler;
        public static void SetHandler(IGameState handler) => _handler = handler;

        public int Version { get; }
        public void Parse(Dictionary<string, string> data);
        public Dictionary<string, string> ToDictionary();


        public bool TransferParsingAvailable { get; }
        public Dictionary<string, string> TransferParse(string data);



        public static string VersionPrefix => $"DN{_handler.Version}";

        public static string GetEncodedData() => GameStateEncoder.Encode(_handler.ToDictionary());
        public static void RestoreFromEncodedData(string data)
        {
            bool dataIsSupported = string.IsNullOrEmpty(data) || data.StartsWith(VersionPrefix);

            if (dataIsSupported)
                _handler.Parse(GameStateEncoder.Decode(data));

            else if (_handler.TransferParsingAvailable)
            {
                Debug.Log($"{Info.Prefix} Game state version \"{VersionPrefix}\": " +
                    $"Using transfer parsing old saves to current version of the encoder.");

                _handler.TransferParse(data);
            }
            else
            {
                Debug.Log($"{Info.Prefix} Encoder \"{VersionPrefix}\": Current data format is not supported.\n" +
               $"Please write realisation for TransferParse() to transfer old saves to current version of the encoder. " +
               $"Now all player saves are deleted.\nOld data: {data}");

                var emptyDictionary = new Dictionary<string, string>();
                _handler.Parse(emptyDictionary);
            }

        }




    }
}

