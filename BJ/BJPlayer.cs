namespace BJ
{
    public class BJPlayer
    {
        public Player Player { get; set; }
        public Blackjack Blackjack { get; set; }
        public Func<Card, Hand, bool> DoSurrender { get; set; }
        public Func<Card, Hand, bool> DoSplit { get; set; }
        public Func<Card, Hand, bool> DoHit { get; set; }
        public Func<Card, Hand, bool> DoDoubleDown { get; set; }


        public void Play(decimal bet)
        {
            var upcard = Blackjack.DealersUpcard;
            var dealershand = Blackjack.DealersHand;

            List<HandAndResult> undecidedhands = [];
            List<HandAndResult> decidedhands = [];
#if DEBUG
            Console.WriteLine($"Dealer's upcard is {upcard.Rank}.");
#endif
            ProcessHands(Player.Hands, upcard, bet, undecidedhands, decidedhands, Blackjack.DealersHand.IsBlackjack);

#if DEBUG
            Console.WriteLine($"Dealer's downcard is {Blackjack.DealersDowncard.Rank}");
#endif
            foreach (var result in decidedhands)
            {
                Player.BankRoll += result.Payout;
#if DEBUG
                if (result.Payout < 0)
                    Console.WriteLine($"You lost ${Math.Abs(result.Payout)}.");
                else
                    Console.WriteLine($"You won ${result.Payout}.");
#endif

            }

            while (Blackjack.DealersHand.Value < 17 || (Blackjack.DealersHand.Value == 17 && Blackjack.DealersHand.IsSoft))
            {
                var hitcard = Blackjack.Hit();
                Blackjack.DealersHand.AddCard(hitcard);
#if DEBUG
                Console.WriteLine($"The dealer drew a {hitcard.Rank} for a total of {Blackjack.DealersHand.Value}.");
#endif
            }

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
#if DEBUG
            Console.WriteLine("-----------------------");
#endif
        }

        private void ProcessHands(IEnumerable<Hand> hands, Card upcard, decimal bet, List<HandAndResult> undecidedhands, List<HandAndResult> decidedhands, bool dealerhasblackjack)
        {
            foreach (Hand hand in hands)
            {
                HandAndResult hr = new();
#if DEBUG
                Console.WriteLine($"You have: {string.Join(" ", hand.Cards.Select(c => c.Rank))}.");
#endif
                if (hand.IsBlackjack && !dealerhasblackjack)
                {
#if DEBUG
                    Console.WriteLine($"You win on a blackjack! Paying out {Blackjack.BlackjackPayout} times {bet}!");
#endif
                    hr.SetHandAndPayout(hand, bet * 1.5m);
                    decidedhands.Add(hr);

                    continue;
                }

                if (hand.Hits == 0 && Blackjack.IsSurrenderAllowed && DoSurrender(upcard, hand))
                {
#if DEBUG
                    Console.WriteLine($"Surrendering on {hand.Value} against Dealer's {upcard.Rank}");
#endif
                    //return half the bet on a surrender
                    hr.SetHandAndPayout(hand, bet * -0.5m);
                    decidedhands.Add(hr);
                    continue;
                }

                if (dealerhasblackjack && !hand.IsBlackjack)
                {
#if DEBUG
                    Console.WriteLine("Sorry, dealer had blackjack.");
#endif
                    hr.SetHandAndPayout(hand, -bet);
                    decidedhands.Add(hr);
                    continue;
                }

                if (hand.Hits == 0 && DoSplit(upcard, hand))
                {
#if DEBUG
                    Console.WriteLine($"Splitting hand with {hand.Cards[0].Rank} and {hand.Cards[1].Rank}");
#endif
                    var hand1 = new Hand();
                    var hand2 = new Hand();

                    hand1.AddCard(hand.Cards[0], false);
                    hand1.AddCard(Blackjack.Hit(), false);
                    hand2.AddCard(hand.Cards[1], false);
                    hand2.AddCard(Blackjack.Hit(), false);

                    hand.Clear();

                    ProcessHands([hand1, hand2], upcard, bet, undecidedhands, decidedhands, dealerhasblackjack);
                    continue;
                }

                if (hand.Hits == 0 && DoDoubleDown(upcard, hand))
                {
                    var hit = Blackjack.Hit();
#if DEBUG
                    Console.WriteLine($"Doubled down on a {hand.Value} and drew a {hit.Rank} for a total of {hand.AddCard(hit)}");
#endif
                    if (hand.IsBusted)
                    {
                        hr.SetHandAndPayout(hand, bet * -2);
                        decidedhands.Add(hr);
                    }
                    else
                    {
                        hr.SetHandAndPayout(hand, bet * 2);
                        undecidedhands.Add(hr);
                    }
                    continue;
                }


                while (DoHit(upcard, hand))
                {
                    var addingcard = Blackjack.Hit();
#if DEBUG
                    Console.WriteLine($"Hitting a {(hand.IsSoft ? "soft" : "hard")} {hand.Value} into the dealer's {upcard.Rank}.");
#endif
                    hand.AddCard(addingcard);
#if DEBUG
                    Console.WriteLine($"\tYou hit a {addingcard.Rank} for a total of {hand.Value}.");
#endif
                }
                if (hand.IsBusted)
                {
#if DEBUG
                    Console.WriteLine($"\tYou busted.");
#endif
                    hr.SetHandAndPayout(hand, -bet);
                    decidedhands.Add(hr);
                    break;
                }
                if (!hand.IsBusted)
                {
#if DEBUG
                    Console.WriteLine($"Staying with a {hand.Value} against the dealer's {upcard.Rank}.");
#endif
                    hr.SetHandAndPayout(hand, bet);
                    undecidedhands.Add(hr);
                }
            }
        }
    }
}
