using System;
using System.IO;
using System.Linq;
using SatSolver.Dtos;

namespace SatSolver
{
    public class ReadSatManager : IReadSatManager
    {
        public SatDefinitionDto ReadDefinition(string fullName)
        {
            SatDefinitionDto definition = null;
            // Read the file and display it line by line.  
            using (var file = new StreamReader(fullName))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    definition = ProcessLine(line, definition);
                }
                file.Close();
            }

            return definition;
        }

        private SatDefinitionDto ProcessLine(string line, SatDefinitionDto definition)
        {
            if (line.StartsWith("c") || line.StartsWith("%") || line.StartsWith("0") || string.IsNullOrEmpty(line))
            {
                return definition;
            }

            if (line.StartsWith("w"))
            {
                definition.Weights = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToList();
                if (definition.Weights.Count != definition.VariableCount)
                {
                    throw new ArgumentException($"Expected {definition.VariableCount} but {definition.Weights.Count} provided!");
                }

                return definition;
            }

            if (line.StartsWith("p"))
            {
                var properties = line.Split(' ',StringSplitOptions.RemoveEmptyEntries);
                var variableCount = int.Parse(properties[2]);
                var clausesCount = int.Parse(properties[3]);
                return new SatDefinitionDto(variableCount,clausesCount);
            }

            var variables = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            variables.Remove(variables.Last());
            definition.Clauses.Add(new ClausesDto(variables));
            return definition;
        }
    }
}