using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace WindowsFormsApp4
{
    public static class CsvHelperService
    {
        public static List<Student> ReadStudents(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<Student>().ToList();
            }
        }
    }
}
