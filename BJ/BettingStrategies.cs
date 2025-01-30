namespace BJ
{
    public static class BettingStrategies
    {
        public static Func<int, int, int> BuildStrategy(int d2, int d1, int p0, int p1, int p2, int p3, int p4)
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
                else if (tc >= 4)
                {
                    return p4 * bu;
                }

                return tc switch
                {
                    0 => p0 * bu,
                    1 => p1 * bu,
                    2 => p2 * bu,
                    3 => p3 * bu,
                    _ => 0
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
}
