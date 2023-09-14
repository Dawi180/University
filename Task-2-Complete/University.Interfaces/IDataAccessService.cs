using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Interfaces
{
    public interface IDataAccessService
    {
        void SaveData<T>(string filePath, T data);
        T LoadData<T>(string filePath);
    }
}
