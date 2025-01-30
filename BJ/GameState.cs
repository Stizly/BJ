namespace BJ
{
    public class GameState
    {
        public IHand PlayerHand { get; set; }
        public Card DealerUpcard { get; set; }
        public int TrueCount { get; set; }
        public bool IsSurrenderAllowed { get; set; }

        /// <summary>
        /// New hands can Split, Double, or Surrender.
        /// </summary>
        /// <returns></returns>
        private bool GetHandIsNew()
        {
            return PlayerHand.Hits == 0;
        }

        /// <summary>
        /// Blackjack automatically ends the hand.
        /// </summary>
        /// <returns></returns>
        public bool GetIsPlayerBlackjack() => PlayerHand.IsBlackjack && PlayerHand.SplitCount == 0;

        public bool CanDoubleDown() => GetHandIsNew();
        public bool CanSplit() => GetHandIsNew() && PlayerHand.CanSplit;
        public bool CanSurrender() => IsSurrenderAllowed && GetHandIsNew() && PlayerHand.SplitCount == 0;
    }
}
