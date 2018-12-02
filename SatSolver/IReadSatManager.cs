using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SatSolver
{
    public interface IReadSatManager
    {
        SatDefinitionDto ReadDefinition(string fullName);
    }

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
                    definition = ProcessLine(line, definition, fullName);
                }
                file.Close();
            }

            return definition;
        }

        private SatDefinitionDto ProcessLine(string line, SatDefinitionDto definition, string fullName)
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
                return new SatDefinitionDto(fullName,variableCount,clausesCount);
            }

            var variables = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            variables.Remove(variables.Last());
            definition.Clauses.Add(new ClausesDto(variables));
            return definition;
        }
    }

    public class SatDefinitionDto
    {
        private string FullName { get; }
        public int VariableCount { get; }
        public IList<int> Weights { get; set; }
        public List<ClausesDto> Clauses { get; }

        public SatDefinitionDto(string fullName, int variableCount,int count)
        {
            FullName = fullName;
            VariableCount = variableCount;
            Clauses = new List<ClausesDto>(count);
            Weights = Enumerable.Repeat(1,VariableCount).ToList();
        }
    }

    public class ClausesDto
    {
        public IList<int> Variables { get; }

        public ClausesDto(IList<int> variables)
        {
            Variables = variables;
        }
    }
}