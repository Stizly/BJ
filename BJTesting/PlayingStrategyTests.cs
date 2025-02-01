using BJ;

namespace BJTesting
{
    [TestClass]
    public class PlayingStrategyTests
    {
        PlayingStrategy BASICSTRATEGY = new(PlayingStrategies.BasicStrategy_HardHand_2D_H17, PlayingStrategies.BasicStrategy_SoftHand_2D_H17, PlayingStrategies.BasicStrategy_Pairs_2D_H17_DAS);
        PlayingStrategy BASICSTRATEGY_WITHDEVIATIONS = new(PlayingStrategies.BasicStrategy_HardHand_2D_H17, PlayingStrategies.BasicStrategy_SoftHand_2D_H17, PlayingStrategies.BasicStrategy_Pairs_2D_H17_DAS)
        {
            HardHandDeviations = PlayingStrategies.Deviations_HardHand_SD
        };

        [TestMethod]
        public void BasicStrategy_IsCorrectForFreshHardHands()
        {
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 4, [Card.RANKS], [ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 5, [Card.RANKS], [ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 6, [Card.RANKS], [ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 7, [Card.RANKS], [ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 8, [Card.RANKS], [ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 9, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "A"]], [ActionEnum.D, ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 10, [["2", "3", "4", "5", "6", "7", "8", "9"], ["10", "A"]], [ActionEnum.D, ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 11, [Card.RANKS], [ActionEnum.D]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 12, [["2", "3", "7", "8", "9", "10", "A"], ["4", "5", "6"]], [ActionEnum.H, ActionEnum.S]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 13, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "A"]], [ActionEnum.S, ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 14, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "A"]], [ActionEnum.S, ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 15, [["2", "3", "4", "5", "6"], ["7", "8", "9"], ["10", "A"]], [ActionEnum.S, ActionEnum.H, ActionEnum.R]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 16, [["2", "3", "4", "5", "6"], ["7", "8", "9"], ["10", "A"]], [ActionEnum.S, ActionEnum.H, ActionEnum.R]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 17, [["2", "3", "4", "5", "6", "7", "8", "9", "10"], ["A"]], [ActionEnum.S, ActionEnum.R]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 18, [Card.RANKS], [ActionEnum.S]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 19, [Card.RANKS], [ActionEnum.S]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 20, [Card.RANKS], [ActionEnum.S]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 21, [Card.RANKS], [ActionEnum.S]));
        }

        private bool TestRowResult(PlayingStrategy strategy, int handvalue, IEnumerable<string>[] dealerupcards, ActionEnum[] expectedaction, bool issoft = false, bool cansplit = false, bool issurrenderallowed = true, bool isfresh = true, int tc = 0)
        {
            var hand = new TestHand(handvalue)
            {
                IsSoft = issoft,
                Hits = isfresh ? 0 : 1,
                CanSplit = cansplit,
            };

            return TestRowResult(strategy, hand, dealerupcards, expectedaction, issurrenderallowed, tc);
        }

        private bool TestRowResult(PlayingStrategy strategy, IHand hand, IEnumerable<string>[] dealerupcards, ActionEnum[] expectedaction, bool issurrenderallowed = true, int tc = 0)
        {
            GameState gamestate = new()
            {
                PlayerHand = hand,
                IsSurrenderAllowed = issurrenderallowed,
                TrueCount = tc
            };
            for (int i = 0; i < dealerupcards.Length; i++)
            {
                foreach (var rank in dealerupcards[i])
                {
                    gamestate.DealerUpcard = new Card(rank);
                    if (expectedaction[i] != strategy.GetAction(gamestate))
                        return false;
                }
            }
            return true;
        }

        [TestMethod]
        public void BasicStrategy_IsCorrectForFreshSoftHands()
        {
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 13, [["2", "3", "4", "7", "8", "9", "10", "A"], ["5", "6"]], [ActionEnum.H, ActionEnum.D], issoft: true));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 14, [["2", "3", "4", "7", "8", "9", "10", "A"], ["5", "6"]], [ActionEnum.H, ActionEnum.D], issoft: true));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 15, [["2", "3", "7", "8", "9", "10", "A"], ["4", "5", "6"]], [ActionEnum.H, ActionEnum.D], issoft: true));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 16, [["2", "3", "7", "8", "9", "10", "A"], ["4", "5", "6"]], [ActionEnum.H, ActionEnum.D], issoft: true));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 17, [["2", "7", "8", "9", "10", "A"], ["3", "4", "5", "6"]], [ActionEnum.H, ActionEnum.D], issoft: true));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 18, [["2", "3", "4", "5", "6"], ["7", "8"], ["9", "10", "A"]], [ActionEnum.D, ActionEnum.S, ActionEnum.H], issoft: true));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 19, [["2", "3", "4", "5", "7", "8", "9", "10", "A"], ["6"]], [ActionEnum.S, ActionEnum.D], issoft: true));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 20, [Card.RANKS], [ActionEnum.S], issoft: true));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 21, [Card.RANKS], [ActionEnum.S], issoft: true));
        }

        [TestMethod]
        public void BasicStrategy_IsCorrectForHitSoftHands()
        {
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 13, [Card.RANKS], [ActionEnum.H], issoft: true, isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 14, [Card.RANKS], [ActionEnum.H], issoft: true, isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 15, [Card.RANKS], [ActionEnum.H], issoft: true, isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 16, [Card.RANKS], [ActionEnum.H], issoft: true, isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 17, [Card.RANKS], [ActionEnum.H], issoft: true, isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 18, [["2", "3", "4", "5", "6"], ["7", "8"], ["9", "10", "A"]], [ActionEnum.H, ActionEnum.S, ActionEnum.H], issoft: true, isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 19, [Card.RANKS], [ActionEnum.S], issoft: true, isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 20, [Card.RANKS], [ActionEnum.S], issoft: true));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 21, [Card.RANKS], [ActionEnum.S], issoft: true));
        }

        [TestMethod]
        public void BasicStrategy_IsCorrectForHitHardHands()
        {
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 4, [Card.RANKS], [ActionEnum.H], isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 5, [Card.RANKS], [ActionEnum.H], isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 6, [Card.RANKS], [ActionEnum.H], isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 7, [Card.RANKS], [ActionEnum.H], isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 8, [Card.RANKS], [ActionEnum.H], isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 9, [Card.RANKS], [ActionEnum.H], isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 10, [Card.RANKS], [ActionEnum.H], isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 11, [Card.RANKS], [ActionEnum.H], isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 12, [["2", "3", "7", "8", "9", "10", "A"], ["4", "5", "6"]], [ActionEnum.H, ActionEnum.S], isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 13, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "A"]], [ActionEnum.S, ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 14, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "A"]], [ActionEnum.S, ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 15, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "A"], ["10", "A"]], [ActionEnum.S, ActionEnum.H, ActionEnum.H], isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 16, [["2", "3", "4", "5", "6"], ["7", "8", "9"], ["10", "A"]], [ActionEnum.S, ActionEnum.H, ActionEnum.H], isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 17, [["2", "3", "4", "5", "6", "7", "8", "9", "10"], ["A"]], [ActionEnum.S, ActionEnum.S], isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 18, [Card.RANKS], [ActionEnum.S], isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 19, [Card.RANKS], [ActionEnum.S], isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 20, [Card.RANKS], [ActionEnum.S], isfresh: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, 21, [Card.RANKS], [ActionEnum.S], isfresh: false));
        }

        [TestMethod]
        public void BasicStrategy_IsCorrectForSplits()
        {
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, new TestHand(4) { Cards = [new("2"), new("2")], CanSplit = true }, [["2", "3", "4", "5", "6", "7"], ["8", "9", "10", "A"]], [ActionEnum.P, ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, new TestHand(6) { Cards = [new("3"), new("3")], CanSplit = true }, [["2", "3", "4", "5", "6", "7"], ["8", "9", "10", "A"]], [ActionEnum.P, ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, new TestHand(8) { Cards = [new("4"), new("4")], CanSplit = true }, [["2", "3", "4"], ["5", "6"], ["7", "8", "9", "10", "A"]], [ActionEnum.H, ActionEnum.P, ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, new TestHand(10) { Cards = [new("5"), new("5")], CanSplit = true }, [["2", "3", "4", "5", "6", "7", "8", "9"], ["10", "A"]], [ActionEnum.D, ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, new TestHand(12) { Cards = [new("6"), new("6")], CanSplit = true }, [["2", "3", "4", "5", "6", "7"], ["8", "9", "10", "A"]], [ActionEnum.P, ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, new TestHand(14) { Cards = [new("7"), new("7")], CanSplit = true }, [["2", "3", "4", "5", "6", "7", "8"], ["9", "10", "A"]], [ActionEnum.P, ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, new TestHand(16) { Cards = [new("8"), new("8")], CanSplit = true }, [["2", "3", "4", "5", "6", "7", "8", "9", "10"], ["A"]], [ActionEnum.P, ActionEnum.R]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, new TestHand(18) { Cards = [new("9"), new("9")], CanSplit = true }, [["2", "3", "4", "5", "6", "8", "9"], ["7", "10", "A"]], [ActionEnum.P, ActionEnum.S]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, new TestHand(20) { Cards = [new("10"), new("10")], CanSplit = true }, [Card.RANKS], [ActionEnum.S]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY, new TestHand(12) { Cards = [new("A"), new("A")], CanSplit = true }, [Card.RANKS], [ActionEnum.P]));
        }

        [TestMethod]
        public void DeviatedStrategy_IsCorrect()
        {
            Assert.IsTrue(TestRowResult(BASICSTRATEGY_WITHDEVIATIONS, 4, [Card.RANKS], [ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY_WITHDEVIATIONS, 5, [Card.RANKS], [ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY_WITHDEVIATIONS, 6, [Card.RANKS], [ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY_WITHDEVIATIONS, 7, [Card.RANKS], [ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY_WITHDEVIATIONS, 8, [Card.RANKS], [ActionEnum.H]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY_WITHDEVIATIONS, 9, [["2", "3", "4", "5", "6", "7"], ["8", "9", "10", "A"]], [ActionEnum.D, ActionEnum.H], tc: 3));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY_WITHDEVIATIONS, 10, [["2", "3", "4", "5", "6", "7", "8", "9", "10", "A"]], [ActionEnum.D, ActionEnum.H], tc: 4));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY_WITHDEVIATIONS, 11, [Card.RANKS], [ActionEnum.D], tc: 1));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY_WITHDEVIATIONS, 12, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "A"]], [ActionEnum.S, ActionEnum.H], tc: 3));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY_WITHDEVIATIONS, 12, [Card.RANKS], [ActionEnum.H], tc: -2));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY_WITHDEVIATIONS, 13, [["4", "5", "6"], ["2", "3", "7", "8", "9", "10", "A"]], [ActionEnum.S, ActionEnum.H], tc: -2));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY_WITHDEVIATIONS, 14, [["2", "3", "4", "5", "6"], ["7", "8", "9", "10", "A"]], [ActionEnum.S, ActionEnum.H], tc: 1));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY_WITHDEVIATIONS, 15, [["2", "3", "4", "5", "6", "10"], ["7", "8", "9", "A"]], [ActionEnum.S, ActionEnum.H], tc: 4, issurrenderallowed: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY_WITHDEVIATIONS, 16, [["2", "3", "4", "5", "6", "9", "10"], ["7", "8", "A"]], [ActionEnum.S, ActionEnum.H], tc: 5, issurrenderallowed: false));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY_WITHDEVIATIONS, 17, [["2", "3", "4", "5", "6", "7", "8", "9", "10"], ["A"]], [ActionEnum.S, ActionEnum.R]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY_WITHDEVIATIONS, 18, [Card.RANKS], [ActionEnum.S]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY_WITHDEVIATIONS, 19, [Card.RANKS], [ActionEnum.S]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY_WITHDEVIATIONS, 20, [Card.RANKS], [ActionEnum.S]));
            Assert.IsTrue(TestRowResult(BASICSTRATEGY_WITHDEVIATIONS, 21, [Card.RANKS], [ActionEnum.S]));
        }
    }
}
