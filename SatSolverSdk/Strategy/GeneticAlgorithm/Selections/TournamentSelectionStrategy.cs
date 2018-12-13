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

        protected override IEnumerable<FenotypDto> SelectByCriteria(SatDefinitionDto definition, Random random,
            List<FenotypDto> generation, IDictionary<int, FormulaResultDto> cache)
        {
            for (int index = StartCount; index < generation.Count; index++)
            {
                var tournamentCandidates = GenerateTournament(random, generation);
                var tournament = tournamentCandidates.ToList();
                var maxScore = tournament.Max(item => item.Score);
                var winner = tournament.First(item => item.Score == maxScore);
                yield return new FenotypDto(new BitArray(winner.Fenotyp),winner.Score,winner.SatResult);
            }
        }

        private IEnumerable<FenotypDto> GenerateTournament(Random random, List<FenotypDto> generation)
        {
            for (int tournamentRound = 0; tournamentRound < TournamentSize; tournamentRound++)
            {
                var randomIndex = random.Next(0, generation.Count);
                yield return generation[randomIndex];
            }
        }
    }
}