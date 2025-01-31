namespace BJ
{
    public interface IHand
    {
        public List<Card> Cards { get; }
        public int Value { get; }
        public bool IsSoft { get; }
        public bool IsBusted { get; }
        public int Hits { get; }
        public bool IsBlackjack { get; }
        public bool CanSplit { get; }
        public int SplitCount { get; }
        public int AddCard(Card card, bool IsHit);
    }

    public class Hand : IHand
    {
        public List<Card> Cards { get; private set; }
        public int Value { get; private set; }
        public bool IsSoft { get; private set; }
        public bool IsBusted { get; private set; }
        public int Hits { get; private set; }
        public bool IsBlackjack => Hits == 0 && Value == 21;
        public bool CanSplit => Hits == 0 && Cards.Count == 2 && (Cards[0].Value == Cards[1].Value || Cards[0].Rank == Cards[1].Rank);
        public int SplitCount { get; set; }

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
    public class TestHand : IHand

    {
        public List<Card> Cards { get; set; }

        public int Value { get; set; }

        public bool IsSoft { get; set; }

        public bool IsBusted { get; set; }

        public int Hits { get; set; }

        public bool IsBlackjack { get; set; }

        public bool CanSplit { get; set; }
        public int SplitCount { get; set; }

        public TestHand(int value)
        {
            Value = value;
        }

        public int AddCard(Card card, bool IsHit)
        {
            throw new NotImplementedException();
        }
    }

    public class HandAndResult
    {
        public decimal Payout { get; set; }
        public Hand Hand { get; set; }
        public string Message { get; set; }

        public void SetHandAndPayout(Hand hand, decimal payout)
        {
            Hand = hand;
            Payout = payout;
        }
    }
}
