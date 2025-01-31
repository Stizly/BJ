namespace BJ
{
    public static class BettingStrategies
    {
        /// <summary>
        /// Build a bet spread from true count -2 to +7. if the TC is 3 and betting unit $10, it would bet p3*10
        /// </summary>
        /// <param name="d2"></param>
        /// <param name="d1"></param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <param name="p5"></param>
        /// <param name="p6"></param>
        /// <param name="p7"></param>
        /// <returns></returns>
        public static Func<int, int, int> BuildBetSpread(int d2, int d1, int p0, int p1, int p2, int p3, int p4, int p5, int p6, int p7)
        {
            return new Func<int, int, int>((tc, bu) =>
            {
                if (tc <= -2)
                {
                    return d2 * bu;
                }
                else if (tc == -1)
                {
                    return d1 * bu;
                }

                return tc switch
                {
                    0 => p0 * bu,
                    1 => p1 * bu,
                    2 => p2 * bu,
                    3 => p3 * bu,
                    4 => p4 * bu,
                    5 => p5 * bu,
                    6 => p6 * bu,
                    _ => p7 * bu,
                };
            });
        }
        public static Func<int, int, int> Flat = new((tc, bu) => bu);
        public static Func<int, int, int> Linear = new((tc, bu) =>
        {
            if (tc <= 0)
            {
                return bu;
            }
            return bu * Math.Min(tc, 12);
        });

        public static Func<int, int, int> Exponential = new((tc, bu) =>
        {
            if (tc <= 0)
            {
                return bu;
            }
            return bu * (int)Math.Pow(Math.Min(tc, 5), 2);
        });

        public static Func<int, int, int> WongLinear = new((tc, bu) =>
        {
            if (tc < 0)
                return 0;
            else if (tc == 1)
                return bu;
            else
                return bu * Math.Min(tc, 12);
        });

        public static Func<int, int, int> GrowingLinear = new((tc, bu) =>
        {
            if (tc <= 1)
                return bu;
            return tc switch
            {
                2 or 3 or 4 => tc * bu,
                _ => 10 * bu,
            };
        });
    }

    public interface IBettingStrategy
    {
        public Func<int, int, (int, int)> BettingStrategy { get; }
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
