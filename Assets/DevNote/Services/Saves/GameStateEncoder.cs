using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;


namespace DevNote
{
    public static class GameStateEncoder
    {
        private const char SERVICE_SEPARATOR = '/';
        private const char CELL_SEPARATOR = '|';
        private const char KEY_VALUE_SEPARATOR = '+';


        public static bool DataIsSupported(string encodedData) 
            => encodedData.StartsWith($"{Info.ENCODER_VERSION}") || string.IsNullOrEmpty(encodedData);

        public static DateTime GetSaveTime(string encodedData)
        {
            string[] splitData = encodedData.Split(SERVICE_SEPARATOR);

            if (splitData.Length != 3) return DateTime.MinValue;
            return DateTime.Parse(encodedData.Split(SERVICE_SEPARATOR)[1]);
        }


        public static Dictionary<string, string> Decode(string encodedData)
        {
            string[] splitData = encodedData.Split(SERVICE_SEPARATOR);
            bool decodeAvailable = splitData.Length == 3 && splitData[2] != string.Empty;

            if (decodeAvailable)
                return ToDataDictionary(Decompress(splitData[2]));

            return new Dictionary<string, string>();
        }

        public static string Encode(Dictionary<string, string> originDataDictionary)
        {
            var time = IEnvironment.Time;
            string originData = ToDataString(originDataDictionary);
            return $"{Info.ENCODER_VERSION}{SERVICE_SEPARATOR}{time}{SERVICE_SEPARATOR}" + Compress(originData);
        }




        private static Dictionary<string, string> ToDataDictionary(string data)
        {
            var result = new Dictionary<string, string>();
            var splitByCellData = data.Split(CELL_SEPARATOR);

            for (int i = 0; i < splitByCellData.Length; i++)
            {
                var splitCell = splitByCellData[i].Split(KEY_VALUE_SEPARATOR);
                result.Add(splitCell[0], splitCell[1]);
            }

            return result;
        }

        private static string ToDataString(Dictionary<string, string> dataDictionary)
        {
            var result = string.Empty;

            int i = 0;
            foreach (var keyValue in dataDictionary)
            {
                result += $"{keyValue.Key}{KEY_VALUE_SEPARATOR}{keyValue.Value}";
                if (i != dataDictionary.Count - 1) result += CELL_SEPARATOR;
                i++;
            }

            return result;
        }


        private static string Compress(string uncompressedString)
        {
            byte[] compressedBytes;

            using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString)))
            {
                using (var compressedStream = new MemoryStream())
                {
                    using (var compressorStream = new DeflateStream(compressedStream, CompressionLevel.Fastest, true))
                    {
                        uncompressedStream.CopyTo(compressorStream);
                    }
                    compressedBytes = compressedStream.ToArray();
                }
            }

            return Convert.ToBase64String(compressedBytes);
        }

        private static string Decompress(string compressedString)
        {
            try
            {
                byte[] decompressedBytes;

                var compressedStream = new MemoryStream(Convert.FromBase64String(compressedString));
                using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
                {
                    using (var decompressedStream = new MemoryStream())
                    {
                        decompressorStream.CopyTo(decompressedStream);

                        decompressedBytes = decompressedStream.ToArray();
                    }
                }

                return Encoding.UTF8.GetString(decompressedBytes);
            }
            catch 
            {
                return string.Empty;
            }
        }




    }
}


