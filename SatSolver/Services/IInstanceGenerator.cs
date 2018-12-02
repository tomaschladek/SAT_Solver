using System;
using System.IO;

namespace SatSolver.Services
{
    public interface IInstanceGenerator
    {
        void Generate(string folderPath, string folderName);
    }

    public class InstanceGenerator : IInstanceGenerator
    {
        private IReadSatManager _reader;

        public InstanceGenerator()
        {
            _reader = new ReadSatManager();
        }

        public void Generate(string folderPath, string folderName)
        {
            var targetPath = Path.Combine(folderPath, folderName);
            Directory.CreateDirectory(targetPath);
            var random = new Random();
            foreach (var file in Directory.GetFiles(folderPath))
            {
                var definition = _reader.ReadDefinition(file);
                for (int index = 0; index < definition.Weights.Count; index++)
                {
                    definition.Weights[index] = random.Next(1, definition.Weights.Count);
                }

                _reader.WriteDefinition(definition, Path.Combine(targetPath, Path.GetFileName(file)));

            }
        }
    }
}