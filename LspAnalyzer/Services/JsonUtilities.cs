using System;
using System.Data;
using Newtonsoft.Json.Linq;

namespace LspAnalyzer.Services
{
    public class JsonUtility
    {
        /// <summary>
        /// Convert a JSon Object into a DataTable with the two columns Name, Value
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public static DataTable JObjectToDataTable(JObject jObject)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            char[] cStart = "{\r\n".ToCharArray();
            foreach (JProperty property in jObject.Properties())
            {
                string value = property.Value.ToString().Trim(cStart).Trim('}').TrimStart(' ').Replace("\r\n ", "\r\n");
                dt.Rows.Add(property.Name, value);
                Console.WriteLine(property.Name + " - " + property.Value);
            }

            return dt;
        }
    }
}
