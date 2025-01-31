namespace BJ
{
    public class BlackjackRules
    {
        public int ShoeSize { get; private set; }
        public decimal DeckPenetration { get; set; }
        public decimal BlackjackPayout { get; set; } = 1.5m;
        public int PairSplitLimit { get; set; } = 3;
        public bool DAS { get; set; } = true;
        public bool IsSurrenderAllowed { get; set; } = false;
        public bool DealerHitsSoft17 { get; set; } = true;
        public bool DealerPeeksForBlackjack { get; set; } = true;
        public int SplitAcesLimit { get; set; } = 1;
        public bool CanHitAcesAfterSplit { get; set; } = false;

        public BlackjackRules(int shoesize, decimal deckpen)
        {
            ShoeSize = shoesize;
            DeckPenetration = deckpen;
        }
    }

    public class Table
    {
        public BlackjackRules Rules { get; private set; }

        public Card DealerUpcard { get; private set; }
        public Card DealerDowncard { get; private set; }

        public Hand DealerHand { get; private set; }
        public List<Card> Shoe { get; private set; }

        public int RunningCount { get; private set; }

        public Table(BlackjackRules rules)
        {
            Rules = rules;

            Shoe = [];
            DealerHand = new Hand();

            RefillShoe();
        }

        public void RefillShoe()
        {
            Shoe.Clear();
            RunningCount = 0;
            for (int i = 0; i < Rules.ShoeSize; i++)
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

        public void ClearHands(Player player)
        {
            DealerHand.Clear();
            player.ClearHands();
        }

        //dealer generally will deal Right to left, then themselves (face down), Right to left, then themselves face up
        public void Deal(List<Hand> PlayerHands)
        {
            foreach (var hand in PlayerHands)
            {
                hand.AddCard(Hit(), false);
            }
            DealerDowncard = Hit();
            DealerHand.AddCard(DealerDowncard, false);

            foreach (var hand in PlayerHands)
            {
                hand.AddCard(Hit(), false);
            }
            DealerUpcard = Hit();
            DealerHand.AddCard(DealerUpcard, false);
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
            return Shoe.Count <= (1 - Rules.DeckPenetration) * 52 * Rules.ShoeSize;
        }
        public decimal GetTrueCount() => RunningCount / (Shoe.Count / 52m);
    }
}
