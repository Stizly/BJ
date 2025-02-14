namespace BJ
{
	public class BlackjackRules(int shoesize, int deckpen)
	{
		public int ShoeSize { get; private set; } = shoesize;
		public int DeckPenetrationPercent { get; set; } = deckpen;
		public decimal BlackjackPayout { get; set; } = 1.5m;
		public int PairSplitLimit { get; set; } = 3;
		public bool DAS { get; set; } = true;
		public bool IsSurrenderAllowed { get; set; } = false;
		public bool DealerHitsSoft17 { get; set; } = true;
		public bool CanHitAcesAfterSplit { get; set; } = false;
		public bool CanResplitAces { get; set; } = false;
	}

	public class Table
	{
		public BlackjackRules Rules { get; private set; }
		public Dealer Dealer { get; private set; }
		public List<Card> Shoe { get; private set; }

		public int RunningCount { get; private set; }

		public Table(BlackjackRules rules, Dealer dealer)
		{
			Rules = rules;
			Dealer = dealer;
			Shoe = [];
			RefillShoe();
		}

		public void RefillShoe()
		{
			Shoe.Clear();
			RunningCount = 0;
			for (int i = 0; i < Rules.ShoeSize; i++)
			{
				foreach (var rank in Card.RANKS)
				{
					Shoe.Add(new Card(rank, SuitEnum.Diamonds));
					Shoe.Add(new Card(rank, SuitEnum.Hearts));
					Shoe.Add(new Card(rank, SuitEnum.Spades));
					Shoe.Add(new Card(rank, SuitEnum.Clubs));
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
				Shoe = [.. shoearray];
			}
			else
			{
				Shoe = [.. Shoe.OrderBy(cardshuffle)];
			}
		}

		public void ClearHands(Player player)
		{
			Dealer.Hand.Clear();
			player.ClearHands();
		}

		//dealer generally will deal Right to left, then themselves (face down), Right to left, then themselves face up
		public void Deal(List<Hand> PlayerHands)
		{
			foreach (var hand in PlayerHands)
			{
				hand.AddCard(Hit(), false);
			}
			Dealer.Downcard = Hit();
			Dealer.Hand.AddCard(Dealer.Downcard, false);

			foreach (var hand in PlayerHands)
			{
				hand.AddCard(Hit(), false);
			}
			Dealer.Upcard = Hit();
			Dealer.Hand.AddCard(Dealer.Upcard, false);
		}

		public Card Hit()
		{
			if (Shoe.Count == 0)
			{
				//this really shouldn't happen... but this prevents crashing and should only have a very minor effect on EV
				RefillShoe();
			}


			var topcard = Shoe.First();
			Shoe.Remove(topcard);
			RunningCount += topcard.CountValue;
			return topcard;
		}

		public bool DoReshuffleShoe() => Shoe.Count <= 52 * Rules.ShoeSize * (100 - Rules.DeckPenetrationPercent) / 100;
		public int GetTrueCount() => RunningCount * 52 / Shoe.Count;
	}
}
