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
        public IHand ParentHand { get; }
        public bool IsNewHand { get; }

        public int AddCard(Card card, bool IsHit);
        public int IncrementSplitsThisHand();
        public int GetSplitsThisHand();
        public bool CanSplitThisHand(int maxsplits);
        public (IHand, IHand) SplitHand(Card card1, Card card2);
    }

    public class Hand : IHand
    {
        public List<Card> Cards { get; private set; }
        public int Value { get; private set; }
        public bool IsNewHand => Hits == 0 && GetSplitsThisHand() == 0;
        public bool IsSoft { get; private set; }
        public bool IsBusted { get; private set; }
        public int Hits { get; private set; }
        public bool IsBlackjack => IsNewHand && Value == 21;
        private int _splitcount { get; set; }
        public IHand ParentHand { get; set; } //keep track of "parent hand" to know when a hand has been split too many times

        public Hand()
        {
            Clear();
        }

        public void Clear()
        {
            Cards = [];
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

        /// <summary>
        /// PUBLIC FOR TESTING
        /// </summary>
        /// <returns></returns>
        public int IncrementSplitsThisHand()
        {
            if (ParentHand != null)
                return ParentHand.IncrementSplitsThisHand();
            else
                return ++_splitcount;
        }
        public int GetSplitsThisHand()
        {
            if (ParentHand != null)
                return ParentHand.GetSplitsThisHand();
            else
                return _splitcount;
        }

        /// <summary>
        /// Returns if the hand is splittable (contains a pair) and if the # of times split is less than or equal to max splits
        /// </summary>
        /// <param name="maxsplits"></param>
        /// <returns></returns>
        public bool CanSplitThisHand(int maxsplits)
        {
            return GetSplitsThisHand() <= maxsplits && Hits == 0 && Cards.Count == 2 && (Cards[0].Value == Cards[1].Value || Cards[0].Rank == Cards[1].Rank);
        }

        public (IHand, IHand) SplitHand(Card card1, Card card2)
        {
            IncrementSplitsThisHand();
            var hand1 = new Hand()
            {
                ParentHand = this,
            };
            var hand2 = new Hand()
            {
                ParentHand = this,
            };

            hand1.AddCard(new Card(Cards[0].Rank, Cards[0].Suit), false);
            hand1.AddCard(card1, false);
            hand2.AddCard(new Card(Cards[1].Rank, Cards[1].Suit), false);
            hand2.AddCard(card2, false);

            Clear();

            return (hand1, hand2);
        }
    }

    public class TestHand : IHand

    {
        public List<Card> Cards { get; set; }

        public int Value { get; set; }
        public bool IsNewHand { get; set; }

        public bool IsSoft { get; set; }

        public bool IsBusted { get; set; }

        public int Hits { get; set; }

        public bool IsBlackjack { get; set; }

        public bool CanSplit { get; set; }

        public IHand ParentHand { get; set; }
        public int SplitsThisHand { get; set; }

        public TestHand(int value)
        {
            Value = value;
        }

        public int AddCard(Card card, bool IsHit)
        {
            throw new NotImplementedException();
        }
        public int IncrementSplitsThisHand()
        {
            throw new NotImplementedException();
        }
        public int GetSplitsThisHand() => SplitsThisHand;
        public bool CanSplitThisHand(int maxsplits)
        {
            return CanSplit;
        }
        public int IncrementSplits()
        {
            throw new NotImplementedException();
        }

        public (IHand, IHand) SplitHand(Card card1, Card card2)
        {
            throw new NotImplementedException();
        }
    }

    public class HandAndResult
    {
        public decimal Payout { get; set; }
        public IHand Hand { get; set; }
        public string Message { get; set; }
    }
}
