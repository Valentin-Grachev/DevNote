using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;


namespace DevNote
{
    public static class GameStateEncoder
    {
        public const string VERSION = "DN1";
        private const char VERSION_DATA_SEPARATOR = ':';

        private const char CELL_SEPARATOR = '|';
        private const char KEY_VALUE_PAIR_SEPARATOR = '+';


        public static bool DataIsSupported(string data) 
            => data.StartsWith($"{VERSION}{VERSION_DATA_SEPARATOR}") || data == string.Empty;


        public static Dictionary<string, string> Decode(string encodedData)
        {
            if (encodedData == string.Empty) 
                return new Dictionary<string, string>();

            encodedData = encodedData.Replace($"{VERSION}{VERSION_DATA_SEPARATOR}", string.Empty);
            string originData = Decompress(encodedData);

            if (originData == string.Empty) 
                return new Dictionary<string, string>();

            return ToDataDictionary(originData);
        }

        public static string Encode(Dictionary<string, string> originDataDictionary)
        {
            string originData = ToDataString(originDataDictionary);
            return $"{VERSION}{VERSION_DATA_SEPARATOR}" + Compress(originData);
        }




        private static Dictionary<string, string> ToDataDictionary(string data)
        {
            var result = new Dictionary<string, string>();
            var splitByCellData = data.Split(CELL_SEPARATOR);

            for (int i = 0; i < splitByCellData.Length; i++)
            {
                var splitCell = splitByCellData[i].Split(KEY_VALUE_PAIR_SEPARATOR);
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
                result += $"{keyValue.Key}{KEY_VALUE_PAIR_SEPARATOR}{keyValue.Value}";
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


