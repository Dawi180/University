using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using University.Controls;
using Newtonsoft.Json;
using University.Interfaces;

namespace University.Services
{
    
    public class DataAccessService<T> : IDataAccessService<T>
    {
        public void SaveData(T data, string filePath)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, jsonData);
        }

        public T ReadData(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(jsonData);
            }
            else
            {
                throw new FileNotFoundException("File not found", filePath);
            }
        }
    }
}
