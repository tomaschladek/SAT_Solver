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
                var tournament = GenerateTournament(random, generation).ToList();
                int maxIndex = GetMaxIndex(tournament);
                var winner = tournament[maxIndex];
                yield return new FenotypDto(new BitArray(winner.Fenotyp), winner.Score, winner.SatResult);
            }
        }

        private static int GetMaxIndex(List<FenotypDto> tournament)
        {
            var maxScore = long.MinValue;
            var indexScore = 0;
            var maxIndex = int.MinValue;
            foreach (var fenotyp in tournament)
            {
                if (fenotyp.Score.CompareTo(maxScore) > 0 || maxIndex == int.MinValue)
                {
                    maxIndex = indexScore;
                    maxScore = fenotyp.Score;
                }
                indexScore++;
            }

            return maxIndex;
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