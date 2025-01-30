using BJ;

namespace BJTesting
{
    [TestClass]
    public class BJTesting
    {
        [TestMethod]
        public void Shoe_IsCorrectSize()
        {
            var bj1 = new Blackjack(1);
            var bj2 = new Blackjack(2);
            var bj4 = new Blackjack(4);

            Assert.AreEqual(52, bj1.Shoe.Count);
            Assert.AreEqual(52 * 2, bj2.Shoe.Count);
            Assert.AreEqual(52 * 4, bj4.Shoe.Count);
        }
        [TestMethod]
        public void RunningCount_IsCorrectAtStart()
        {
            var bj1 = new Blackjack(1);
            Assert.AreEqual(0, bj1.RunningCount);
            Assert.AreEqual(0, bj1.GetTrueCount());

            var bj2 = new Blackjack(2);
            Assert.AreEqual(0, bj2.RunningCount);
            Assert.AreEqual(0, bj2.GetTrueCount());

            var bj4 = new Blackjack(4);
            Assert.AreEqual(0, bj4.RunningCount);
            Assert.AreEqual(0, bj4.GetTrueCount());
        }

        [TestMethod]
        public void RunningCount_UpdatesCorrectly()
        {
            const int hits = 4;
            var bj2 = new Blackjack(2);
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
            var bj1_dontreshuffle = new Blackjack(1)
            {
                DeckPenetration = 0.7m
            };

            var bj1_doreshuffle = new Blackjack(1)
            {
                DeckPenetration = 0.7m
            };

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