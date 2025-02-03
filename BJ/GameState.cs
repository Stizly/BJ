namespace BJ
{
    public class GameState
    {
        public IHand PlayerHand { get; set; }
        public Card DealerUpcard { get; set; }
        public BlackjackRules Rules { get; set; }
        public int TrueCount { get; set; }

        public bool CanDoubleDown() => PlayerHand.Hits == 0 && (Rules.DAS || PlayerHand.GetSplitsThisHand() == 0);
        public bool CanSplit() => PlayerHand.CanSplitThisHand(Rules.PairSplitLimit) && (Rules.CanResplitAces || PlayerHand.Cards[0].Rank != "A");
        public bool CanSurrender() => Rules.IsSurrenderAllowed && PlayerHand.IsNewHand;
    }
}
