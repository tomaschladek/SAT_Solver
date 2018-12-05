using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SatSolver.Dtos;

namespace SatSolver.Services
{
    public class ReadSatManager : IReadSatManager
    {
        public IIoProvider IoProvider;

        public ReadSatManager()
        {
            IoProvider = new IoProvider();
        }

        public SatDefinitionDto ReadDefinition(string fullName)
        {
            SatDefinitionDto definition = null;
            var fileName = Path.GetFileName(fullName);
            // Read the file and display it line by line.  
            using (var file = IoProvider.GetFileReader(fullName))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    definition = ProcessLine(line, definition, fileName);
                }
                file.Close();
            }

            return definition;
        }

        private SatDefinitionDto ProcessLine(string line, SatDefinitionDto definition, string fileName)
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
                return new SatDefinitionDto(fileName,variableCount, clausesCount);
            }

            var variables = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            variables.Remove(variables.Last());
            definition.Clauses.Add(new ClausesDto(variables));
            return definition;
        }

        public void WriteDefinition(SatDefinitionDto definition, string fullName)
        {
            using (var file = IoProvider.GetFileWrite(fullName))
            {

                file.WriteLine(string.Join(" ", "p", "cnf", definition.VariableCount.ToString(),
                    definition.Clauses.Count.ToString()));

                file.WriteLine(string.Join(" ", new[] { "w" }.Concat(definition.Weights.Select(weight => weight.ToString()))));

                foreach (var clause in definition.Clauses)
                {
                    file.WriteLine(string.Join(" ", clause.Variables.Select(variable => variable.ToString()).Concat(new[] { 0.ToString() })));
                }
            }
        }

        public void AppendFile(string path, IEnumerable<string> data)
        {
            var value = string.Join("\t", data);
            File.AppendAllText(path, $"{value}{Environment.NewLine}");
        }
    }
}