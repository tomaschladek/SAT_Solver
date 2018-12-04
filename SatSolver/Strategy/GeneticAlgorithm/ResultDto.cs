namespace SatSolver.Strategy.GeneticAlgorithm
{
    public class ResultDto
    {
        public int Counter { get; set; }
        public ESatisfaction Satisfaction { get; set; }

        public ResultDto(int counter, ESatisfaction satisfaction)
        {
            Counter = counter;
            Satisfaction = satisfaction;
        }
    }
}