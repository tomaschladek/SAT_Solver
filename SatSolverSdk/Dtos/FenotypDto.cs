using System.Collections;
using SatSolverSdk.Strategy;

namespace SatSolverSdk.Dtos
{
    public class FenotypDto
    {
        public BitArray Fenotyp { get; }

        public long Score { get; set; }

        public FormulaResultDto SatResult { get; set; }

        public FenotypDto(BitArray fenotyp, long score, FormulaResultDto satResult)
        {
            SatResult = satResult;
            Fenotyp = fenotyp;
            Score = score;
        }
    }
}