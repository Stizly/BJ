namespace BJ_Blazor.Models
{
	public class BlackjackResult
	{
		public BlackjackSetupDTO SetupDTO { get; set; }
		public int Bankruptcies { get; set; }
		public int Iteration { get; set; }
		public string ErrorMessage { get; set; }

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

		public string GetRulesList()
		{
			List<string> rules = [];
			if (SetupDTO.DAS)
				rules.Add("DAS");
			else
				rules.Add("NDAS");

			if (SetupDTO.DealerHitsSoft17)
				rules.Add("H17");
			else
				rules.Add("S17");

			if (SetupDTO.CanHitAcesAfterSplit)
				rules.Add("HSA");
			else
				rules.Add("NHSA");

			if (SetupDTO.IsSurrenderAllowed)
				rules.Add("SURR");
			else
				rules.Add("NSURR");

			rules.Add($"SP{SetupDTO.PairSplitLimits}");

			rules.Add($"Blackjack pays {SetupDTO.BlackjackPayout}x");

			return string.Join(", ", rules);
		}
	}

	public class BlackjackSetupDTO
	{
		public int ShoeSize { get; set; }
		public int DeckPenetration { get; set; }
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
