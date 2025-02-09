namespace BJ_Blazor.Models
{
	public class BlackjackResult
	{
		public BlackjackSetupDTO SetupDTO { get; set; }
		public int Bankruptcies { get; set; }
		public int Iteration { get; set; }

		public int TotalRounds => SetupDTO.RoundsPerPlayerSimulated * SetupDTO.SimulatedPlayers;
		public decimal AverageProfit { get; private set; }
		public decimal StandardDeviation { get; private set; }
		public decimal ProfitPerHour => AverageProfit / SetupDTO.RoundsPerPlayerSimulated * SetupDTO.RoundsPerHour;
		public decimal MaxProfit { get; private set; }
		public decimal RiskOfRuin => Bankruptcies * 100 / (decimal)SetupDTO.SimulatedPlayers;
		public string BetSpread => string.Join(", ", SetupDTO.BetAndHands.Select((bh, index) => $"({index - 3}: ${bh.Bet} x{bh.Hands})"));

		public BlackjackResult(IEnumerable<decimal> profits)
		{
			AverageProfit = profits.Average();
			StandardDeviation = (decimal)Math.Sqrt(profits.Average(p => Math.Pow((double)(p - AverageProfit), 2)));
			MaxProfit = profits.Max();
		}
	}

	public class BlackjackSetupDTO
	{
		public int ShoeSize { get; set; }
		public decimal DeckPenetration { get; set; }
		public decimal BlackjackPayout { get; set; }
		public int PairSplitLimits { get; set; }
		public bool DAS { get; set; } = true;
		public bool IsSurrenderAllowed { get; set; } = false;
		public bool DealerHitsSoft17 { get; set; } = true;
		public bool CanHitAcesAfterSplit { get; set; } = false;
		public bool CanResplitAces { get; set; } = false;
		public List<BetAndHandCount> BetAndHands { get; set; }
		public decimal Bankroll { get; set; }
		public int RoundsPerHour { get; set; }
		public int RoundsPerPlayerSimulated { get; set; }
		public int SimulatedPlayers { get; set; }
		public decimal BetUnit { get; set; }
		public bool UseIllustrious18 { get; set; }


		public BlackjackSetupDTO Copy()
		{
			BlackjackSetupDTO copy = (BlackjackSetupDTO)MemberwiseClone();
			copy.BetAndHands = new List<BetAndHandCount>();
			foreach (var bh in BetAndHands)
			{
				copy.BetAndHands.Add(new BetAndHandCount { Bet = bh.Bet, Hands = bh.Hands });
			}
			return copy;
		}
	}

	public class BetAndHandCount
	{
		public decimal Bet { get; set; }
		public int Hands { get; set; }
	}
}
