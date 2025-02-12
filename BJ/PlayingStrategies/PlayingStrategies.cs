namespace BJ.PlayingStrategies
{
	public static partial class PlayingStrategies
	{
		private static ActionEnum H(GameState gamestate) => ActionEnum.H;
		private static ActionEnum Dh(GameState gamestate) => gamestate.CanDoubleDown() ? ActionEnum.D : ActionEnum.H;
		private static ActionEnum Ds(GameState gamestate) => gamestate.CanDoubleDown() ? ActionEnum.D : ActionEnum.S;
		private static ActionEnum S(GameState gamestate) => ActionEnum.S;
		private static ActionEnum Rh(GameState gamestate) => gamestate.CanSurrender() ? ActionEnum.R : ActionEnum.H;
		private static ActionEnum Rs(GameState gamestate) => gamestate.CanSurrender() ? ActionEnum.R : ActionEnum.S;
		private static ActionEnum P(GameState gamestate) => ActionEnum.P;
		private static ActionEnum Rp(GameState gamestate) => gamestate.CanSurrender() ? ActionEnum.R : ActionEnum.P;

		//It is unclear if (0, H) means hit at or above 0, or hit at or below 0, so for 0-based deviations, use methods like this
		private static (int, Func<GameState, ActionEnum>) Hlte0 => (0, (GameState gs) => gs.RunningCount < 0 ? ActionEnum.H : ActionEnum.S);
		private static (int, Func<GameState, ActionEnum>) Sgte0 => (0, (GameState gs) => gs.RunningCount > 0 ? ActionEnum.S : ActionEnum.H);
	}

	public enum ActionEnum
	{
		H, D, S, R, P
	}

	public abstract class PlayingStrategy
	{
		public Func<GameState, ActionEnum>[][] HardHandStrategy { get; set; }
		public Func<GameState, ActionEnum>[][] SoftHandStrategy { get; set; }
		public Func<GameState, ActionEnum>[][] PairsStrategy { get; set; }
		public (int AtTrueCount, Func<GameState, ActionEnum> Deviation)?[][] HardHandDeviations { get; set; }
		public (int AtTrueCount, Func<GameState, ActionEnum> Deviation)?[][] PairsDeviations { get; set; }
		public Func<GameState, bool>? InsuranceDeviation { get; set; }

		public PlayingStrategy(Func<GameState, ActionEnum>[][][] HardSoftPairStrategy)
		{
			HardHandStrategy = HardSoftPairStrategy[0];
			SoftHandStrategy = HardSoftPairStrategy[1];
			PairsStrategy = HardSoftPairStrategy[2];
		}

		public PlayingStrategy UseDeviations()
		{
			HardHandDeviations = PlayingStrategies.Deviations_HardHand;
			PairsDeviations = PlayingStrategies.Deviations_Pairs;
			InsuranceDeviation = PlayingStrategies.TakeInsuranceDeviation;
			return this;
		}

		public ActionEnum GetAction(GameState gamestate)
		{
			var dealerupcardindex = GetDealerUpcardIndex(gamestate.DealerUpcard);
			if (gamestate.CanSplit())
			{
				var rowindex = GetSplitRowIndex(gamestate.PlayerHand);
				var deviation = PairsDeviations?[rowindex][dealerupcardindex];
				if (deviation != null && UseDeviation(deviation.Value.AtTrueCount, gamestate.TrueCount))
					return deviation.Value.Deviation(gamestate);
				else
					return PairsStrategy[GetSplitRowIndex(gamestate.PlayerHand)][dealerupcardindex](gamestate);
			}
			else if (gamestate.PlayerHand.IsSoft)
			{
				return SoftHandStrategy[GetSoftHandRowIndex(gamestate.PlayerHand)][dealerupcardindex](gamestate);
			}
			else
			{
				var rowindex = GetHardHandRowIndex(gamestate.PlayerHand);
				var deviation = HardHandDeviations?[rowindex][dealerupcardindex];

				if (deviation != null && UseDeviation(deviation.Value.AtTrueCount, gamestate.TrueCount))
					return deviation.Value.Deviation(gamestate);
				else
					return HardHandStrategy[rowindex][dealerupcardindex](gamestate);
			}
		}

		public bool DoTakeInsurrance(GameState gamestate)
		{
			if (InsuranceDeviation == null)
				return false;

			return InsuranceDeviation(gamestate);
		}

		private static int GetDealerUpcardIndex(Card upcard) => upcard.Rank == "A" ? 9 : upcard.Value - 2;
		private static int GetSplitRowIndex(IHand hand) => hand.Cards[0].Rank == "A" ? 9 : hand.Cards[0].Value - 2;
		private static int GetSoftHandRowIndex(IHand hand) => hand.Value - 12;
		private static int GetHardHandRowIndex(IHand hand) => hand.Value - 4;
		private static bool UseDeviation(int attruecount, int truecount) => attruecount == 0 || (attruecount < 0 && truecount <= attruecount) || (attruecount > 0 && truecount >= attruecount);
	}
}
