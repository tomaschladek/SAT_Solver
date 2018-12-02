using System.Collections;

namespace SatSolver.Dtos
{
    public class ResultDto
    {
        public long Duration { get; set; }
        public BitArray Solution { get; set; }

        public ResultDto(long duration, BitArray solution)
        {
            Duration = duration;
            Solution = solution;
        }
    }
}