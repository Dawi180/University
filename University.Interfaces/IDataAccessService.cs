using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Interfaces
{
    public interface IDataAccessService<T>
    {
        void SaveData(T data, string filePath);
        T ReadData(string filePath);
    }
}
