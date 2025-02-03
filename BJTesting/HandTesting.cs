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

        [TestMethod]
        public void CanSplit_IsCorrectForNewHand()
        {
            var handAA = new Hand();
            handAA.AddCard(new Card("A"), false);
            handAA.AddCard(new Card("A"), false);

            Assert.IsTrue(handAA.CanSplitThisHand(3));
        }

        [TestMethod]
        public void CanSplit_IsCorrectForNewHandAtMaxLimit()
        {
            var handAA = new Hand();
            handAA.AddCard(new Card("A"), false);
            handAA.AddCard(new Card("A"), false);
            handAA.IncrementSplitsThisHand();

            Assert.IsTrue(handAA.CanSplitThisHand(1));
        }

        [TestMethod]
        public void CanSplit_IsCorrectForNewHandBeyondMaxLimit()
        {
            var handAA = new Hand();
            handAA.AddCard(new Card("A"), false);
            handAA.AddCard(new Card("A"), false);
            handAA.IncrementSplitsThisHand();
            handAA.IncrementSplitsThisHand();

            Assert.IsFalse(handAA.CanSplitThisHand(0));
        }

        [TestMethod]
        public void CanSplit_IsCorrectForParentSplitPastLimit()
        {
            var handAA = new Hand();
            handAA.AddCard(new Card("A"), false);
            handAA.AddCard(new Card("A"), false);
            handAA.IncrementSplitsThisHand();
            handAA.IncrementSplitsThisHand();

            Assert.IsFalse(handAA.CanSplitThisHand(0));
        }

        [TestMethod]
        public void IncrementSplitsThisHand_IsCorrect()
        {
            var handAA = new Hand();
            handAA.AddCard(new Card("2"), false);
            handAA.AddCard(new Card("2"), false);

            //1 split
            (var split1, var split2) = handAA.SplitHand(new Card("2"), new Card("3"));

            //2 splits
            (var split1_1, var split1_2) = split1.SplitHand(new Card("3"), new Card("4"));

            Assert.AreEqual(2, handAA.GetSplitsThisHand());
            Assert.AreEqual(2, split2.GetSplitsThisHand());
            Assert.AreEqual(2, split1.GetSplitsThisHand());
            Assert.AreEqual(2, split1_1.GetSplitsThisHand());
            Assert.AreEqual(2, split1_2.GetSplitsThisHand());
        }

        [TestMethod]
        public void CanSplit_IsCorrectFor10ValueCards()
        {
            var hand1010 = new Hand();
            hand1010.AddCard(new Card("10"), false);
            hand1010.AddCard(new Card("10"), false);
            Assert.IsTrue(hand1010.CanSplitThisHand(1));

            string[] tenvalues = ["10", "J", "Q", "K"];
            foreach (var firstcardrank in tenvalues)
            {
                var firstcard = new Card(firstcardrank);
                foreach (var secondcardrank in tenvalues)
                {
                    var secondcard = new Card(secondcardrank);
                    var hand = new Hand();
                    hand.AddCard(firstcard, false);
                    hand.AddCard(secondcard, false);

                    Assert.IsTrue(hand.CanSplitThisHand(1));
                }
            }
        }
    }
}
