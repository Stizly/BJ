namespace BJ
{
    public class Blackjack
    {
        public int ShoeSize { get; private set; }
        public decimal DeckPenetration { get; set; }
        public bool IsSurrenderAllowed { get; set; }
        public bool DealerHitsSoft17 { get; set; }
        public bool DealerPeeksForBlackjack { get; set; }
        public decimal BlackjackPayout { get; set; }

        public Card DealersUpcard { get; private set; }
        public Card DealersDowncard { get; private set; }

        public Hand DealersHand { get; private set; }
        public List<Card> Shoe { get; private set; }

        public int RunningCount { get; private set; } = 0;

        public Blackjack(int shoesize)
        {
            Shoe = new List<Card>();
            DealersHand = new Hand();

            ShoeSize = shoesize;
            RefillShoe();
        }

        public void RefillShoe()
        {
            Shoe.Clear();
            RunningCount = 0;
            for (int i = 0; i < ShoeSize; i++)
            {
                for (int rankindex = 0; rankindex < Card.RANKS.Length; rankindex++)
                {
                    Shoe.Add(new Card(rankindex, SuitEnum.Diamonds));
                    Shoe.Add(new Card(rankindex, SuitEnum.Hearts));
                    Shoe.Add(new Card(rankindex, SuitEnum.Spades));
                    Shoe.Add(new Card(rankindex, SuitEnum.Clubs));
                }
            }
            ShuffleShoe();
        }

        public void ShuffleShoe(Func<Card, object>? cardshuffle = null)
        {
            if (cardshuffle == null)
            {
                var shoearray = Shoe.ToArray();
                Random.Shared.Shuffle(shoearray);
                Shoe = shoearray.ToList();
            }
            else
            {
                Shoe = Shoe.OrderBy(cardshuffle).ToList();
            }
        }

        public void ClearHands(IEnumerable<Hand> playerhands)
        {
            DealersHand.Clear();
            foreach (var hand in playerhands)
                hand.Clear();

        }

        //dealer generally will deal Right to left, then themselves (face down), Right to left, then themselves face up
        public void Deal(List<Hand> PlayerHands)
        {
            foreach (var hand in PlayerHands)
            {
                hand.AddCard(Hit(), false);
            }
            DealersDowncard = Hit();
            DealersHand.AddCard(DealersDowncard, false);

            foreach (var hand in PlayerHands)
            {
                hand.AddCard(Hit(), false);
            }
            DealersUpcard = Hit();
            DealersHand.AddCard(DealersUpcard, false);
        }

        public Card Hit()
        {
            if (!Shoe.Any())
                return null;

            var topcard = Shoe.First();
            Shoe.Remove(topcard);
            RunningCount += topcard.CountValue;
            return topcard;
        }

        public bool DoReshuffleShoe()
        {
            return Shoe.Count <= (1 - DeckPenetration) * 52 * ShoeSize;
        }
        public decimal GetTrueCount() => RunningCount / (Shoe.Count / 52m);
    }
}
