namespace BJ
{
    public class Player
    {
        public List<Hand> Hands { get; set; } = [];
        public decimal BankRoll { get; set; }
        public int BettingUnit { get; set; } = 1;   //set BU to 1 dollar by default...
        public BettingStrategy BettingStrategy { get; set; }
        public PlayingStrategy PlayingStrategy { get; set; }

        public Player(decimal bankroll, BettingStrategy bettingstrategy, PlayingStrategy playingstrategy)
        {
            BankRoll = bankroll;
            BettingStrategy = bettingstrategy;
            PlayingStrategy = playingstrategy;
        }

        public void AddHands(int handscount)
        {
            for (int i = 0; i < handscount; i++)
            {
                Hands.Add(new Hand());
            }
        }

        public void ClearHands()
        {
            foreach (Hand hand in Hands)
                hand.Clear();
            Hands = [];
        }

        public (int Bet, int Hands) GetBetAmount(int truecount)
        {
            return BettingStrategy.GetBetAmountAndHands(truecount, BettingUnit);
        }
    }
}
