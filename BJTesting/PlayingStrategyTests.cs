using BJ;

namespace BJTesting
{
    [TestClass]
    public class PlayingStrategyTests
    {
        PlayingStrategy BASICSTRATEGY = new(PlayingStrategies.BasicStrategy_HardHand_2D_H17, PlayingStrategies.BasicStrategy_SoftHand_2D_H17, PlayingStrategies.BasicStrategy_Pairs_2D_H17_DAS);

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

        private bool TestRowResult(PlayingStrategy strategy, int handvalue, IEnumerable<string>[] dealerupcards, ActionEnum[] expectedaction, bool issoft = false, bool cansplit = false, bool issurrenderallowed = true, bool isfresh = true)
        {
            GameState gamestate = new()
            {
                PlayerHand = new TestHand(handvalue)
                {
                    IsSoft = issoft,
                    CanSplit = cansplit,
                },
                IsSurrenderAllowed = issurrenderallowed
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
        }
    }
}
