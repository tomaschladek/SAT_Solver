using System.IO;

namespace SatSolver.Services
{
    public interface IIoProvider
    {
        StreamReader GetFileReader(string fullName);
        StreamWriter GetFileWrite(string fullName);
    }

    public class IoProvider : IIoProvider
    {
        public StreamReader GetFileReader(string fullName)
        {
            return new StreamReader(fullName);
        }
        
        public StreamWriter GetFileWrite(string fullName)
        {
            return new StreamWriter(fullName);
        }
    }
}