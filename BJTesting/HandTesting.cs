using BJ;

namespace BJTesting
{
	[TestClass]
	public class HandTesting
	{
		[TestMethod]
		public void IsBlackjack_IsCorrect()
		{
			var handJ = new Hand();
			handJ.AddCard(new Card("A", SuitEnum.Diamonds), false);
			handJ.AddCard(new Card("J", SuitEnum.Diamonds), false);

			var handQ = new Hand();
			handQ.AddCard(new Card("A", SuitEnum.Diamonds), false);
			handQ.AddCard(new Card("Q", SuitEnum.Diamonds), false);

			var handK = new Hand();
			handK.AddCard(new Card("A", SuitEnum.Diamonds), false);
			handK.AddCard(new Card("K", SuitEnum.Diamonds), false);

			var hand10 = new Hand();
			hand10.AddCard(new Card("A", SuitEnum.Diamonds), false);
			hand10.AddCard(new Card("10", SuitEnum.Diamonds), false);

			Assert.IsTrue(handJ.IsBlackjack);
			Assert.IsTrue(handQ.IsBlackjack);
			Assert.IsTrue(handK.IsBlackjack);
			Assert.IsTrue(hand10.IsBlackjack);
		}
	}
}
