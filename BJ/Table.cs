namespace BJ
{
	public class BlackjackRules(int shoesize, decimal deckpen)
	{
		public int ShoeSize { get; private set; } = shoesize;
		public decimal DeckPenetration { get; set; } = deckpen;
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
			if (Shoe.Count == 0)
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

		public int GetTrueCount() => RunningCount * 52 / Shoe.Count;
	}
}
