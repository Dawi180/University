using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using University.Interfaces;

namespace University.Services
{
    public class DataAccessService : IDataAccessService
    {
        private readonly IFileWrapper fileWrapper;

        public DataAccessService(IFileWrapper fileWrapper)
        {
            this.fileWrapper = fileWrapper;
        }

        public void SaveData<T>(string filePath, T data)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
                fileWrapper.WriteAllText(filePath, jsonData);
                Console.WriteLine($"Data saved to {filePath}");
            }
            catch (Exception ex)
            {
                // Handle exceptions here, e.g., log or throw.
                Console.WriteLine($"Error saving data to {filePath}: {ex.Message}");
                throw;
            }
        }

        public T LoadData<T>(string filePath)
        {
            try
            {
                if (fileWrapper.Exists(filePath))
                {
                    string jsonData = fileWrapper.ReadAllText(filePath);
                    T result = JsonConvert.DeserializeObject<T>(jsonData);
                    Console.WriteLine($"Data loaded from {filePath}");
                    return result;
                }
                else
                {
                    // Handle file not found scenario.
                    Console.WriteLine($"File not found: {filePath}");
                    return default(T); // Return default value for the type.
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here, e.g., log or throw.
                Console.WriteLine($"Error loading data from {filePath}: {ex.Message}");
                return default(T); // Return default value for the type.
                throw;
            }
        }
    }

}
