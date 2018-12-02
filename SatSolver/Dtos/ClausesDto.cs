using System.Collections.Generic;

namespace SatSolver.Dtos
{
    public class ClausesDto
    {
        public IList<int> Variables { get; }

        public ClausesDto(IList<int> variables)
        {
            Variables = variables;
        }
    }
}