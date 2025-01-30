namespace BJ
{
	public class Card
	{
		public static string[] RANKS = ["2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A"];
		public static int[] COUNTSTRATEGY = [1, 1, 1, 1, 1, 0, 0, 0, -1, -1, -1, -1, -1];

		public SuitEnum Suit { get; private set; }
		public string Rank { get; private set; }
		public int CountValue { get; private set; }
		public int Value { get; private set; }
		public bool IsSoft { get; private set; }

		public Card(int rankindex, SuitEnum suit)
		{
			Rank = RANKS[rankindex];
			Suit = suit;
			CountValue = COUNTSTRATEGY[rankindex];
			UpdateValue();
		}
		public Card(string rank, SuitEnum suit)
		{
			Rank = rank;
			Suit = suit;
			CountValue = COUNTSTRATEGY[Array.IndexOf(RANKS, rank)];
			UpdateValue();
		}

		private void UpdateValue()
		{
			if (Rank == "A")
			{
				Value = 11;
				IsSoft = true;
				return;
			}

			IsSoft = false;
			if (int.TryParse(Rank, out int numberrank))
			{
				Value = numberrank;
			}
			else
			{
				Value = 10;
			}
		}

		public void SoftAceToHardAce()
		{
			if (Rank == "A" && Value == 11)
			{
				Value = 1;
				IsSoft = false;
			}
		}
	}
	public enum SuitEnum
	{
		Diamonds, Hearts, Clubs, Spades
	}
}
