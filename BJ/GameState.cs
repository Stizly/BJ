namespace BJ
{
    public class GameState
    {
        public Hand PlayerHand { get; set; }
        public Card DealerUpcard { get; set; }
        public int TrueCount { get; set; }
    }
}
