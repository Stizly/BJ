using BJ;
using BJ.PlayingStrategies;
using BJ_Blazor.Models;

namespace BJ_Blazor.Services
{
	public class BlackjackService
	{
		public BlackjackResult SimulateBlackjack(BlackjackSetupDTO setup)
		{
			List<decimal> profits = [];
			int bankruptcies = 0;
			Parallel.For(0, setup.SimulatedPlayers, i =>
			{
				var rules = new BlackjackRules(setup.ShoeSize, setup.DeckPenetration)
				{
					CanResplitAces = setup.CanResplitAces,
					BlackjackPayout = setup.BlackjackPayout,
					CanHitAcesAfterSplit = setup.CanHitAcesAfterSplit,
					DAS = setup.DAS,
					DealerHitsSoft17 = setup.DealerHitsSoft17,
					PairSplitLimit = setup.PairSplitLimits,
					IsSurrenderAllowed = setup.IsSurrenderAllowed
				};
				var dealer = new Dealer(setup.DealerHitsSoft17 ? new DealerStrategy_H17() : new DealerStrategy_S17());
				var table = new Table(rules, dealer);
				PlayingStrategy playingstrategy = setup.ShoeSize switch
				{
					1 => new PlayingStrategySD(),
					2 => new PlayingStrategyDD(),
					_ => new PlayingStrategyShoe()
				};

				if (setup.UseIllustrious18)
					playingstrategy.UseDeviations();
				if (!setup.DAS)
					playingstrategy.NDAS();
				if (!setup.DealerHitsSoft17)
					playingstrategy.S17();

				var player = new Player(
					setup.Bankroll,
					new BettingStrategy(BettingStrategies.BuildBetSpread(setup.BetAndHands[..3].Select(bh => (int)bh.Bet).ToArray(), setup.BetAndHands[3..].Select(bh => (int)bh.Bet).ToArray()), setup.BetAndHands[3..].Select(bh => bh.Hands).ToArray()),
					playingstrategy);

				var bjplayer = new BJPlayer(table, player);

				var bankroll = PlayBlackjack(bjplayer, setup.RoundsPerPlayerSimulated);
				profits.Add(bankroll - setup.Bankroll);
				if (bankroll <= 0)
					bankruptcies++;
			});

			return new BlackjackResult(profits) { Bankruptcies = bankruptcies, SetupDTO = setup };
		}

		private static decimal PlayBlackjack(BJPlayer bjplayer, int rounds)
		{
			var player = bjplayer.Player;
			var bj = bjplayer.Table;

			for (int i = 0; i < rounds; i++)
			{
				if (player.BankRoll < 0)
					return player.BankRoll;

				var (bet, hands) = player.GetBetAmount(bjplayer.Table.GetTrueCount());
				player.AddHands(hands);

				bj.Deal(player.Hands);
				bjplayer.Play(bet);
				bj.ClearHands(player);
				if (bj.DoReshuffleShoe())
				{
					bj.RefillShoe();
				}
			}

			return player.BankRoll;
		}
	}
}
