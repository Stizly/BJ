namespace BJ
{
    public class BJPlayer(Table table, Player player)
    {
        public Player Player { get; set; } = player;
        public Table Table { get; set; } = table;

        public void Play(decimal bet)
        {
            var upcard = Table.DealerUpcard;
            var dealershand = Table.DealerHand;

            List<HandAndResult> undecidedhands = [];
            List<HandAndResult> decidedhands = [];
#if DEBUG
            Console.WriteLine($"Dealer's upcard is {upcard.Rank}.");
#endif

            ProcessHands(bet, Player.Hands, upcard, undecidedhands, decidedhands);
            ProcessResolvedHands(decidedhands);

#if DEBUG
            Console.WriteLine($"Dealer's downcard is {Table.DealerDowncard.Rank}");
#endif
            FinishDealerDrawing();
            ResolveRemainingHands(undecidedhands);
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
                    Rules = Table.Rules
                };
                //if you both get blackjack
                if (Table.DealerHand.IsBlackjack && hand.IsBlackjack)
                {
                    decidedhands.Add(new HandAndResult() { Hand = hand, Payout = 0, Message = "Both you and dealer got blackjack. Pushing." });
                    continue;
                }
                else if (hand.IsBlackjack)
                {
                    decidedhands.Add(new HandAndResult { Hand = hand, Payout = 1.5m * bet, Message = "You won with blackjack!" });
                    continue;
                }
                else if (Table.DealerHand.IsBlackjack)
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
                        while (Player.PlayingStrategy.GetAction(gs) == ActionEnum.H)
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
            while (Table.DealerHand.Value < 17 || (Table.Rules.DealerHitsSoft17 && Table.DealerHand.Value == 17 && Table.DealerHand.IsSoft))
            {
                var hitcard = Table.Hit();
                Table.DealerHand.AddCard(hitcard);
#if DEBUG
                Console.WriteLine($"The dealer drew a {hitcard.Rank} for a total of {Table.DealerHand.Value}.");
#endif
            }
        }
        private void ResolveRemainingHands(List<HandAndResult> undecidedhands)
        {
            foreach (var result in undecidedhands)
            {
                if (result.Hand.Value == Table.DealerHand.Value)
                {
#if DEBUG
                    Console.WriteLine($"Hand pushed with {result.Hand.Value}.");
#endif
                }

                else if (result.Hand.Value > Table.DealerHand.Value || Table.DealerHand.IsBusted)
                {
#if DEBUG
                    Console.WriteLine($"Your {result.Hand.Value} beat the dealer's {Table.DealerHand.Value}. You win {result.Payout}!");
#endif
                    Player.BankRoll += result.Payout;
                }
                else
                {
#if DEBUG
                    Console.WriteLine($"The dealers {Table.DealerHand.Value} beat your {result.Hand.Value}. You lose {result.Payout}.");
#endif
                    Player.BankRoll -= result.Payout;
                }
            }
        }
    }
}
