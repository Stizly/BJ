using BJ.PlayingStrategies;

namespace BJ
{
	public class BJPlayer(Table table, Player player)
	{
		public Player Player { get; set; } = player;
		public Table Table { get; set; } = table;

		public void Play(decimal bet)
		{
			var upcard = Table.Dealer.Upcard;
			var dealershand = Table.Dealer.Hand;

			List<HandAndResult> undecidedhands = [];
			List<HandAndResult> decidedhands = [];
#if DEBUG
			Console.WriteLine($"Dealer's upcard is {upcard.Rank}.");
#endif

			ProcessHands(bet, Player.Hands, upcard, undecidedhands, decidedhands);
			ProcessResolvedHands(decidedhands);

#if DEBUG
			Console.WriteLine($"Dealer's downcard is {Table.Dealer.Downcard.Rank}");
#endif
			if (undecidedhands.Count > 0)
			{
				FinishDealerDrawing();
				ResolveRemainingHands(undecidedhands);
			}
		}

		private void ProcessHands(decimal bet, IEnumerable<IHand> hands, Card upcard, List<HandAndResult> undecidedhands, List<HandAndResult> decidedhands)
		{
			foreach (Hand hand in hands)
			{
#if DEBUG
				Console.WriteLine($"Your cards are {string.Join(", ", hand.Cards.Select(c => c.Rank))}.");
#endif
				GameState gs = new()
				{
					DealerUpcard = upcard,
					PlayerHand = hand,
					Rules = Table.Rules,
					RunningCount = Table.RunningCount,
					TrueCount = Table.GetTrueCount()
				};

				if (Table.Dealer.Upcard.Rank == "A" && Player.PlayingStrategy.DoTakeInsurrance(gs))
				{
					//take insurance
					if (Table.Dealer.Hand.IsBlackjack)
					{
						//get your full bet value back if you win
						decidedhands.Add(new HandAndResult() { Hand = hand, Payout = bet, Message = "You took insurance and the dealer had Blackjack!" });
					}
					else
					{
						//lose half your bet if you lose
						decidedhands.Add(new HandAndResult() { Hand = hand, Payout = bet / 2, Message = "You took insurance and the dealer did not have Blackjack!" });
					}
				}

				//if you both get blackjack
				if (Table.Dealer.Hand.IsBlackjack && hand.IsBlackjack)
				{
					decidedhands.Add(new HandAndResult() { Hand = hand, Payout = 0, Message = "Both you and dealer got blackjack. Pushing." });
					continue;
				}
				else if (hand.IsBlackjack)
				{
					decidedhands.Add(new HandAndResult { Hand = hand, Payout = 1.5m * bet, Message = "You won with blackjack!" });
					continue;
				}
				else if (Table.Dealer.Hand.IsBlackjack)
				{
					decidedhands.Add(new HandAndResult { Hand = hand, Payout = -bet, Message = "Dealer had blackjack. Hand lost." });
					continue;
				}

				switch (Player.PlayingStrategy.GetAction(gs))
				{
					case ActionEnum.R:
						decidedhands.Add(new HandAndResult { Hand = hand, Payout = -0.5m * bet, Message = $"Surrendering on a {hand.Value} against dealer's {upcard.Rank}" });
						continue;

					case ActionEnum.D:
						var doubledown = Table.Hit();
						hand.AddCard(doubledown);
						undecidedhands.Add(new HandAndResult { Hand = hand, Payout = 2 * bet, Message = $"Doubled down for a total of {hand.Value} against dealer's {upcard.Rank}." });
						continue;

					case ActionEnum.P:
						bool isacesplit = hand.Cards[0].Rank == "A" && hand.Cards[1].Rank == "A";
						var split = hand.SplitHand(Table.Hit(), Table.Hit());

						if (isacesplit && !Table.Rules.CanHitAcesAfterSplit)
							undecidedhands.AddRange([
								new(){ Hand = split.Item1, Payout=bet, Message=$"Split aces for a total of {split.Item1.Value}." },
								new() { Hand = split.Item2, Payout = bet, Message = $"Split aces for a total of {split.Item2.Value}." }
							]);
						else
							ProcessHands(bet, [split.Item1, split.Item2], upcard, undecidedhands, decidedhands);
						continue;

					case ActionEnum.H:
						do
						{
							int previousvalue = hand.Value;
							var hit = Table.Hit();
							hand.AddCard(hit);
							if (hand.IsBusted)
							{
								decidedhands.Add(new HandAndResult { Hand = hand, Payout = -bet, Message = $"You hit on a {previousvalue} and busted with {hand.Value} against the dealer's {upcard.Value}" });
								break;
							}
						}
						while (Player.PlayingStrategy.GetAction(gs) == ActionEnum.H);

						if (!hand.IsBusted)
						{
							undecidedhands.Add(new() { Hand = hand, Payout = bet, Message = $"Staying on a {hand.Value} against dealer's {upcard.Value}." });
						}
						break;

					case ActionEnum.S:
						undecidedhands.Add(new() { Hand = hand, Payout = bet, Message = $"You stayed with a {hand.Value} against the dealer's {upcard.Value}." });
						break;
				}

			}
		}

		private void ProcessResolvedHands(List<HandAndResult> decidedhands)
		{
			foreach (var result in decidedhands)
			{
				Player.BankRoll += result.Payout;
#if DEBUG
				Console.WriteLine(result.Message);
#endif
			}
		}
		private void FinishDealerDrawing()
		{
			Card upcardfordealeraction = new("2");
			//if it's a H17 game and Value == soft 17, Get Action will return H, so keep hitting.
			//if it's a S17 game and Value == soft 17m Get Action will return S, so it will NOT continue hitting
			while (Table.Dealer.Hand.Value <= 17 && Table.Dealer.PlayingStrategy.GetAction(new GameState { DealerUpcard = upcardfordealeraction, PlayerHand = Table.Dealer.Hand, Rules = Table.Rules, RunningCount = 0, TrueCount = 0 }) == ActionEnum.H)
			{
				var hitcard = Table.Hit();
				Table.Dealer.Hand.AddCard(hitcard);
#if DEBUG
				Console.WriteLine($"The dealer drew a {hitcard.Rank} for a total of {Table.Dealer.Hand.Value}.");
#endif
			}
		}
		private void ResolveRemainingHands(List<HandAndResult> undecidedhands)
		{
			foreach (var result in undecidedhands)
			{
				if (result.Hand.Value == Table.Dealer.Hand.Value)
				{
#if DEBUG
					Console.WriteLine($"Hand pushed with {result.Hand.Value}.");
#endif
				}

				else if (result.Hand.Value > Table.Dealer.Hand.Value || Table.Dealer.Hand.IsBusted)
				{
#if DEBUG
					Console.WriteLine($"Your {result.Hand.Value} beat the dealer's {Table.Dealer.Hand.Value}. You win {result.Payout}!");
#endif
					Player.BankRoll += result.Payout;
				}
				else
				{
#if DEBUG
					Console.WriteLine($"The dealers {Table.Dealer.Hand.Value} beat your {result.Hand.Value}. You lose {result.Payout}.");
#endif
					Player.BankRoll -= result.Payout;
				}
			}
		}
	}
}
