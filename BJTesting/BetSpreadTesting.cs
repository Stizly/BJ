using BJ;
using BJ.PlayingStrategies;

namespace BJTesting
{
	[TestClass]
	public class BetSpreadTesting
	{
		private static readonly Player PLAYER_1HAND = new(1000, new BettingStrategy(BettingStrategies.BuildBetSpread([0, 25], [25, 25, 50, 75, 100])), new PlayingStrategy_DD_H17());
		private static readonly Player PLAYER_2HANDSAT2 = new(1000, new BettingStrategy(BettingStrategies.BuildBetSpread([0, 25], [25, 25, 50, 75, 100]), [1, 1, 2]), new PlayingStrategy_DD_H17());

		[TestMethod]
		public void BuildBetTestSpread_IsCorrectForNegativeCounts()
		{
			var p1H_TCd1 = PLAYER_1HAND.GetBetAmount(-1);
			Assert.AreEqual(25, p1H_TCd1.Bet);
			Assert.AreEqual(1, p1H_TCd1.Hands);

			var p1H_TCd2 = PLAYER_1HAND.GetBetAmount(-2);
			Assert.AreEqual(0, p1H_TCd2.Bet);
			Assert.AreEqual(1, p1H_TCd2.Hands);

			var p1H_TCd3 = PLAYER_1HAND.GetBetAmount(-3);
			Assert.AreEqual(0, p1H_TCd3.Bet);
			Assert.AreEqual(1, p1H_TCd3.Hands);
		}

		[TestMethod]
		public void BuildBetTestSpread_IsCorrectForPositiveCounts()
		{
			var p1H_TCd1 = PLAYER_1HAND.GetBetAmount(0);
			Assert.AreEqual(25, p1H_TCd1.Bet);
			Assert.AreEqual(1, p1H_TCd1.Hands);

			var p1H_TCp2 = PLAYER_1HAND.GetBetAmount(2);
			Assert.AreEqual(50, p1H_TCp2.Bet);
			Assert.AreEqual(1, p1H_TCp2.Hands);

			var p1H_TCp4 = PLAYER_1HAND.GetBetAmount(4);
			Assert.AreEqual(100, p1H_TCp4.Bet);
			Assert.AreEqual(1, p1H_TCp4.Hands);

			var p1H_TCp6 = PLAYER_1HAND.GetBetAmount(6);
			Assert.AreEqual(100, p1H_TCp6.Bet);
			Assert.AreEqual(1, p1H_TCp6.Hands);
		}

		[TestMethod]
		public void BuildBetTestSpread_IsCorrectForNegativeCountsMultipleHands()
		{
			var p1H_TCd1 = PLAYER_2HANDSAT2.GetBetAmount(-1);
			Assert.AreEqual(25, p1H_TCd1.Bet);
			Assert.AreEqual(1, p1H_TCd1.Hands);

			var p1H_TCd2 = PLAYER_2HANDSAT2.GetBetAmount(-2);
			Assert.AreEqual(0, p1H_TCd2.Bet);
			Assert.AreEqual(1, p1H_TCd2.Hands);

			var p1H_TCd3 = PLAYER_2HANDSAT2.GetBetAmount(-3);
			Assert.AreEqual(0, p1H_TCd3.Bet);
			Assert.AreEqual(1, p1H_TCd3.Hands);
		}

		[TestMethod]
		public void BuildBetTestSpread_IsCorrectForPositiveCountsMultipleHands()
		{
			var p1H_TCd1 = PLAYER_2HANDSAT2.GetBetAmount(0);
			Assert.AreEqual(25, p1H_TCd1.Bet);
			Assert.AreEqual(1, p1H_TCd1.Hands);

			var p1H_TCp2 = PLAYER_2HANDSAT2.GetBetAmount(2);
			Assert.AreEqual(50, p1H_TCp2.Bet);
			Assert.AreEqual(2, p1H_TCp2.Hands);

			var p1H_TCp4 = PLAYER_2HANDSAT2.GetBetAmount(4);
			Assert.AreEqual(100, p1H_TCp4.Bet);
			Assert.AreEqual(2, p1H_TCp4.Hands);

			var p1H_TCp6 = PLAYER_2HANDSAT2.GetBetAmount(6);
			Assert.AreEqual(100, p1H_TCp6.Bet);
			Assert.AreEqual(2, p1H_TCp6.Hands);
		}
	}
}
