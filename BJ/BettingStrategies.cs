namespace BJ
{
    public static class BettingStrategies
    {
        /// <summary>
        /// Negatives go from -N to -1, so [0,0,1] would be 0 at -3, 0 at -2, 1 at -1.
        /// </summary>
        /// <param name="negatives"></param>
        /// <param name="positivesandzero"></param>
        /// <returns></returns>
        public static Func<int, int, int> BuildBetSpread(int[] negatives, int[] positivesandzero)
        {
            return new Func<int, int, int>((tc, bu) =>
            {
                if (tc < 0)
                {
                    //TC -4 with negatives [0, 0, 1], use 0
                    if (Math.Abs(tc) > negatives.Length)
                        return bu * negatives[0];
                    else
                        return bu * negatives[negatives.Length + tc];   //if negatives.Length == 3, tc -3 would return negatives[0], tc -1 would return negatives[2], etc.
                }
                else
                {
                    //TC +4 with positives [1, 1, 2, 4] would return 4 * bu.
                    if (tc + 1 > positivesandzero.Length)
                        return bu * positivesandzero.Last();
                    else
                        return bu * positivesandzero[tc];  //tc +1 would return positives[1], tc+2 would return positives[2], etc.
                }
            });
        }
    }

    public class BettingStrategy
    {
        private Func<int, int, (int, int)> _bettingStrategy { get; set; }
        public BettingStrategy(Func<int, int, int> bettingstrategy, int hands = 1)
        {
            _bettingStrategy = new Func<int, int, (int, int)>((tc, bu) => (bettingstrategy(tc, bu), hands));
        }
        /// <summary>
        /// Assume hands played starts at TC 0, because you likely wouldn't start betting multiple hands at a negative count.
        /// </summary>
        /// <param name="bettingstrategy"></param>
        /// <param name="hands"></param>
        public BettingStrategy(Func<int, int, int> bettingstrategy, int[] hands)
        {
            _bettingStrategy = new Func<int, int, (int, int)>((tc, bu) => (bettingstrategy(tc, bu), tc < 0 ? 1 : tc >= hands.Length ? hands.Last() : hands[tc]));
        }

        public (int Bet, int Hands) GetBetAmountAndHands(int tc, int bu) => _bettingStrategy(tc, bu);
    }
}
