using UnityEngine;


namespace DevNote
{
    public static class TableExtensions
    {

        public static int GetRow(this Table table, Column keyColumn, string key)
        {
            for (int row = 1; row <= table.Rows; row++)
                if (table.Get(row, keyColumn) == key) return row;

            string errorMessage =
                $"{Info.Prefix} Key \"{key}\" doesn't exists; Table: \"{table.Key}\", Key column: {keyColumn}";

            throw new System.Exception(errorMessage);
        }

        public static float GetFloat(this Table table, int row, Column column)
        {
            float result = -1;

            string dataString = table.Get(row, column);
            if (dataString == string.Empty || dataString.StartsWith('-')) return result;

            if (!float.TryParse(dataString, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.GetCultureInfo("en-US"), out result))
            {
                Debug.LogError($"{Info.Prefix} Table {table.Key}: Error parse Float {dataString}, " +
                    $"row - {row}, column - {column}.");
            }

            return result;
        }

        public static int GetInt(this Table table, int row, Column column)
        {
            int result = -1;

            string dataString = table.Get(row, column);
            if (dataString == string.Empty || dataString.StartsWith('-')) return result;

            if (!int.TryParse(dataString, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.GetCultureInfo("en-US"), out result))
            {
                Debug.LogError($"{Info.Prefix} Table {table.Key}: Error parse Int {dataString}, " +
                    $"row - {row}, column - {column}.");
            }

            return result;
        }

    }
}


