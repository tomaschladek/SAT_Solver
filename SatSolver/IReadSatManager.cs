using SatSolver.Dtos;

namespace SatSolver
{
    public interface IReadSatManager
    {
        SatDefinitionDto ReadDefinition(string fullName);

        void WriteDefinition(SatDefinitionDto definition, string fullName);
    }
}