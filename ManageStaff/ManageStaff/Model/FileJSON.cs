using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace ManageStaff.Model
{
    public class FileJSON
    {
        private string _cityName;
        private string _workshopName;
        private string _employeeName;
        private string _brigadeTime;
        private string _shiftName;
        public string City
        {
            get => _cityName; 
            set => _cityName = value; 
        }
        public string Workshop
        {
            get => _workshopName; 
            set => _workshopName = value; 
        }
        public string Employee
        {
            get => _employeeName; 
            set => _employeeName = value; 
        }
        public string Brigade
        {
            get => _brigadeTime; 
            set => _brigadeTime = value;
        }
        public string Shift
        {
            get => _shiftName; 
            set => _shiftName = value; 
        }

        public FileJSON(string cityName, string workshopName, string employeeName, string brigadeTime, string shiftName)
        {
            City = cityName; Workshop = workshopName; Employee = employeeName; Brigade = brigadeTime; Shift = shiftName; 
        }

        public bool Save(FileJSON infoJson)
        {
            bool result;
            // сохранение данных
            using (FileStream fs = new FileStream("TechInfo.json", FileMode.Create))
            {
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    WriteIndented = true
                };
                JsonSerializer.Serialize(fs, infoJson, options);
                result = true;
            }
            return result;
        }
    }
}
