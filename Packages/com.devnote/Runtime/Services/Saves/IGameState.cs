using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    public interface IGameState
    {
        public static bool NoAdsPurchased { get; set; }
        public static int SaveVersion { get; set; }
        public static List<ProductKey> PurchasedPermanentProducts { get; set; }
        public static DateTime LastOnlineTime { get; set; }
        public static bool IsFirstLaunch { get; set; }



        private const string NO_ADS_PURCHASED = "noAds";
        private const string SAVE_VERSION = "sVer";
        private const string PURCHASED_PRODUCTS = "prods";
        private const string LAST_ONLINE_TIME = "online";
        private const string IS_FIRST_LAUNCH = "firstLn";


        protected static void ParseState(Dictionary<string, string> data)
        {
            NoAdsPurchased = bool.Parse(data.GetValueOrDefault(NO_ADS_PURCHASED, $"{false}"));
            SaveVersion = int.Parse(data.GetValueOrDefault(SAVE_VERSION, "0"));
            IsFirstLaunch = bool.Parse(data.GetValueOrDefault(IS_FIRST_LAUNCH, $"{true}"));
            LastOnlineTime = data.GetValueOrDefault(LAST_ONLINE_TIME, $"{DateTime.MinValue}").ToDateTime();

            List<string> purchasedProductStrings = data.GetValueOrDefault(PURCHASED_PRODUCTS, string.Empty)
                .SaveDataToList(data => data);

            PurchasedPermanentProducts = new();
            foreach (var purchasedProductString in purchasedProductStrings)
            {
                if (Enum.TryParse<ProductKey>(purchasedProductString, out var productKey))
                    PurchasedPermanentProducts.Add(productKey);
            }
        }

        protected static Dictionary<string, string> GetStateDictionary() => new()
        {
            { NO_ADS_PURCHASED, NoAdsPurchased.ToString() },
            { SAVE_VERSION, SaveVersion.ToString() },
            { PURCHASED_PRODUCTS, PurchasedPermanentProducts.ToSaveData() },
            { IS_FIRST_LAUNCH, IsFirstLaunch.ToString() },
            { LAST_ONLINE_TIME, LastOnlineTime.ToDataString() },
        };




        private static IGameState _handler;
        public static void SetHandler(IGameState handler) => _handler = handler;

        public int Version { get; }
        public void Parse(Dictionary<string, string> data);
        public Dictionary<string, string> ToDictionary();


        public bool TransferParsingAvailable { get; }
        public Dictionary<string, string> TransferParse(string data);



        public static string VersionPrefix => $"DN{_handler.Version}";

        public static bool DataIsSupported(string data) => string.IsNullOrEmpty(data) || data.StartsWith(VersionPrefix);


        public static string GetEncodedData() => GameStateEncoder.Encode(_handler.ToDictionary());
        public static void RestoreFromEncodedData(string data)
        {
            if (DataIsSupported(data))
                _handler.Parse(GameStateEncoder.Decode(data));

            else if (_handler.TransferParsingAvailable)
            {
                Debug.Log($"{Info.Prefix} Game state version \"{VersionPrefix}\": " +
                    $"Using transfer parsing old saves to current version of the encoder.");

                _handler.Parse(_handler.TransferParse(data));
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

