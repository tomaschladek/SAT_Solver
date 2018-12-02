using System.Collections.Generic;
using System.Linq;

namespace SatSolver.Dtos
{
    public class SatDefinitionDto
    {
        public int VariableCount { get; }
        public IList<int> Weights { get; set; }
        public List<ClausesDto> Clauses { get; }

        public SatDefinitionDto(int variableCount,int count)
        {
            VariableCount = variableCount;
            Clauses = new List<ClausesDto>(count);
            Weights = Enumerable.Repeat(1,VariableCount).ToList();
        }
    }
}