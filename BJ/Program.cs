using BJ;

const decimal INITIALBANKROLL = 5000;
const int ROUNDS = 10000;
const int ROUNDSPERHOUR = 100;
const int CONCURRENTPLAYERS = 1000;
const int BETTINGUNIT = 1;
const decimal DECKPEN = 0.7m;
const int SHOESIZE = 2;
const decimal BJPAYOUT = 1.5m;

Console.WriteLine("Barebones Blackjack Simulator!");

decimal runningprofits = 0;
int bankruptcount = 0;
decimal maxprofit = 0;

var rules = new BlackjackRules(SHOESIZE, DECKPEN)
{
    IsSurrenderAllowed = false,
    BlackjackPayout = BJPAYOUT,
    DealerPeeksForBlackjack = true, //NOT IMPLEMENTED ALWAYS CONSIDERED TRUE
    DAS = true, //NOT IMPLEMENTED. ALWAYS CONSIDERED TRUE
    CanHitAcesAfterSplit = false,
    DealerHitsSoft17 = true,
    PairSplitLimit = 3, //NOT IMPLEMENTED - PLAYER CAN SPLIT ANY HAND ANY NUMBER OF TIMES
    SplitAcesLimit = 1  //NOT IMPLEMENTED - PLAYER CAN SPLIT ANY HAND ANY NUMBER OF TIMES
};

Parallel.For(0, CONCURRENTPLAYERS, i =>
{
    var table = new Table(rules);
    var player = new Player(
        INITIALBANKROLL,
        new BettingStrategy(BettingStrategies.BuildBetSpread([0, 0, 0, 10, 20, 30, 40, 60, 100]), [1, 1, 2]),
        new PlayingStrategy(PlayingStrategies.BasicStrategy_HardHand_2D_H17, PlayingStrategies.BasicStrategy_SoftHand_2D_H17, PlayingStrategies.BasicStrategy_Pairs_2D_H17_DAS)
    );

    var bjplayer = new BJPlayer(table, player);

    var bankroll = PlayBlackjack(bjplayer, BETTINGUNIT, ROUNDS);
    var currentprofit = bankroll - INITIALBANKROLL;
    runningprofits += currentprofit;
    if (currentprofit < -INITIALBANKROLL)
        bankruptcount++;
    else if (currentprofit > maxprofit)
        maxprofit = currentprofit;

    Console.WriteLine($"Iteration {i} profits: ${currentprofit}.");
});


var averageprofit = runningprofits / CONCURRENTPLAYERS;
Console.WriteLine($"Average profits: ${averageprofit} over {ROUNDS * CONCURRENTPLAYERS} rounds.");
Console.WriteLine($"Average profit/hour: ${averageprofit / ROUNDSPERHOUR}");
Console.WriteLine($"Bankruptices: {bankruptcount} for a {bankruptcount * 100 / CONCURRENTPLAYERS}% ROR.");
Console.WriteLine($"Max profit: ${maxprofit}");

Console.WriteLine("Press any key to end.");
Console.ReadKey();

decimal PlayBlackjack(BJPlayer bjplayer, int bettingunit, int rounds)
{
    var player = bjplayer.Player;
    var bj = bjplayer.Table;

    for (int i = 0; i < rounds; i++)
    {
        if (player.BankRoll < 0)
            return player.BankRoll;

        var bet = player.BettingStrategy.GetBetAmountAndHands((int)bj.GetTrueCount(), bettingunit);
#if DEBUG
        Console.WriteLine($"Betting ${bet.Bet} on {bet.Hands} hands.");
#endif

        player.AddHands(bet.Hands);

        bj.Deal(player.Hands);
        bjplayer.Play(bet.Bet);
        bj.ClearHands(player);
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


