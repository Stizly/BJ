using BJ;

namespace BJTesting
{
    [TestClass]
    public class BJTesting
    {
        private readonly static Table BJTABLE1 = new Table(new BlackjackRules(1, 0.75m));
        private readonly static Table BJTABLE2 = new Table(new BlackjackRules(2, 0.75m));
        private readonly static Table BJTABLE4 = new Table(new BlackjackRules(4, 0.75m));
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
            const int hits = 4;
            var bj2 = new Table(new BlackjackRules(2, 0.75m));
            bj2.ShuffleShoe(c => c.Value);
            for (int i = 0; i < hits; i++)
            {
                var hitcard = bj2.Hit();
            }

            Assert.AreEqual(hits, bj2.RunningCount);
            Assert.AreEqual(2, (int)bj2.GetTrueCount());
        }

        [TestMethod]
        public void RefillShoe_IsCorrect()
        {
            var bj1_dontreshuffle = new Table(new BlackjackRules(1, 0.7m));
            var bj1_doreshuffle = new Table(new BlackjackRules(1, 0.7m));

            for (int i = 0; i < 52 / 2; i++)
            {
                bj1_dontreshuffle.Hit();
                bj1_doreshuffle.Hit();
            }
            for (int i = 0; i < 52 / 4; i++)
            {
                bj1_doreshuffle.Hit();
            }

            Assert.IsFalse(bj1_dontreshuffle.DoReshuffleShoe());
            Assert.IsTrue(bj1_doreshuffle.DoReshuffleShoe());

            bj1_doreshuffle.RefillShoe();
            Assert.IsFalse(bj1_doreshuffle.DoReshuffleShoe());
            Assert.AreEqual(0, bj1_doreshuffle.RunningCount);
        }
    }
}