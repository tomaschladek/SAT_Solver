using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SatSolverSdk.Dtos;
using SatSolverSdk.Strategy.GeneticAlgorithm.Corrections;

namespace SatSolverSdk.Strategy.GeneticAlgorithm.Selections
{
    public class TournamentSelectionStrategy : AbstractSelectionStrategy
    {
        public override string Id => $"Tournament-Size:{TournamentSize}-E:{ElitesCount}-W:{WeakestsCount}-Corr:{CorrectionStrategy.Id}";

        public TournamentSelectionStrategy(int tournamentSize) : this(tournamentSize, 0, 0, new NoCorrectionStrategy())
        {

        }

        public TournamentSelectionStrategy(int tournamentSize, int elitesCount, int weakestsCount, ICorrectionStrategy correctionStrategy) : base(elitesCount, weakestsCount, correctionStrategy)
        {
            TournamentSize = tournamentSize;
        }

        private int TournamentSize { get; set; }

        protected override IEnumerable<BitArray> SelectByCriteria(SatDefinitionDto definition, Random random,
            List<BitArray> generation, IDictionary<int, FormulaResultDto> cache)
        {
            for (int index = StartCount; index < generation.Count; index++)
            {
                var tournamentCandidates = GenerateTournament(random, generation);
                var tournament = tournamentCandidates
                    .Select((fenotyp, tournamentInde) => new
                    {
                        Index = tournamentInde,
                        Fenotyp = fenotyp,
                        Score = ScoreComputation.GetScores(definition, new List<BitArray>{fenotyp},cache).Single()
                    })
                    .ToList();
                var maxScore = tournament.Max(item => item.Score.Score);
                yield return new BitArray(tournament.First(item => item.Score.Score == maxScore).Fenotyp);
            }
        }

        private IEnumerable<BitArray> GenerateTournament(Random random, List<BitArray> generation)
        {
            for (int tournamentRound = 0; tournamentRound < TournamentSize; tournamentRound++)
            {
                var randomIndex = random.Next(0, generation.Count);
                yield return generation[randomIndex];
            }
        }
    }
}