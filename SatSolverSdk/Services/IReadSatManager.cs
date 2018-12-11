using SatSolverSdk.Dtos;

namespace SatSolverSdk.Services
{
    public interface IReadSatManager
    {
        SatDefinitionDto ReadDefinition(string fullName);

        void WriteDefinition(SatDefinitionDto definition, string fullName);
        void AppendFile(string path, params string[] data);
    }
}