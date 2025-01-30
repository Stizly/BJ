using BJ;

const decimal INITIALBANKROLL = 10000;
const int ROUNDS = 1000;
const int ROUNDSPERHOUR = 100;
const int CONCURRENTPLAYERS = 5;
const int BETTINGUNIT = 10;
const decimal DECKPEN = 0.75m;
const int SHOESIZE = 2;
const decimal BJPAYOUT = 1.5m;

Console.WriteLine("Barebones Blackjack Simulator!");

decimal runningprofits = 0;
int bankruptcount = 0;
decimal maxprofit = 0;

//Parallel.For(0, CONCURRENTPLAYERS, i =>
//{
var bj = new Blackjack(SHOESIZE)
{
    DeckPenetration = DECKPEN,
    BlackjackPayout = BJPAYOUT,
    DealerPeeksForBlackjack = true,
    DealerHitsSoft17 = true,
    IsSurrenderAllowed = true
};
var player = new Player()
{
    BankRoll = INITIALBANKROLL,
    //BettingStrategy = BettingStrategies.BuildStrategy(1, 1, 1, 1, 3, 5, 10)
    BettingStrategy = BettingStrategies.BuildStrategy(0, 0, 0, 0, 0, 1, 1)
};
var bjplayer = new BJPlayer()
{
    Player = player,
    Blackjack = bj,
    DoSurrender = PlayingStrategies.DoSurrender_2Deck_HS17,
    DoSplit = PlayingStrategies.DoSplit_2Deck_HS17_DAS,
    DoHit = PlayingStrategies.DoHit_2Deck_HS17,
    DoDoubleDown = PlayingStrategies.DoDoubleDown_2Deck_HS17
};


var bankroll = PlayBlackjack(bjplayer, BETTINGUNIT, ROUNDS);
var currentprofit = bankroll - INITIALBANKROLL;
runningprofits += currentprofit;
if (currentprofit <= -INITIALBANKROLL)
    bankruptcount++;
else if (currentprofit > maxprofit)
    maxprofit = currentprofit;

Console.WriteLine($"Iteration {1} profits: ${currentprofit}");
//});
var averageprofit = runningprofits / CONCURRENTPLAYERS;
Console.WriteLine($"Average profits: ${averageprofit}");
Console.WriteLine($"Average profit/hour: ${averageprofit / ROUNDSPERHOUR}");
Console.WriteLine($"Bankruptices: {bankruptcount} for a {bankruptcount * 100 / CONCURRENTPLAYERS}% ROR.");
Console.WriteLine($"Max profit: ${maxprofit}");


decimal PlayBlackjack(BJPlayer bjplayer, int bettingunit, int rounds)
{
    var player = bjplayer.Player;
    var bj = bjplayer.Blackjack;

    for (int i = 0; i < rounds; i++)
    {
        if (player.BankRoll <= 0)
            return player.BankRoll;

        var bet = player.DetermineBet(bj.GetTrueCount(), bettingunit);
#if DEBUG
        Console.WriteLine($"Betting ${bet}.");
#endif


        bj.Deal(player.Hands);
        bjplayer.Play(bet);
        bj.ClearHands(player.Hands);
        if (bj.DoReshuffleShoe())
        {
#if DEBUG
            Console.WriteLine($"Refilling and shuffling shoe!");
#endif
            bj.RefillShoe();
        }

#if DEBUG
        Console.WriteLine($"Player's new Bankroll is ${player.BankRoll}.");
        Console.WriteLine($"RC is {bj.RunningCount} and TC is {bj.GetTrueCount()}.\n\n");
#endif
    }

    return player.BankRoll;
}


