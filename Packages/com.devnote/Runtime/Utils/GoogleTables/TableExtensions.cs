using UnityEngine;


namespace DevNote
{
    public static class TableExtensions
    {

        public static string Get(this Table table, int row, Column column) 
            => table == null ? string.Empty : table.Cells[row - 1][(int)column];


        public static Column GetColumn(this Table table, string columnName)
        {
            for (int columnIndex = 0; columnIndex < table.Columns; columnIndex++)
            {
                var column = (Column)columnIndex;
                if (table.Get(row: 1, column) == columnName) return column;
            }

            string errorMessage = $"{Info.Prefix} Column with name \"{columnName}\" does not exist in Table \"{table.Key}\"";
            throw new System.Exception(errorMessage);
        }


        public static int GetRow(this Table table, Column keyColumn, string key)
        {
            for (int row = 1; row <= table.Rows; row++)
                if (table.Get(row, keyColumn) == key) return row;

            string errorMessage =
                $"{Info.Prefix} Key \"{key}\" doesn't exists; Table: \"{table.Key}\", Key column: {keyColumn}";

            throw new System.Exception(errorMessage);
        }

        public static bool TryGetRow(this Table table, Column keyColumn, string key, out int row)
        {
            row = -1;
            if (table == null) return false;

            for (row = 1; row <= table.Rows; row++)
                if (table.Get(row, keyColumn) == key)
                    return true;

            return false;
        }

        public static int GetInt(this Table table, Column keyColumn, Column valueColumn, string key)
        {
            if (table == null) return -1;

            int row = GetRow(table, keyColumn, key);
            return table.GetInt(row, valueColumn);
        }

        public static bool TryGetInt(this Table table, Column keyColumn, Column valueColumn, string key, out int value)
        {
            value = -1;
            if (table == null) return false;

            if (TryGetRow(table, keyColumn, key, out int row))
            {
                value = table.GetInt(row, valueColumn);
                return true;
            }

            return false;
        }

        public static float GetFloat(this Table table, int row, Column column)
        {
            float result = -1;
            if (table == null) return -1;

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
            if (table == null) return -1;

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

        public static bool GetBool(this Table table, int row, Column column)
        {
            if (table == null) return false;

            string dataString = table.Get(row, column);

            if (dataString.ToLower() == "true") return true;
            else if (dataString.ToLower() == "false") return false;
            else
            {
                Debug.LogError($"{Info.Prefix} Wrong bool format! Table: \"{table.Key}\", " +
                    $"Row: {row}, Column: {column}, Value: \"{dataString}\"");

                return false;
            }
        }



    }
}


