using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;

namespace Solarflare.StrapiAPI
{
    public class BuildDataForFiles
    {
        internal string ConvertJsonToCsv(string jsonContent, string delimiter)
        {
            ExpandoObject[] expandObject = JsonConvert.DeserializeObject<ExpandoObject[]>(jsonContent) ?? throw new InvalidOperationException();

            foreach (var obj in expandObject)
            {
                //int dateTime = int.Parse(obj["created_at"]);
            }
            
            using var writer = new StringWriter();
            
            using (CsvWriter csv = new(writer, System.Globalization.CultureInfo.CurrentCulture))
            {
                var config = new CsvConfiguration(System.Globalization.CultureInfo.CurrentCulture)
                    {Delimiter = delimiter};

                csv.WriteRecords(expandObject as IEnumerable<dynamic>);
            }

            return writer.ToString();
        }

        internal void WriteToCsvFile(string dataToWrite)
        {
            var csvPath = Path.Combine("/Users/tommy/Desktop", "prize-draw-entries.csv");
            using (var streamWriter = new StreamWriter(csvPath))
            {
                using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    
                    File.WriteAllText(csvPath, dataToWrite);
                }
            }
        }
    }
}