using BJ;

namespace BJTesting
{
	[TestClass]
	public class BJTesting
	{
		private readonly static Table BJTABLE1 = new Table(new BlackjackRules(1, 75));
		private readonly static Table BJTABLE2 = new Table(new BlackjackRules(2, 75));
		private readonly static Table BJTABLE4 = new Table(new BlackjackRules(4, 75));
		[TestMethod]
		public void Shoe_IsCorrectSize()
		{
			Assert.AreEqual(52, BJTABLE1.Shoe.Count);
			Assert.AreEqual(52 * 2, BJTABLE2.Shoe.Count);
			Assert.AreEqual(52 * 4, BJTABLE4.Shoe.Count);
		}
		[TestMethod]
		public void RunningCount_IsCorrectAtStart()
		{
			Assert.AreEqual(0, BJTABLE1.RunningCount);
			Assert.AreEqual(0, BJTABLE1.GetTrueCount());

			Assert.AreEqual(0, BJTABLE2.RunningCount);
			Assert.AreEqual(0, BJTABLE2.GetTrueCount());

			Assert.AreEqual(0, BJTABLE4.RunningCount);
			Assert.AreEqual(0, BJTABLE4.GetTrueCount());
		}

		[TestMethod]
		public void RunningCount_UpdatesCorrectly()
		{
			var bj2 = new Table(new BlackjackRules(2, 75));
			bj2.ShuffleShoe(c => c.Value);
			for (int i = 0; i < 4; i++)
			{
				var hitcard = bj2.Hit();
			}

			Assert.AreEqual(4, bj2.RunningCount);
			Assert.AreEqual(2, bj2.GetTrueCount());
		}

		[TestMethod]
		public void RefillShoe_IsCorrect()
		{
			var bj1_dontreshuffle = new Table(new BlackjackRules(1, 70));
			var bj1_doreshuffle = new Table(new BlackjackRules(1, 70));
			var bj6_doreshuffle = new Table(new BlackjackRules(6, 80));

			for (int i = 0; i < 52 / 2; i++)
			{
				bj1_dontreshuffle.Hit();
				bj1_doreshuffle.Hit();
			}
			for (int i = 0; i < 52 / 4; i++)
			{
				bj1_doreshuffle.Hit();
			}
			for (int i = 0; i < 52 * 6 * 9 / 10; i++)
			{
				bj6_doreshuffle.Hit();
			}

			Assert.IsFalse(bj1_dontreshuffle.DoReshuffleShoe());
			Assert.IsTrue(bj1_doreshuffle.DoReshuffleShoe());

			bj1_doreshuffle.RefillShoe();
			Assert.IsFalse(bj1_doreshuffle.DoReshuffleShoe());
			Assert.AreEqual(0, bj1_doreshuffle.RunningCount);
			Assert.AreEqual(0, bj1_doreshuffle.GetTrueCount());

			Assert.IsTrue(bj6_doreshuffle.DoReshuffleShoe());
			bj6_doreshuffle.RefillShoe();
			Assert.IsFalse(bj6_doreshuffle.DoReshuffleShoe());
			Assert.AreEqual(0, bj6_doreshuffle.RunningCount);
			Assert.AreEqual(0, bj6_doreshuffle.GetTrueCount());
		}
	}
}