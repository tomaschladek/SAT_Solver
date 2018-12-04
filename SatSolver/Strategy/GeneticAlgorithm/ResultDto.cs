namespace SatSolver.Strategy.GeneticAlgorithm
{
    public class FormulaResultDto
    {
        public int Counter { get; set; }
        public ESatisfaction Satisfaction { get; set; }

        public FormulaResultDto(int counter, ESatisfaction satisfaction)
        {
            Counter = counter;
            Satisfaction = satisfaction;
        }
    }
}