using BJ.PlayingStrategies;

namespace BJ
{
	public class Dealer : Player
	{
		public Card? Upcard { get; set; }
		public Card? Downcard { get; set; }
		public Hand Hand => Hands.First();
		public Dealer(PlayingStrategy dealerstrategy) : base(0, new(BettingStrategies.BuildBetSpread([0], [0]), 0), dealerstrategy)
		{
			Hands = [new Hand()];
		}

	}
}
