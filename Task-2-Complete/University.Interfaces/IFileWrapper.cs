using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Interfaces
{
    public interface IFileWrapper
    {
        void WriteAllText(string path, string contents);
        bool Exists(string path);
        string ReadAllText(string path);
    }
}
