namespace BJ
{
    public class Hand
    {
        public List<Card> Cards { get; private set; }
        public int Value { get; private set; }
        public bool IsSoft { get; private set; }
        public bool IsBusted { get; private set; }
        public int Hits { get; private set; }
        public bool IsBlackjack => Hits == 0 && Value == 21;

        public Hand()
        {
            Clear();
        }

        public void Clear()
        {
            Cards = new List<Card>();
            Value = 0;
            Hits = 0;
            IsSoft = false;
            IsBusted = false;
        }

        public int AddCard(Card card, bool IsHit = true)
        {
            Cards.Add(card);
            Value += card.Value;

            if (Value > 21 && Cards.FirstOrDefault(c => c.Rank == "A" && c.Value == 11) is Card softace)
            {
                Value -= 10;
                softace.SoftAceToHardAce();
            }

            IsSoft = Cards.Any(c => c.IsSoft);

            IsBusted = Value > 21;
            if (IsHit)
            {
                Hits++;
            }

            return Value;
        }
    }

    public class HandAndResult
    {
        public decimal Payout { get; private set; }
        public Hand Hand { get; private set; }

        public void SetHandAndPayout(Hand hand, decimal payout)
        {
            Hand = hand;
            Payout = payout;
        }
    }
}
