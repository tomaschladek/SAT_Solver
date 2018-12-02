using SatSolver.Dtos;

namespace SatSolver
{
    public interface IReadSatManager
    {
        SatDefinitionDto ReadDefinition(string fullName);
    }
}