namespace BJ
{
    public class BJPlayer
    {
        public Player Player { get; set; }
        public Blackjack Blackjack { get; set; }

        public void Play(decimal bet)
        {
            var upcard = Blackjack.DealersUpcard;
            var dealershand = Blackjack.DealersHand;

            List<HandAndResult> undecidedhands = [];
            List<HandAndResult> decidedhands = [];
#if DEBUG
            Console.WriteLine($"Dealer's upcard is {upcard.Rank}.");
#endif

            ProcessHands(bet, Player.Hands, upcard, undecidedhands, decidedhands);
            ProcessResolvedHands(decidedhands);

#if DEBUG
            Console.WriteLine($"Dealer's downcard is {Blackjack.DealersDowncard.Rank}");
#endif
            FinishDealerDrawing();
            ResolveRemainingHands(undecidedhands);
        }

        private void ProcessHands(decimal bet, IEnumerable<Hand> hands, Card upcard, List<HandAndResult> undecidedhands, List<HandAndResult> decidedhands)
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
                    IsSurrenderAllowed = Blackjack.IsSurrenderAllowed
                };
                //if you both get blackjack
                if (Blackjack.DealersHand.IsBlackjack && hand.IsBlackjack)
                {
                    decidedhands.Add(new HandAndResult() { Hand = hand, Payout = 0, Message = "Both you and dealer got blackjack. Pushing." });
                    continue;
                }
                else if (hand.IsBlackjack)
                {
                    decidedhands.Add(new HandAndResult { Hand = hand, Payout = 1.5m * bet, Message = "You won with blackjack!" });
                    continue;
                }
                else if (Blackjack.DealersHand.IsBlackjack)
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
                        var doubledown = Blackjack.Hit();
                        hand.AddCard(doubledown);
                        undecidedhands.Add(new HandAndResult { Hand = hand, Payout = 2 * bet, Message = $"Doubled down for a total of {hand.Value} against dealer's {upcard.Rank}." });
                        continue;
                    case ActionEnum.P:
                        var hand1 = new Hand();
                        var hand2 = new Hand();

                        //create new cards to clear hard/soft ace pairs
                        Card card1 = new(hand.Cards[0].Rank, hand.Cards[0].Suit);
                        Card card2 = new(hand.Cards[1].Rank, hand.Cards[1].Suit);

                        hand1.AddCard(card1, false);
                        hand1.AddCard(Blackjack.Hit(), false);
                        hand2.AddCard(card2, false);
                        hand2.AddCard(Blackjack.Hit(), false);

                        hand.Clear();

                        ProcessHands(bet, [hand1, hand2], upcard, undecidedhands, decidedhands);
                        continue;
                    case ActionEnum.H:
                        while (Player.PlayingStrategy.GetAction(gs) == ActionEnum.H)
                        {
                            int previousvalue = hand.Value;
                            var hit = Blackjack.Hit();
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
            while (Blackjack.DealersHand.Value < 17 || (Blackjack.DealersHand.Value == 17 && Blackjack.DealersHand.IsSoft))
            {
                var hitcard = Blackjack.Hit();
                Blackjack.DealersHand.AddCard(hitcard);
#if DEBUG
                Console.WriteLine($"The dealer drew a {hitcard.Rank} for a total of {Blackjack.DealersHand.Value}.");
#endif
            }
        }
        private void ResolveRemainingHands(List<HandAndResult> undecidedhands)
        {
            foreach (var result in undecidedhands)
            {
                if (result.Hand.Value == Blackjack.DealersHand.Value)
                {
#if DEBUG
                    Console.WriteLine($"Hand pushed with {result.Hand.Value}.");
#endif
                }

                else if (result.Hand.Value > Blackjack.DealersHand.Value || Blackjack.DealersHand.IsBusted)
                {
#if DEBUG
                    Console.WriteLine($"Your {result.Hand.Value} beat the dealer's {Blackjack.DealersHand.Value}. You win {result.Payout}!");
#endif
                    Player.BankRoll += result.Payout;
                }
                else
                {
#if DEBUG
                    Console.WriteLine($"The dealers {Blackjack.DealersHand.Value} beat your {result.Hand.Value}. You lose {result.Payout}.");
#endif
                    Player.BankRoll -= result.Payout;
                }
            }
        }
    }
}
