using System.Collections.Generic;
using SatSolver.Dtos;

namespace SatSolver.Services
{
    public interface IReadSatManager
    {
        SatDefinitionDto ReadDefinition(string fullName);

        void WriteDefinition(SatDefinitionDto definition, string fullName);
        void AppendFile(string path, IEnumerable<string> data);
    }
}