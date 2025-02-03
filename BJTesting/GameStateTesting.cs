using BJ;

namespace BJTesting
{
    [TestClass]
    public class GameStateTesting
    {
        [TestMethod]
        public void CanSplit_IsCorrectForNoSplits()
        {
            var rules = new BlackjackRules(1, 1)
            {
                PairSplitLimit = 1,
            };
            var hand = new Hand();
            hand.AddCard(new("2"), false);
            hand.AddCard(new("2"), false);


            GameState gs = new()
            {
                PlayerHand = hand,
                Rules = rules
            };

            Assert.IsTrue(gs.CanSplit());
        }

        [TestMethod]
        public void CanSplit_IsCorrectForSplitsPastLimit()
        {
            var rules = new BlackjackRules(1, 1)
            {
                PairSplitLimit = 1,
            };

            var hand1 = new Hand();
            hand1.AddCard(new("2"), false);
            hand1.AddCard(new("2"), false);

            //1 split
            (var hand11, var hand21) = hand1.SplitHand(new("2"), new("2"));

            //2 splits
            (var hand111, var hand211) = hand11.SplitHand(new("2"), new("2"));

            GameState gs = new()
            {
                PlayerHand = hand111,
                Rules = rules
            };

            Assert.IsFalse(gs.CanSplit());
        }

        [TestMethod]
        public void CanDD_IsCorrectForAllowDAS()
        {
            var rules = new BlackjackRules(1, 1)
            {
                DAS = true,
            };

            GameState gs = new()
            {
                PlayerHand = new TestHand(4) { Cards = [new("5"), new("5")], SplitsThisHand = 1 },
                Rules = rules
            };

            Assert.IsTrue(gs.CanDoubleDown());
        }

        [TestMethod]
        public void CanDD_IsCorrectForNoDAS()
        {
            var rules = new BlackjackRules(1, 1)
            {
                DAS = false,
            };

            GameState gs = new()
            {
                PlayerHand = new TestHand(4) { Cards = [new("5"), new("5")], SplitsThisHand = 1 },
                Rules = rules
            };

            Assert.IsFalse(gs.CanDoubleDown());
        }
    }
}
