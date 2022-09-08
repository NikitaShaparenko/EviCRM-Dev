using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using System.Text;
using static EviCRM.Server.Pages.Statistics.TimeAccounting.Statistics_WorkingHoursChart;

namespace EviCRM.Server.Core
{
    public class ExcelModel
    {
        public string DateTimeColumn { get; set; }
        public string ActionColumn { get; set; }
        public string FilePathColumn { get; set; }
        public string FilePathDescription { get; set; }

        public string FilePathDetailsColumn { get; set; }

        public string FilePathLastLogColumn { get; set; }

    }
    
    public class MiCommonModel2
    {
        public List<string> datetime { get; set; }
        public List<string> application_path { get; set; }
        public List<string> application_name { get; set; }
        public List<string> appplication_desc { get; set; }
        public List<string> appplication_data { get; set; }

        public List<StatisticsCat> category { get; set; }
    }

    public class MiCommonModel
    {
        public string datetime { get; set; }
        public string application_path { get; set; }
        public string application_name { get; set; }
        public string appplication_desc { get; set; }
        public string appplication_data { get; set; }

        public StatisticsCat category { get; set; }
    }

    internal class StatHandlerCore
    {

    }

     public class MipkoIntegratorCore
    {

            public void DebugReadInCSV2(string fileName)
            {
                var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Encoding = Encoding.UTF8, // Our file uses UTF-8 encoding.
                    Delimiter = ";" // The delimiter is a comma.
                };

                using (var fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var textReader = new StreamReader(fs, Encoding.UTF8))
                    using (var csv = new CsvReader(textReader, configuration))
                    {
                        var data = csv.GetRecords<ExcelModel>();

                        foreach (var person in data)
                        {
                            // Do something with values in each row
                            Console.WriteLine(person);
                        }
                    }
                }
            }


            public List<string> GetCSV_byColumnID(string fileName, int columnID)
            {
                List<string> lstColumn = new List<string>();

                var path = fileName;

                using (TextFieldParser csvParser = new TextFieldParser(path, Encoding.GetEncoding(1251)))
                {
                    csvParser.CommentTokens = new string[] { "#" };
                    csvParser.SetDelimiters(new string[] { ";" });
                    csvParser.HasFieldsEnclosedInQuotes = true;

                    // Skip the row with the column names
                    csvParser.ReadLine();

                    while (!csvParser.EndOfData)
                    {
                        // Read current line fields, pointer moves to the next line.
                        string[] fields = csvParser.ReadFields();

                        Console.WriteLine("Количество столбцов: " + fields.Length);
                        lstColumn.Add(fields[columnID]);
                    }
                }
                return lstColumn;
            }

            public void DebugReadInCSV(string fileName)
            {
                var path = fileName; // Habeeb, "Dubai Media City, Dubai"
                using (TextFieldParser csvParser = new TextFieldParser(path, Encoding.GetEncoding(1251)))
                {
                    csvParser.CommentTokens = new string[] { "#" };
                    csvParser.SetDelimiters(new string[] { ";" });
                    csvParser.HasFieldsEnclosedInQuotes = true;

                    // Skip the row with the column names
                    csvParser.ReadLine();

                    while (!csvParser.EndOfData)
                    {
                        // Read current line fields, pointer moves to the next line.
                        string[] fields = csvParser.ReadFields();

                        Console.WriteLine("Количество столбцов: " + fields.Length);
                        Console.WriteLine(fields[0]);
                        Console.WriteLine(fields[1]);
                        Console.WriteLine(fields[2]);
                        Console.WriteLine(fields[3]);
                        Console.WriteLine(fields[4]);
                        Console.WriteLine(fields[5]);
                    }
                }
            }
        }
    }