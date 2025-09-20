using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;


namespace DevNote
{
    public static class S // Separators
    {
        public const char ENCODER = '|';

        public const char S1 = '_';
        public const char S2 = ',';
        public const char S3 = ':';
        public const char S4 = ';';
        public const char S5 = '&';

        public const char L1 = '(', R1 = ')';
        public const char L2 = '[', R2 = ']';
        public const char L3 = '{', R3 = '}';
        
        public const char DICTIONARY1 = '~';
        public const char DICTIONARY2 = '|';
    }

    public static class GameStateEncoder
    {
        public static bool DataIsSupported(string encodedData)
            => encodedData.StartsWith($"{Info.ENCODER_VERSION}") || string.IsNullOrEmpty(encodedData);

        public static DateTime GetSaveTime(string encodedData)
        {
            string[] splitData = encodedData.Split(S.ENCODER);
            return splitData.Length == 3 ? DateTime.Parse(splitData[1]) : DateTime.MinValue;
        }

        public static Dictionary<string, string> Decode(string encodedData)
        {
            string[] splitData = encodedData.Split(S.ENCODER);
            bool decodeAvailable = splitData.Length == 3 && splitData[2] != string.Empty;

            if (decodeAvailable)
                return ToDataDictionary(Decompress(splitData[2]));

            return new Dictionary<string, string>();
        }

        public static string Encode(Dictionary<string, string> dictionary)
        {
            var time = IEnvironment.Time;
            string originData = ToDataString(dictionary);
            return $"{Info.ENCODER_VERSION}{S.ENCODER}{time}{S.ENCODER}" + Compress(originData);
        }

        private static Dictionary<string, string> ToDataDictionary(string data)
        {
            var result = new Dictionary<string, string>();
            var splitByCellData = data.Split(S.DICTIONARY2);

            for (int i = 0; i < splitByCellData.Length; i++)
            {
                var splitCell = splitByCellData[i].Split(S.DICTIONARY1);
                result.Add(splitCell[0], splitCell[1]);
            }

            return result;
        }

        private static string ToDataString(Dictionary<string, string> dataDictionary)
        {
            var builder = new StringBuilder();
            bool isFirst = true;

            foreach (var keyValue in dataDictionary)
            {
                if (!isFirst) builder.Append(S.DICTIONARY2);
                builder.Append($"{keyValue.Key}{S.DICTIONARY1}{keyValue.Value}");

                isFirst = false;
            }

            return builder.ToString();
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


