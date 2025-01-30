namespace BJ
{
    public class Player
    {
        public List<Hand> Hands { get; set; }
        public Hand CurrentHand { get; set; }
        public decimal BankRoll { get; set; }
        public Func<int, int, int> BettingStrategy { get; set; } = BettingStrategies.Flat;
        public PlayingStrategy PlayingStrategy { get; set; }

        public Player()
        {
            Hands = new List<Hand>() { new Hand() };
        }

        public decimal DetermineBet(decimal tc, decimal bet) => BettingStrategy((int)tc, (int)bet);
    }
}
