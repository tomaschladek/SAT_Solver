using System.Collections.Generic;
using System.Linq;

namespace SatSolverSdk.Dtos
{
    public class SatDefinitionDto
    {
        public int VariableCount { get; }
        public IList<int> Weights { get; set; }
        public List<ClausesDto> Clauses { get; }
        public string FileName { get; set; }

        public SatDefinitionDto(string fileName, int variableCount,int count)
        {
            FileName = fileName;
            VariableCount = variableCount;
            Clauses = new List<ClausesDto>(count);
            Weights = Enumerable.Repeat(1,VariableCount).ToList();
        }
    }
}