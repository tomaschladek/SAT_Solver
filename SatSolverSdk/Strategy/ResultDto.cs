namespace SatSolverSdk.Strategy
{
    public class FormulaResultDto
    {
        public int Counter { get; set; }
        public ESatisfaction Satisfaction { get; set; }
        public long Weights { get; set; }

        public FormulaResultDto(int counter, ESatisfaction satisfaction, long weights)
        {
            Counter = counter;
            Satisfaction = satisfaction;
            Weights = weights;
        }
    }
}